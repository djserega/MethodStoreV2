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
    public sealed partial class PageType : Page
    {
        public Models.Types Types { get; set; }
        private Models.Method _parentMethod;

        public PageType()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is ParametersNavigating parametersNav)
            {
                if (parametersNav[0] is int id)
                {
                    Types = new EF.Context<Models.Types>().FindByID(id);
                }
                else
                {
                    _parentMethod = parametersNav[0] as Models.Method;
                }
            }

            if (Types == null)
                Types = new Models.Types();
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            new EF.Context<Models.Types>().UpdateMethods(Types);

            TryBack(_parentMethod, Types);
        }

        private void TryBack(params object[] param)
        {
            Navigating.Navigate(typeof(PageMethod), param);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            TextBoxName.Focus(FocusState.Programmatic);
            ApplicationView.GetForCurrentView().Title = "Создание типа";
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            TryBack(_parentMethod);
        }
    }
}
