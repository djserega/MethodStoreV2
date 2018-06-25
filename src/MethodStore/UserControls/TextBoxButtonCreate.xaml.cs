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
    internal delegate void ClickNew();
    internal delegate void ClickSelect();

    public sealed partial class TextBoxButtonCreate : UserControl, INotifyPropertyChanged
    {
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

        public TextBoxButtonCreate()
        {
            InitializeComponent();
        }

        internal event ClickNew ClickNew;
        private void ButtonNew_Click(object sender, RoutedEventArgs e)
        {
            ClickNew?.Invoke();
        }
       
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        internal event ClickSelect ClickSelect;
        private void ButtonSelect_Click(object sender, RoutedEventArgs e)
        {
            ClickSelect?.Invoke();
        }
    }
}
