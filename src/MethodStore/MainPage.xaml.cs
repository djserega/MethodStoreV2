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
        #region Fields
        private Models.Method _selectedItemMethod;
        private ObservableCollection<Models.Method> _listMethods = new ObservableCollection<Models.Method>();
        #endregion

        #region Properties
        public ParametersSearch ParametersSearch { get; set; }
        public ObservableCollection<Models.Method> ListMethods { get => _listMethods; }
        #endregion

        #region Page events

        public MainPage()
        {
            if (ParametersSearch == null)
                ParametersSearch = new ParametersSearch();

            InitializeComponent();

            SystemNavigationManager.GetForCurrentView().BackRequested += (s, e) => 
            {
                TryGoBack(e);

                FillListMethods();

                SetVisiblilityBackButton();
            };
        }

        private void PageMainPage_Loaded(object sender, RoutedEventArgs e)
        {
            ApplicationView.GetForCurrentView().TryResizeView(new Size(1200, 600));
            FillListMethods();

            if (_selectedItemMethod != null)
            {
                DataGridMethods.SelectedItem = _listMethods.Single(f => f.ID == _selectedItemMethod.ID);
            }
        }

        private void PageMainPage_Unloaded(object sender, RoutedEventArgs e)
        {
            ParametersSearch?.Dispose();
        }

        #endregion

        #region Button

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            Navigating(typeof(PageMethod));

            SetVisiblilityBackButton();
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
                    new EF.Context().RemoveMethods(method);
            }
            FillListMethods();

            SetVisiblilityBackButton();
        }

        #endregion

        #region Navigating

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

        private void Navigating(Type typePage, object param = null)
        {
            ParametersNavigating parameters = new ParametersNavigating()
            {
                parameters = param
            };

            ParametersSearch.Dispose();

            Frame.Navigate(typePage, parameters);
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

        private void SetVisiblilityBackButton()
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = Frame.CanGoBack ? AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;
        }

        #endregion

        private void FillListMethods()
        {
            _listMethods.Clear();
            foreach (Models.Method item in new EF.Context().GetListMethods(ParametersSearch))
            {
                _listMethods.Add(item);
            }

        }

    }
}
