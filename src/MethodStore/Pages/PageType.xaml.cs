using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
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
        #region Fields
        private Models.Method _parentMethod;
        private Type _backPage;
        #endregion

        #region Properties
        public Models.Types Types { get; set; }
        #endregion

        #region Constructors

        public PageType()
        {
            InitializeComponent();
        }

        #endregion

        #region Page events

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            TextBoxName.Focus(FocusState.Programmatic);
            if (Types.ID == 0)
                ApplicationView.GetForCurrentView().Title = "Создание типа";
            else
                ApplicationView.GetForCurrentView().Title = "Тип";
        }

        private void Page_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Escape)
            {
                TryBack(_parentMethod);
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is ParametersNavigating parametersNav)
            {
                if (parametersNav[0] is int id)
                {
                    Types = new EF.Context<Models.Types>().FindByID(id);
                    _parentMethod = parametersNav[1] as Models.Method;
                }
                else if (parametersNav[0] is string typeName)
                {
                    Types = new EF.Context<Models.Types>().FindByName(typeName);
                    _parentMethod = parametersNav[1] as Models.Method;
                }
                else
                {
                    if (parametersNav[0] is Type typeParam)
                    {
                        _backPage = typeParam;

                        _parentMethod = parametersNav[1] as Models.Method;
                        if (parametersNav[2] != null)
                            Types = new EF.Context<Models.Types>().FindByID((int)parametersNav[2]);
                    }
                    else
                        _parentMethod = parametersNav[0] as Models.Method;
                }
            }

            if (Types == null)
                Types = new Models.Types();
        }

        #endregion

        #region Button

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            TryBack(_parentMethod);
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            new EF.Context<Models.Types>().Update(Types);

            TryBack(_parentMethod, Types);
        }

        #endregion

        private void TryBack(params object[] param)
        {
            if (_backPage == null)
                Navigating.Navigate(typeof(PageMethod), param);
            else
            {
                object[] paramNavigate = new object[param.Count() + 1];
                for (int i = 0; i < param.Count(); i++)
                    paramNavigate[i] = param[i];

                paramNavigate[param.Count()] = new EF.Context<Models.Types>().GetList();

                Navigating.Navigate(_backPage, paramNavigate);
            }
        }

    }
}
