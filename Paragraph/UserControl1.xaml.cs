using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        public UserControl1()
        {
            InitializeComponent();
            Method = ShowType.Text;
            Data = "";
        }

        public int index;
        public delegate void DeleteEvent(int index);
        public DeleteEvent? OnDelete;
        public string Data
        {
            get; private set;
        }

        public ShowType Method
        {
            get; private set;
        }
        

        private string GetInnerText()
        {
            return new TextRange(TextBox.Document.ContentStart, TextBox.Document.ContentEnd).Text;
        }
        
        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            OnDelete?.Invoke(index);
        }

        private void TextButton_Click(object sender, RoutedEventArgs e)
        {
            ImageBox.Visibility = Visibility.Collapsed;
            FileBox.Visibility = Visibility.Collapsed;
            TextBox.Visibility = Visibility.Visible;
            Method = ShowType.Text;
            TextButton.IsEnabled = false;
            FileButton.IsEnabled = true;
            ImageButton.IsEnabled = true;
        }

        private void ImageButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "常见图片格式(*.jpg,*.png,*.bmp,*.gif,*.JPG,*.PNG,*.BMP,*.GIF)|*.jpg;*.png;*.bmp;*.gif;*.JPG;*.PNG;*.BMP;*.GIF|所有文件(*.*)|*.*";
            fileDialog.Multiselect = false;
            if (fileDialog.ShowDialog() == false)
            {
                return;
            }
            Data = fileDialog.FileName;
            ImageBox.Source = BitmapFrame.Create(new Uri(Data));
            TextBox.Visibility = Visibility.Collapsed;
            FileBox.Visibility = Visibility.Collapsed;
            ImageBox.Visibility = Visibility.Visible;
            ImageButton.IsEnabled = false;
            FileButton.IsEnabled = true;
            TextButton.IsEnabled = true;
            Method = ShowType.Image;
        }

        public static string GetFileName(string url)
        {
            return url.Substring(url.LastIndexOf('\\') + 1);
        }

        private void FileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "所有文件(*.*)|*.*";
            fileDialog.Multiselect = false;
            if (fileDialog.ShowDialog() == false)
            {
                return;
            }
            Data = fileDialog.FileName;
            FileBox.Content = GetFileName(Data);
            TextBox.Visibility = Visibility.Collapsed;
            ImageBox.Visibility = Visibility.Collapsed;
            FileBox.Visibility = Visibility.Visible;
            Method = ShowType.File;
            FileButton.IsEnabled = false;
            ImageButton.IsEnabled = true;
            TextButton.IsEnabled = true;
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            ImageBox.Visibility = Visibility.Collapsed;
            FileBox.Visibility = Visibility.Collapsed;
            TextBox.Visibility = Visibility.Visible;
            Method = ShowType.Text;
            TextButton.IsEnabled = false;
            FileButton.IsEnabled = true;
            ImageButton.IsEnabled = true;
            Data = "";
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Data = GetInnerText();
        }
    }

    public enum ShowType
    {
        Text = 0,
        Image = 1,
        File = 2
    }
}