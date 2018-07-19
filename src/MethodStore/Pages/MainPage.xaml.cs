using Microsoft.Toolkit.Uwp.UI.Controls;
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
        private ParameterSearchEvents _parameterSearchEvents = new ParameterSearchEvents();
        #endregion

        #region Properties
        public ParametersSearch ParametersSearch { get; set; }
        public ObservableCollection<Models.Method> ListMethods { get; } = new ObservableCollection<Models.Method>();
        #endregion

        #region Page events

        public MainPage()
        {
            if (ParametersSearch == null)
                ParametersSearch = new ParametersSearch(_parameterSearchEvents);

            InitializeComponent();

            _parameterSearchEvents.ChangedItemSearch += _parameterSearchEvents_ChangedItemSearch;

            SystemNavigationManager.GetForCurrentView().BackRequested += (s, e) =>
            {
                TryGoBack(e);

                FillListMethods();
            };
        }

        private void PageMainPage_Loaded(object sender, RoutedEventArgs e)
        {
            FillListMethods();

            if (_selectedItemMethod != null)
            {
                DataGridMethods.SelectedItem = ListMethods.SingleOrDefault(f => f.ID == _selectedItemMethod.ID);
            }
            ApplicationView.GetForCurrentView().Title = string.Empty;
        }

        private void PageMainPage_Unloaded(object sender, RoutedEventArgs e)
        {
            ParametersSearch?.Dispose();
        }

        #endregion

        #region Button

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            NavigatingPage(typeof(PageMethod));
        }

        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            OpenMethodFromCurrentRowDataGrid();
        }

        private async void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridMethods.SelectedItem is Models.Method method)
            {
                if (await Messages.Question($"Удалить метод {method.MethodInvokationString}?") == ContentDialogResult.Primary)
                    new EF.Context<Models.Method>().RemoveMethods(method);
            }
            FillListMethods();
        }

        #endregion

        #region Navigating

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is ParametersNavigating parametersNav)
            {
                _selectedItemMethod = parametersNav[0] as Models.Method;
            }
        }

        private void NavigatingPage(Type typePage, object param = null)
        {
            ParametersSearch?.SaveSettings();
            Navigating.Navigate(typePage, param);
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

        #endregion

        private void FillListMethods()
        {
            ListMethods.Clear();
            List<Models.Method> methods = new EF.Context<Models.Method>().GetListMethods(ParametersSearch);
            if (methods != null)
                foreach (Models.Method item in methods)
                    ListMethods.Add(item);
        }

        private void _parameterSearchEvents_ChangedItemSearch()
        {
            FillListMethods();
        }

        private void DataGridMethods_CharacterReceived(UIElement sender, CharacterReceivedRoutedEventArgs args)
        {
            if (args.KeyStatus.ScanCode > 1)
                if (args.KeyStatus.ScanCode == 14)
                    ParametersSearch.Text = ParametersSearch.Text.RemoveChars(1);
                else
                {
                    string pathColumn = ((DataGrid)sender).CurrentColumn?.ClipboardContentBinding.Path.Path;

                    switch (pathColumn)
                    {
                        case "Group":
                            ParametersSearch.SearchInGroup = true;
                            break;
                        case "Type":
                            ParametersSearch.SearchInType = true;
                            break;
                        case "ObjectName":
                            ParametersSearch.SearchInObjectName = true;
                            break;
                        case "MethodName":
                            ParametersSearch.SearchInMethodName = true;
                            break;
                    }

                    ParametersSearch.Text += args.Character.ToString();
                }
        }

        private void DataGridMethods_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            OpenMethodFromCurrentRowDataGrid();
        }

        private void OpenMethodFromCurrentRowDataGrid()
        {
            object idSelectedMethod = null;
            if (DataGridMethods.SelectedItem is Models.Method method)
                idSelectedMethod = method.ID;

            if (idSelectedMethod == null)
                return;

            NavigatingPage(typeof(PageMethod), idSelectedMethod);
        }

    }
}
