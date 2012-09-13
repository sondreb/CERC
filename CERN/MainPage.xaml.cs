namespace CERC
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Animation;
    using System.Windows.Shapes;
    using Microsoft.Phone.Controls;
    using System.IO.IsolatedStorage;
    using System.IO;
    using Microsoft.Phone.Info;

    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (!store.DirectoryExists("HTML"))
                {
                    store.CreateDirectory("HTML");
                }

                CopyToIsolatedStorage("HTML\\default.html", store);
            }
        }

        private static void CopyToIsolatedStorage(string file, IsolatedStorageFile store, bool overwrite = true)
        {
            if (store.FileExists(file) && !overwrite)
            {
                return;
            }

            using (Stream resourceStream = Application.GetResourceStream(new Uri(file, UriKind.Relative)).Stream)
            using (IsolatedStorageFileStream fileStream = store.OpenFile(file, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                int bytesRead;
                var buffer = new byte[resourceStream.Length];
                while ((bytesRead = resourceStream.Read(buffer, 0, buffer.Length)) > 0)
                    fileStream.Write(buffer, 0, bytesRead);
            }
        }

        private void browser_ScriptNotify(object sender, NotifyEventArgs e)
        {
            var response = new object[]
                {
                    DeviceStatus.ApplicationCurrentMemoryUsage,
                    DeviceStatus.ApplicationMemoryUsageLimit,
                    DeviceStatus.ApplicationPeakMemoryUsage,
                    DeviceStatus.DeviceTotalMemory
                };

            browser.InvokeScript("getMemoryUsageCallback", response.Select(c => c.ToString()).ToArray());
        }
    }
}