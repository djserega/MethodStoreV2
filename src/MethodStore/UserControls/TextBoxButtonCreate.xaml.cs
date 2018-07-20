using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Документацию по шаблону элемента "Пользовательский элемент управления" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234236

namespace MethodStore
{
    internal delegate void TextChanged();
    internal delegate void ClickNew();
    internal delegate void ClickSelect();
    internal delegate void ClickOpen();

    public sealed partial class TextBoxButtonCreate : UserControl, INotifyPropertyChanged
    {
        public TextBoxButtonCreate()
        {
            InitializeComponent();
        }

        public string Header { get; set; }
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set
            {
                SetValue(TextProperty, value);
                NotifyPropertyChanged();
            }
        }
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(TextBox), null);

        internal event TextChanged TextChanged;
        private void TextBoxtText_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextChanged?.Invoke();
        }

        internal event ClickNew ClickNew;
        private void ButtonNew_Click(object sender, RoutedEventArgs e)
        {
            ClickNew?.Invoke();
        }
       
        internal event ClickSelect ClickSelect;
        private void ButtonSelect_Click(object sender, RoutedEventArgs e)
        {
            ClickSelect?.Invoke();
        }

        internal event ClickOpen ClickOpen;
        private void ButtonOpen_Click(object sender, RoutedEventArgs e)
        {
            ClickOpen?.Invoke();
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
