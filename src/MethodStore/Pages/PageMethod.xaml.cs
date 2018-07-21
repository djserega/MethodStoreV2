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
    public sealed partial class PageMethod : Page
    {
        #region Constructors

        public PageMethod()
        {
            InitializeComponent();
        }

        #endregion

        #region Properties
        public Models.Method Method { get; set; }
        #endregion

        #region Page events

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is ParametersNavigating parametersNav)
            {
                if (parametersNav[0] is int id)
                {
                    Method = new EF.Context<Models.Method>().FindByID(id) ?? new Models.Method();
                }
                else if (parametersNav[0] is Models.Method method)
                {
                    Method = new EF.Context<Models.Method>().FindByID(method.ID) ?? new Models.Method();
                    if (parametersNav[1] is Models.Group newGroup)
                    {
                        Method.Group = newGroup.Name;
                    }
                    if (parametersNav[1] is Models.Types newType)
                    {
                        Method.Type = newType.Name;
                    }
                }
            }

            if (Method == null)
                Method = new Models.Method();
        }

        private void PageMethodPage_Loaded(object sender, RoutedEventArgs e)
        {
            ApplicationView.GetForCurrentView().Title = "Метод" + Method?.MethodInvokationString;
        }

        private void PageMethodPage_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Escape)
            {
                TryBack();
            }
        }

        #endregion

        #region Button

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            SaveObject();
        }

        private void ButtonSaveAndClose_Click(object sender, RoutedEventArgs e)
        {
            SaveObject();

            TryBack();
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            TryBack();
        }

        private async void ButtonGetInfoInClipboard_Click(object sender, RoutedEventArgs e)
        {
            TextParser textParser = new TextParser();
            await textParser.GetTextInClipboard();
            if (textParser.MethodInClipboard != null)
            {
                Method.Fill(textParser.MethodInClipboard);
            }
        }

        #endregion

        #region TextBoxGroup

        private void TextBoxGroup_ClickNew()
        {
            SaveObject();

            Navigating.Navigate(typeof(PageGroup), Method);
        }

        private void TextBoxGroup_ClickSelect()
        {
            SaveObject();
        
            Navigating.Navigate(typeof(PageList), Method, new EF.Context<Models.Group>().GetList());
        }

        private void TextBoxGroup_ClickOpen()
        {
            Navigating.Navigate(typeof(PageGroup), Method.Group, Method);
        }

        #endregion

        #region TextBoxType

        private void TextBoxType_TextChanged()
        {
            Method.SetMethodInvokationString();
        }

        private void TextBoxType_ClickNew()
        {
            SaveObject();

            Navigating.Navigate(typeof(PageType), Method);
        }

        private void TextBoxType_ClickSelect()
        {
            SaveObject();

            Navigating.Navigate(typeof(PageList), Method, new EF.Context<Models.Types>().GetList());
        }

        private void TextBoxType_ClickOpen()
        {
            Navigating.Navigate(typeof(PageType), Method.Type,  Method);
        }

        #endregion

        private void TryBack()
        {
            Navigating.Navigate(typeof(MainPage), Method);
        }

        private void SaveObject()
        {
            new EF.Context<Models.Method>().Update(Method);
        }

    }
}
