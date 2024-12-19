using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Web.WebView2.Core;
using System;
using System.Diagnostics;

namespace WinUI3_test1
{
    public sealed partial class MainWindow : Window
    {
        //private ResourceManager _resourceManager; 
        public MainWindow()
        {
            this.InitializeComponent();
          //  _resourceManager =  new Microsoft.Windows.ApplicationModel.Resources.ResourceManager();

            LoadLocalizedStrings();

            // Initialize WebView2
            InitializeAsync();
        }

        private void LoadLocalizedStrings()
        {
            var resourceContext = new Windows.ApplicationModel.Resources.Core.ResourceContext();
            //resourceContext.QualifierValues["Language"] = "de-DE";
            var resourceMap = Windows.ApplicationModel.Resources.Core.ResourceManager.Current.MainResourceMap;
            this.BrowserBack.Content = resourceMap.GetValue("Strings/ButtonBack", resourceContext).ValueAsString;
            this.BrowserForward.Content = resourceMap.GetValue("Strings/ButtonForward", resourceContext).ValueAsString;
            this.BrowserReload.Content = resourceMap.GetValue("Strings/ButtonReload", resourceContext).ValueAsString;
            this.BrowserGo.Content = resourceMap.GetValue("Strings/ButtonGo", resourceContext).ValueAsString;
        }


        private async void InitializeAsync()
        {
            await MyWebView.EnsureCoreWebView2Async(null);
            MyWebView.CoreWebView2.Navigate("https://www.tommti-systems.de"); // Default URL

            // Capture new window requests and handle them in the same WebView2 instance
            MyWebView.CoreWebView2.NewWindowRequested += CoreWebView2_NewWindowRequested;
        }

        // Back button click event
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (MyWebView.CanGoBack)
            {
                MyWebView.GoBack();
            }
        }

        // Forward button click event
        private void ForwardButton_Click(object sender, RoutedEventArgs e)
        {
            if (MyWebView.CanGoForward)
            {
                MyWebView.GoForward();
            }
        }

        // Reload button click event
        private void ReloadButton_Click(object sender, RoutedEventArgs e)
        {
            MyWebView.Reload();
        }

        // New window requested event handler
        private void CoreWebView2_NewWindowRequested(object sender, CoreWebView2NewWindowRequestedEventArgs e)
        {
            // Cancel the default behavior of opening a new window
            e.Handled = true;

            // Get the URL of the new window and open it in the current WebView2
            string newWindowUrl = e.Uri.ToString();
            MyWebView.CoreWebView2.Navigate(newWindowUrl);
        }

        // Go button click event (navigate to the URL entered)
        private void GoButton_Click(object sender, RoutedEventArgs e)
        {
            var url = UrlTextBox.Text;
            if (!string.IsNullOrWhiteSpace(url))
            {
                try
                {
                    MyWebView.CoreWebView2.Navigate(url);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Exception: " + ex.Message);
                }
            }
        }

        // URL text box change event
        private void UrlTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var url = UrlTextBox.Text;
        }
    }
}
