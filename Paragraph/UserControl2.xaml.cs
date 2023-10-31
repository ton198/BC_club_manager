using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Paragraph
{
    /// <summary>
    /// UserControl2.xaml 的交互逻辑
    /// </summary>
    public partial class UserControl2 : UserControl
    {

        public delegate void DeleteEvent(int index);
        public event DeleteEvent? OnDelete;
        public int index;

        public UserControl2()
        {
            InitializeComponent();
        }

        public TextValueTemplete GetText()
        {
            TextValueTemplete templete = new()
            {
                name = NameText.Text,
                time = TimeText.Text,
                detail = new TextRange(DetailText.Document.ContentStart, DetailText.Document.ContentEnd).Text
            };
            return templete;
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {

            OnDelete?.Invoke(index);
        }
    }

    public class TextValueTemplete
    {
        public string? name { get; set; }
        public string? time { get; set; }
        public string? detail { get; set; }
    }
}
