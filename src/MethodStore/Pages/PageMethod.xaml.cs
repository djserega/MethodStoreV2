﻿using System;
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
    public sealed partial class PageMethod : Page
    {
        public Models.Method Method { get; set; }

        public PageMethod()
        {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Enabled;
        }

        private void PageMethodPage_Loaded(object sender, RoutedEventArgs e)
        {
            ApplicationView.GetForCurrentView().TryResizeView(new Size(1200, 600));
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is ParametersNavigating parametersNav)
            {
                if (parametersNav.parameters is int id)
                {
                    Method = new EF.Context<Models.Method>().FindByID(id);
                }
            }

            if (Method == null)
                Method = new Models.Method();
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            SaveObject();
        }

        private void SaveObject()
        {
            new EF.Context<Models.Method>().UpdateMethods(Method);
        }

        private void ButtonSaveAndClose_Click(object sender, RoutedEventArgs e)
        {
            SaveObject();

            TryBack();
        }

        private void TryBack()
        {
            Navigating.Navigate(typeof(MainPage), new ParametersNavigating() { parameters = Method });
        }

        private void TextBoxGroup_ClickNew()
        {
            Navigating.Navigate(typeof(PageGroup));
        }

        private void TextBoxType_ClickNew()
        {

        }

        private void TextBoxObjectName_ClickNew()
        {

        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            TryBack();
        }
    }
}
