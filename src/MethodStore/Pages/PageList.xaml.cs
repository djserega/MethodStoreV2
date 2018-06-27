using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace MethodStore
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class PageList : Page
    {
        private Models.Method _parentMethod;
        private Type _typeList;

        public PageList()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is ParametersNavigating parametersNav)
            {
                _parentMethod = parametersNav[0] as Models.Method;

                if (parametersNav[1] is List<Models.Group> listGroup)
                {
                    ListView.ItemsSource = listGroup;
                    _typeList = typeof(Models.Group);
                }
                else if (parametersNav[1] is List<Models.Types> listType)
                {
                    ListView.ItemsSource = listType;
                    _typeList = typeof(Models.Types);
                }
            }
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            TryBack(_parentMethod);
        }

        private void ButtonSelect_Click(object sender, RoutedEventArgs e)
        {
            TryBack(_parentMethod, ListView.SelectedItem);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ListView.Focus(FocusState.Programmatic);

            string newTitle = string.Empty;
            if (_typeList == typeof(Models.Group))
                newTitle = "Выбор группы";
            else if (_typeList == typeof(Models.Types))
                newTitle = "Выбор типа";
            ApplicationView.GetForCurrentView().Title = newTitle;
        }

        private void TryBack(params object[] param)
        {
            Navigating.Navigate(typeof(PageMethod), param);
        }


    }
}
