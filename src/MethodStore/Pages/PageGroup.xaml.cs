using System;
using System.Collections.Generic;
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

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace MethodStore
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class PageGroup : Page
    {
        public Models.Group Group { get; set; }
        private Models.Method _parentMethods;

        public PageGroup()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is ParametersNavigating parametersNav)
            {
                if (parametersNav.CountParameters > 0)
                {
                    object firstParam = parametersNav.Parameters[0];

                    if (firstParam is int id)
                    {
                        Group = new EF.Context<Models.Group>().FindByID(id);
                    }
                    else if (firstParam is Models.Method method)
                    {
                        _parentMethods = method;
                    }
                }
            }

            if (Group == null)
                Group = new Models.Group();
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            TryBack(_parentMethods);
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            new EF.Context<Models.Group>().UpdateMethods(Group);
             
            TryBack(_parentMethods, Group);
        }

        private void TryBack(params object[] param)
        {
            Navigating.Navigate(typeof(PageMethod), param);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            TextBoxName.Focus(FocusState.Programmatic);
        }
    }
}
