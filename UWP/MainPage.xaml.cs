 using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace UWP
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private const string PageNamespace = nameof(UWP);

        public MainPage()
        {
            this.InitializeComponent();
            contentFrame.Navigated += contentFrame_Navigated;
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            SystemNavigationManager.GetForCurrentView().BackRequested += MainPage_BackRequested;
            contentFrame.Navigate(typeof(HomePage));
        }
        
        private void contentFrame_Navigated(object sender, NavigationEventArgs e)
        {
            var pageName = contentFrame.Content.GetType().Name;
            //nvSample.SelectedItem = nvSample.MenuItems.OfType<NavigationViewItem>().Where(item => item.Tag.ToString() == pageName).First();
        }
        private void MainPage_BackRequested(object sender, BackRequestedEventArgs e)
        {
            if (contentFrame.CanGoBack)
                contentFrame.GoBack();
        }
        private void nvSample_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if(args.IsSettingsInvoked)
            {
                contentFrame.Navigate(typeof(SettingPage));
            }
            else
            {
                var invokedMenuItem = sender.MenuItems.OfType<NavigationViewItem>().Where(item => item.Content.ToString() == args.InvokedItem.ToString()).First();
                var pageTypeName = invokedMenuItem.Tag.ToString();
                var pageType = Assembly.GetExecutingAssembly().GetType($"{PageNamespace}.{pageTypeName}");
                contentFrame.Navigate(pageType);
            }
        }

        private void nvSample_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {

        }

        private void nvSample_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (NavigationViewItemBase item in nvSample.MenuItems)
            {
                if(item is NavigationViewItem && item.Tag.ToString()=="HomePage")
                {
                    nvSample.SelectedItem = item;
                    break;
                }
            }
            contentFrame.Navigate(typeof(HomePage));
        }
    }
}
