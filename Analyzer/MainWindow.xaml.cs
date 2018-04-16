using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace Analyzer
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            SenderListBox.SelectionChanged += SenderListBox_SelectionChanged;

            var dpd = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(DataGrid));
            if (dpd != null)
            {
                dpd.AddValueChanged(ServerReceiverListBox, AnalizeIfPossible);
                dpd.AddValueChanged(ReceiverListBox, AnalizeIfPossible);
                dpd.AddValueChanged(SenderListBox, AnalizeIfPossible);
            }
        }

        private void AnalizeIfPossible(object sender, EventArgs e)
        {
            if (SenderListBox.Items.Count == 0 || ServerReceiverListBox.Items.Count == 0 || ReceiverListBox.Items.Count == 0)
                return;
            foreach (var senderItem in senderList)
            {
                var foundItem = false;
                foreach (var receiverItem in receiverList)
                {
                    foreach (var item in receiverItem.ServerMessage.G)
                    {
                        if (senderItem.g == item)
                        {
                            foundItem = true;
                            break;
                        }
                    }
                    if (foundItem)
                        break;
                }

                if (!foundItem) senderItem.BackgroundBrush = Brushes.PaleVioletRed;
                else
                {
                    senderItem.BackgroundBrush = Brushes.LightGreen;
                    continue;
                }

                foreach (var serverSenderItem in serverSenderList)
                {
                    foreach (var serverSenderGuid in serverSenderItem.ServerMessage.G)
                    {
                        if (serverSenderGuid == senderItem.g)
                        {
                            foundItem = true;
                            break;
                        }
                    }
                    if (foundItem)
                        break;

                }
                if (foundItem)
                    continue;

                senderItem.BackgroundBrush = Brushes.DarkRed;
                foreach (var serverReceiverItem in serverReceiverList)
                {
                    if (serverReceiverItem.g != senderItem.g)
                        continue;
                    foundItem = true;
                    senderItem.BackgroundBrush = Brushes.Red;
                    break;

                }
                if (!foundItem)
                    senderItem.BackgroundBrush = Brushes.DimGray;


            }
            SenderListBox.Items.Refresh();
            ToServerReceiverFailCounter.Content = PercentageOfBrushInList(Brushes.DimGray);
            ToServerSenderFailCounter.Content = PercentageOfBrushInList(Brushes.Red);
            ToClientReceiverFailCounter.Content = PercentageOfBrushInList(Brushes.PaleVioletRed);
            ToClientSuccessCounter.Content = PercentageOfBrushInList(Brushes.LightGreen);

        }

        private void SenderListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (ServerReceiverListBox.Items.Count == 0 || ReceiverListBox.Items.Count == 0 || SenderListBox.SelectedItem == null)
                return;

            ServerReceiverListBox.SelectedItems.Clear();
            ServerReceiverListBox.SelectedItems.Add(serverReceiverList.Find(x =>
                x.g == ((ClientMessageWithTimeStamp)SenderListBox.SelectedItem).g));
            ServerReceiverListBox.ScrollIntoView(ServerReceiverListBox.SelectedItem);

            ServerSenderListBox.SelectedItems.Clear();
            ServerSenderListBox.SelectedItems.Add(serverSenderList.Find(x =>
                x.ServerMessage.G.Any(y => y == ((ClientMessageWithTimeStamp)SenderListBox.SelectedItem).g)));
            ServerSenderListBox.ScrollIntoView(ServerSenderListBox.SelectedItem);

            ReceiverListBox.SelectedItems.Clear();
            ReceiverListBox.SelectedItems.Add(receiverList.Find(x =>
                x.ServerMessage.G.Any(y => y == ((ClientMessageWithTimeStamp)SenderListBox.SelectedItem).g)));
            ReceiverListBox.ScrollIntoView(ReceiverListBox.SelectedItem);

            var selectedItem = ((ClientMessageWithTimeStamp)SenderListBox.SelectedItem);
            InfoLabel.Text =
                "Position: x: " + selectedItem.ClientMessage.X + "  y: " + selectedItem.ClientMessage.Y + "\n" +
                "Map name: " + selectedItem.ClientMessage.MN + "\n" +
                "Action List:" + getActionListInString(selectedItem.ClientMessage.AL) + "\n" +
                "Mob List: " + getMobListInString(selectedItem.ClientMessage.ML) + "\n" +
                "Guid: " + selectedItem.ClientMessage.g + "\n";



        }

        private string getActionListInString(List<CharAction> actionList)
        {
            string temp = "";
            foreach (var charAction in actionList)
            {
                temp += "\n  Name: " + charAction.A + ", Object: " + charAction.Oc + ", Sender: " + charAction.Sn +
                        ", Values: " + charAction.V1 + " " + charAction.V2 + " " + charAction.V3 + " " + charAction.V4;
            }
            return temp;
        }

        private string getMobListInString(List<PrivateMob> mobList)
        {
            if (mobList == null) return "";
            string temp = "";
            foreach (var mob in mobList)
            {
                temp += "\n  Name: " + mob.N + ", Health: " + mob.Hp + ", Type: " + mob.T +
                        ", Values: " + mob.X + " " + mob.Y;
            }
            return temp;
        }
        private string senderName;
        List<ClientMessageWithTimeStamp> senderList;
        List<ClientMessageWithTimeStamp> serverReceiverList;
        List<ServerMessageWithTimestamp> serverSenderList;
        List<ServerMessageWithTimestamp> receiverList;

        private void SenderButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() != true)
                return;
            var tempStringTab = File.ReadAllLines(openFileDialog.FileName);
            senderList = new List<ClientMessageWithTimeStamp>();

            foreach (var s in tempStringTab)
            {
                if (!s.Contains("Sent to Server"))
                    continue;
                var temp = JsonConvert.DeserializeObject<ClientMessage>(s.Split(' ')[5].TrimEnd('@'));
                var withTime = new ClientMessageWithTimeStamp(temp,
                    DateTime.ParseExact((s.Split(' ')[0] + " " + s.Split(' ')[1]), "yyyy-MM-dd HH:mm:ss,fff",
                        CultureInfo.InvariantCulture));
                senderList.Add(withTime);
            }
            SenderListBox.ItemsSource = senderList;
        }

        private void ServerSelectFileButton_Click(object sender, RoutedEventArgs e)
        {
            serverReceiverList = new List<ClientMessageWithTimeStamp>();
            serverSenderList = new List<ServerMessageWithTimestamp>();
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                var tempStringTab = File.ReadAllLines(openFileDialog.FileName);
                foreach (var s in tempStringTab)
                {
                    if (s.Split(' ')[2].Contains("Received"))
                    {
                        try
                        {
                            var temp = JsonConvert.DeserializeObject<ClientMessage>(s.Split(' ')[5].TrimEnd('@'));
                            var withTime = new ClientMessageWithTimeStamp(temp,
                                DateTime.ParseExact((s.Split(' ')[0] + " " + s.Split(' ')[1]), "yyyy-MM-dd HH:mm:ss,fff",
                                    CultureInfo.InvariantCulture));
                            serverReceiverList.Add(withTime);
                        }
                        catch (Exception exception)
                        {
                            //LogAnError(exception);

                        }

                    }
                    if (s.Split(' ')[2].Contains("Sent"))
                    {
                        try
                        {
                            var temp = JsonConvert.DeserializeObject<ServerMessage>(s.Split(' ')[5].TrimEnd('@'));
                            var withTime = new ServerMessageWithTimestamp(temp,
                                DateTime.ParseExact((s.Split(' ')[0] + " " + s.Split(' ')[1]), "yyyy-MM-dd HH:mm:ss,fff",
                                    CultureInfo.InvariantCulture));
                            serverSenderList.Add(withTime);
                        }
                        catch (Exception exception)
                        {
                            //LogAnError(exception);
                        }

                    }
                }
                ServerReceiverListBox.ItemsSource = serverReceiverList;
                ServerSenderListBox.ItemsSource = serverSenderList;
            }
        }

        private void ReceiverSelectFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                var tempStringTab = File.ReadAllLines(openFileDialog.FileName);
                receiverList = new List<ServerMessageWithTimestamp>();
                foreach (var s in tempStringTab)
                {
                    if (s.Contains("Received from Server"))
                    {
                        var temp = JsonConvert.DeserializeObject<ServerMessage>(s.Split(' ')[5].TrimEnd('@'));
                        var withTime = new ServerMessageWithTimestamp(temp,
                            DateTime.ParseExact((s.Split(' ')[0] + " " + s.Split(' ')[1]), "yyyy-MM-dd HH:mm:ss,fff",
                                CultureInfo.InvariantCulture));
                        receiverList.Add(withTime);
                    }
                }
                ReceiverListBox.ItemsSource = receiverList;

            }
        }


        private string PercentageOfBrushInList(SolidColorBrush brush)
        {
            return ((Convert.ToDouble(senderList.Count(x => x.BackgroundBrush == brush)) / senderList.Count) * 100).ToString("0.##") + "%";
        }
    }
}
