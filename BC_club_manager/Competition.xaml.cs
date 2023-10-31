using Paragraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
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
    /// Competition.xaml 的交互逻辑
    /// </summary>
    public partial class Competition : Window
    {

        List<UserControl2> competitionsBio;
        List<UserControl2> competitionsChe;

        public Competition()
        {
            InitializeComponent();
            competitionsBio = new List<UserControl2>();
            competitionsChe = new List<UserControl2>();
        }

        private void AddBioButton_Click(object sender, RoutedEventArgs e)
        {
            UserControl2 competitionShower = new UserControl2();
            dataContainerBio.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(300) });
            Grid.SetColumn(competitionShower, 0);
            Grid.SetRow(competitionShower, competitionsBio.Count);
            competitionShower.index = competitionsBio.Count;
            competitionShower.OnDelete += OnBioDeleteEvent;
            competitionsBio.Add(competitionShower);
            dataContainerBio.Children.Add(competitionShower);
        }

        private void OnBioDeleteEvent(int index)
        {
            dataContainerBio.Children.Remove(competitionsBio[index]);
            competitionsBio.RemoveAt(index);
            dataContainerBio.RowDefinitions.RemoveAt(index);
            for (int i = index; i < competitionsBio.Count; i++)
            {
                competitionsBio[i].index = i;
                Grid.SetRow(competitionsBio[i], i);
            }
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect("101.43.188.94", 32347);
            NetworkStream stream = new NetworkStream(socket);

            JSONTemplete templete = new JSONTemplete();
            templete.bio_info = new TextValueTemplete[competitionsBio.Count];
            templete.che_info = new TextValueTemplete[competitionsChe.Count];
            for (int i = 0; i < competitionsBio.Count; i++)
            {
                templete.bio_info[i] = competitionsBio[i].GetText();
            }
            for (int i = 0; i < competitionsChe.Count; i++)
            {
                templete.che_info[i] = competitionsChe[i].GetText();
            }
            string json = JsonSerializer.Serialize(templete);
            byte[] jsonBytes = Encoding.UTF8.GetBytes(json);
            byte[] jsonLengthBytes = BitConverter.GetBytes((uint)jsonBytes.Length);
            byte[] method = { 2 };
            stream.Write(method);
            stream.Write(jsonLengthBytes);
            stream.Write(jsonBytes);
            stream.Read(new byte[1]);
            stream.Close();
            socket.Close();
            MessageBox.Show("发送成功");
        }

        private void AddCheButton_Click(object sender, RoutedEventArgs e)
        {
            UserControl2 competitionShower = new UserControl2();
            dataContainerChe.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(300) });
            Grid.SetColumn(competitionShower, 0);
            Grid.SetRow(competitionShower, competitionsChe.Count);
            competitionShower.index = competitionsChe.Count;
            competitionShower.OnDelete += OnCheDeleteEvent;
            competitionsChe.Add(competitionShower);
            dataContainerChe.Children.Add(competitionShower);
        }

        private void OnCheDeleteEvent(int index)
        {
            dataContainerChe.Children.Remove(competitionsChe[index]);
            competitionsChe.RemoveAt(index);
            dataContainerChe.RowDefinitions.RemoveAt(index);
            for (int i = index; i < competitionsChe.Count; i++)
            {
                competitionsChe[i].index = i;
                Grid.SetRow(competitionsChe[i], i);
            }
        }
    }

    public class JSONTemplete
    {
        public TextValueTemplete[]? bio_info { get; set; }
        public TextValueTemplete[]? che_info { get; set; }
    }
}
