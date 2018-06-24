using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
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
        private Models.Method _selectedItemMethod;
        public ParametersSearch ParametersSearch { get; set; }

        private ObservableCollection<Models.Method> _listMethods = new ObservableCollection<Models.Method>();
        public ObservableCollection<Models.Method> ListMethods { get => _listMethods; }

        public MainPage()
        {
            InitializeComponent();

            SystemNavigationManager.GetForCurrentView().BackRequested += MainPage_BackRequested;
        }

        private void PageMainPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (ParametersSearch == null)
                ParametersSearch = new ParametersSearch();

            ApplicationView.GetForCurrentView().TryResizeView(new Size(1200, 600));
            FillListMethods();

            if (_selectedItemMethod != null)
            {
                DataGridMethods.SelectedItem = _listMethods.Single(f => f.ID == _selectedItemMethod.ID);
            }
        }

        private void FillListMethods()
        {
            using (EF.MethodStoreContext context = new EF.MethodStoreContext())
            {
                _listMethods.Clear();
                List<Models.Method> methods = context.Methods.ToList();
                methods.Sort((a, b) => b.ID.CompareTo(a.ID));
                foreach (Models.Method item in methods)
                {
                    _listMethods.Add(item);
                }
            }
        }

        private void MainPage_BackRequested(object sender, BackRequestedEventArgs e)
        {
            TryGoBack(e);

            FillListMethods();

            SetVisiblilityBackButton();
        }

        private bool TryGoBack(BackRequestedEventArgs e = null)
        {
            bool result = false;
            
            if (Frame.CanGoBack)
            {
                if (e != null)
                    e.Handled = true;

                Frame.GoBack();

                result = true;
            }

            return result;
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            Navigating(typeof(PageMethod));

            SetVisiblilityBackButton();
        }

        private void SetVisiblilityBackButton()
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = Frame.CanGoBack ? AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;
        }

        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            object idSelectedMethod = null;
            if (DataGridMethods.SelectedItem is Models.Method method)
                idSelectedMethod = method.ID;

            Navigating(typeof(PageMethod), idSelectedMethod);

            SetVisiblilityBackButton();
        }

        private async void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridMethods.SelectedItem is Models.Method method)
            {
                if (await Messages.Question($"Удалить метод {method.MethodInvokationString}?") == ContentDialogResult.Primary)
                {
                    using (EF.MethodStoreContext context = new EF.MethodStoreContext())
                    {
                        context.Remove(method);
                        context.SaveChanges();
                    }
                }
            }
            FillListMethods();

            SetVisiblilityBackButton();
        }
   
        private void Navigating(Type typePage, object param = null)
        {
            ParametersNavigating parameters = new ParametersNavigating()
            {
                parameters = param
            };

            ParametersSearch.Dispose();

            Frame.Navigate(typePage, parameters);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is ParametersNavigating parametersNav)
            {
                if (parametersNav.parameters is Models.Method method)
                {
                    _selectedItemMethod = method;
                }
            }
        }

        private void PageMainPage_Unloaded(object sender, RoutedEventArgs e)
        {
            ParametersSearch?.Dispose();
        }
    }
}
