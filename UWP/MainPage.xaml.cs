 using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
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

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace UWP
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private const string PageNamespace = nameof(UWP);

        // List of ValueTuple holding the Navigation Tag and the relative Navigation Page 
        private readonly List<(string Tag, Type Page)> _pages = new List<(string Tag, Type Page)>
        {
            ("HomePage", typeof(HomePage)),
            ("MinePage", typeof(MinePage)),
            ("GetColorPage", typeof(GetColorPage)),
            ("AboutPage", typeof(AboutPage)),
        };

        public MainPage()
        {
            // Hide default title bar.
            this.InitializeComponent();
            CoreApplication.GetCurrentView().TitleBar.LayoutMetricsChanged += (s, e) => UpdateAppTitle(s);
            //var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            //coreTitleBar.ExtendViewIntoTitleBar = true;
        }

        void UpdateAppTitle(CoreApplicationViewTitleBar coreTitleBar)
        {
            var full = (ApplicationView.GetForCurrentView().IsFullScreenMode);
            var left = 12 + (full ? 0 : coreTitleBar.SystemOverlayLeftInset);
        }

        private void nvSample_Loaded(object sender, RoutedEventArgs e)
        {
            contentFrame.Navigated += On_Navigated ;
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
        
        private void nvSample_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
            if (contentFrame.CanGoBack)
                contentFrame.GoBack();
        }

        private void On_Navigated(object sender, NavigationEventArgs e)
        {
            nvSample.IsBackEnabled = contentFrame.CanGoBack;

            if (contentFrame.SourcePageType == typeof(SettingPage))
            {
                // SettingsItem is not part of NavView.MenuItems, and doesn't have a Tag.
                nvSample.SelectedItem = (NavigationViewItem)nvSample.SettingsItem;
            }
            else if (contentFrame.SourcePageType != null)
            {
                var item = _pages.FirstOrDefault(p => p.Page == e.SourcePageType);
                /*
                nvSample.SelectedItem = nvSample.MenuItems
                    .OfType<NavigationViewItem>()
                    .First(n => n.Tag.Equals(item.Tag));
                    */
            }
        }
    }
}
