using System;
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
        private FrameGoBackEvents _frameGoBackEvent;

        private ObservableCollection<Models.Method> _listMethods = new ObservableCollection<Models.Method>();
        public ObservableCollection<Models.Method> ListMethods { get => _listMethods; }

        public MainPage()
        {
            InitializeComponent();

            _frameGoBackEvent = new FrameGoBackEvents();
            _frameGoBackEvent.FrameGoBackEvent += _frameGoBackEvent_FrameGoBackEvent;

            SystemNavigationManager.GetForCurrentView().BackRequested += MainPage_BackRequested;
        }

        private void _frameGoBackEvent_FrameGoBackEvent()
        {
            TryGoBack();
        }

        private void PageMainPage_Loaded(object sender, RoutedEventArgs e)
        {
            ApplicationView.GetForCurrentView().TryResizeView(new Size(1200, 600));
            FillListMethods();
        }

        private void FillListMethods()
        {
            using (EF.MethodStoreContext context = new EF.MethodStoreContext())
            {
                _listMethods.Clear();
                foreach (Models.Method item in context.Methods.ToList())
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

        private void TryGoBack(BackRequestedEventArgs e = null)
        {
            if (Frame.CanGoBack)
            {
                if (e != null)
                    e.Handled = true;

                Frame.GoBack();
            }
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
                frameGoBackEvents = _frameGoBackEvent,
                parameters = param
            };

            Frame.Navigate(typePage, parameters);
        }

    }
}
