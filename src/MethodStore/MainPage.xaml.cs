using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
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

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x419

namespace MethodStore
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private ObservableCollection<Models.Method> _listMethods = new ObservableCollection<Models.Method>();
        public ObservableCollection<Models.Method> ListMethods { get => _listMethods; }

        public MainPage()
        {
            InitializeComponent();

            DataContext = this;

            _listMethods.Add(new Models.Method() { Group = "<без группы>", Type = "Общий модуль" });
            _listMethods.Add(new Models.Method() { Group = "<без группы>", Type = "Справочник" });

            //DataGridMethods.ite
        }
    }
}
