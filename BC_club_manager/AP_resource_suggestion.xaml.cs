using Paragraph;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BC_club_manager
{
    /// <summary>
    /// AP_resource_suggestion.xaml 的交互逻辑
    /// </summary>
    public partial class AP_resource_suggestion : Window
    {
        readonly List<UserControl1> paragraphs;
        private Target submitTarget = Target.BiologyResource;

        public AP_resource_suggestion()
        {
            InitializeComponent();
            paragraphs = new List<UserControl1>();
        }

        private void AddParagraphButton_Click(object sender, RoutedEventArgs e)
        {
            UserControl1 paragraph1 = new UserControl1();
            dataContainer.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(300)});
            Grid.SetColumn(paragraph1, 0);
            Grid.SetRow(paragraph1, paragraphs.Count);
            paragraph1.index = paragraphs.Count;
            paragraph1.OnDelete += OnDeleteEvent;
            paragraphs.Add(paragraph1);
            dataContainer.Children.Add(paragraph1);
        }

        private void OnDeleteEvent(int index)
        {
            dataContainer.Children.Remove(paragraphs[index]);
            paragraphs.RemoveAt(index);
            dataContainer.RowDefinitions.RemoveAt(index);
            for (int i = index;i < paragraphs.Count;i++)
            {
                paragraphs[i].index = i;
                Grid.SetRow(paragraphs[i], i);
            }
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect("101.43.188.94", 32347);
            NetworkStream stream = new NetworkStream(socket);
            string innerHTML = "";
            int totalPacket = 0;
            foreach (UserControl1 paragraph in paragraphs)
            {
                switch (paragraph.Method)
                {
                    case ShowType.Text:
                        innerHTML += "<p>\n" + paragraph.Data + "</p><br>\n";
                        break;
                    case ShowType.Image:
                        string imageName = UserControl1.GetFileName(paragraph.Data);
                        innerHTML += "<img src=\"/res/" + imageName + "\">" + imageName + "</img><br>\n";
                        totalPacket++;
                        SendFile(paragraph.Data, imageName, stream);
                        break;
                    case ShowType.File:
                        string fileName = UserControl1.GetFileName(paragraph.Data);
                        innerHTML += "<a href=\"/res/" + fileName + "\">" + fileName + "</a><br>\n";
                        totalPacket++;
                        SendFile(paragraph.Data, fileName, stream);
                        break;
                }
            }
            totalPacket++;

            byte[] method = { 0 };
            byte[] targetByte = { (byte)submitTarget };
            byte[] htmlBytes = Encoding.UTF8.GetBytes(innerHTML);
            byte[] htmlLengthBytes = BitConverter.GetBytes((uint)htmlBytes.Length);
            stream.Write(method);
            stream.Write(targetByte);
            stream.Write(htmlLengthBytes);
            stream.Write(htmlBytes);


            byte[] b = new byte[totalPacket];
            stream.Read(b, 0, b.Length);
            Console.WriteLine("已全部收到，准备关闭连接");
            this.Dispatcher.Invoke(new Action(() =>
            {
                MessageBox.Show("发送成功");
            }));
            stream.Close();
            socket.Close();
            stream.Dispose();
            socket.Dispose();
        }

        private static void SendFile(string path, string fileName, NetworkStream stream)
        {
            byte[] imageNameBytes = Encoding.UTF8.GetBytes(fileName);
            byte[] imageNameLengthBytes = BitConverter.GetBytes((uint)imageNameBytes.Length);
            byte[] method = { 1 };
            stream.Write(method);
            stream.Write(imageNameLengthBytes);
            stream.Write(imageNameBytes);
            

            byte[] fileBytes = File.ReadAllBytes(path);
            byte[] fileLengthBytes = BitConverter.GetBytes((uint)fileBytes.Length);
            stream.Write(fileLengthBytes);
            stream.Write(fileBytes);
        }

        private void BioResCheck_Checked(object sender, RoutedEventArgs e)
        {
            submitTarget = Target.BiologyResource;
        }

        private void BioSugCheck_Checked(object sender, RoutedEventArgs e)
        {
            submitTarget = Target.BiologySuggestion;
        }

        private void CheResCheck_Checked(object sender, RoutedEventArgs e)
        {
            submitTarget = Target.ChemistryResource;
        }

        private void CheSugCheck_Checked(object sender, RoutedEventArgs e)
        {
            submitTarget = Target.ChemistrySuggestion;
        }
    }

    enum Target
    {
        BiologyResource = 0,
        BiologySuggestion = 1,
        ChemistryResource = 2,
        ChemistrySuggestion = 3
    }
}
