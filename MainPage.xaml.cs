﻿using System;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SuckAssRSSReader
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            HomePageContent.NavigateToWebView += NavigateToWebView;
            NavigateToHome();
        }

        private void NavigateToWebView(object sender, object e)
        {
            frame.Navigate(typeof(WebViewPageContent), e);
            backButton.IsEnabled = true;
        }

        internal void NavigateToHome()
        {
            frame.Navigate(typeof(HomePageContent));
            backButton.IsEnabled = false;
        }
    }
}
