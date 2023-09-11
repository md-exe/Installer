using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Path = System.IO.Path;
using System.Drawing;
using System.Net;
using System.Net.Http;

namespace NoblegardenInstaller
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // Имитация окна Windows
        public void MinButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = (WindowState)FormWindowState.Minimized;
        }

        public void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            if (ProgressBar.Visibility == Visibility.Visible && ProgressBar.Value != 100)
            {
                var result = System.Windows.MessageBox.Show("Вы действительно хотите закрыть программу? Установка будет прервана.", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    this.Close();
                }
                else
                {
                    return;
                }
            }
            else
            {
                this.Close();
            }
        }

        void DragWindow_LeftClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                base.OnMouseLeftButtonDown(e);
                DragMove();
            }
        }

        public void MinButton_OnHover(object sender, System.Windows.Input.MouseEventArgs e)
        {
            MinSign.Visibility = Visibility.Visible;
        }

        public void CloseButton_OnHover(object sender, System.Windows.Input.MouseEventArgs e)
        {
            CloseSign.Visibility = Visibility.Visible;
        }

        private void MinButton_OnLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            MinSign.Visibility = Visibility.Hidden;
        }

        private void CloseButton_OnLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            CloseSign.Visibility = Visibility.Hidden;
        }

        // Скачать
        private async void StartDownload_Click(object sender, RoutedEventArgs e)
        {
            var result = System.Windows.MessageBox.Show("Программа должна находиться в пустой папке. Туда же будет установлена игра. Продолжить?", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                StartDownload.Visibility = Visibility.Hidden;
                ProgressBar.Visibility = Visibility.Visible;
                await Task.Delay(1000);
                Percents.Visibility = Visibility.Visible;

                using (WebClient wc = new WebClient())
                {
                    wc.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();
                    wc.DownloadProgressChanged += (s, g) => { ProgressBar.Value = g.ProgressPercentage; };
                    wc.DownloadFileAsync(new Uri("http://95.216.176.164/patch-ruRU-A.MPQ"), "testfile");
                    wc.DownloadFileCompleted += (s, g) =>
                    {
                        if (g.Error == null)
                        {
                            stopwatch.Stop();
                            var done = System.Windows.MessageBox.Show($"Удачной игры!\nВремя загрузки: {stopwatch.Elapsed:mm\\:ss}", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
                            if (done == MessageBoxResult.OK)
                            {
                                this.Close();
                            }
                            else
                            {
                                return;
                            }
                        }
                        else
                        {
                            return;
                        }
                    };
                }
            }
            else
            {
                return;
            }
        }
        //    {
        //        var result = System.Windows.MessageBox.Show("Программа должна находиться в пустой папке. Туда же будет установлена игра. Продолжить?", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Warning);
        //        if (result == MessageBoxResult.Yes)
        //        {
        //            // Declare a Stopwatch object and start it
        //            var stopwatch = new Stopwatch();
        //            stopwatch.Start();

        //            StartDownload.Visibility = Visibility.Hidden;
        //            ProgressBar.Visibility = Visibility.Visible;
        //            await Task.Delay(1000);
        //            Percents.Visibility = Visibility.Visible;

        //            using (var client = new HttpClient())
        //            {
        //                var response = await client.GetAsync("http://95.216.176.164/patch-ruRU-Z.MPQ", HttpCompletionOption.ResponseHeadersRead);

        //                response.EnsureSuccessStatusCode();

        //                var totalBytes = response.Content.Headers.ContentLength ?? -1L;
        //                var canReportProgress = totalBytes != -1 && totalBytes != 0;

        //                using (var stream = await response.Content.ReadAsStreamAsync())
        //                {
        //                    using (var fileStream = new FileStream("testfile", FileMode.Create))
        //                    {
        //                        var buffer = new byte[8192];
        //                        var bytesRead = 0;
        //                        var bytesWritten = 0;
        //                        do
        //                        {
        //                            bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
        //                            await fileStream.WriteAsync(buffer, 0, bytesRead);
        //                            bytesWritten += bytesRead;

        //                            if (canReportProgress)
        //                            {
        //                                var progressPercentage = (int)Math.Round((double)bytesWritten / totalBytes * 100);
        //                                ProgressBar.Value = progressPercentage;
        //                            }
        //                        } while (bytesRead > 0);
        //                    }
        //                }

        //                // Stop the stopwatch and display the elapsed time in the message box
        //                stopwatch.Stop();
        //                var done = System.Windows.MessageBox.Show($"Удачной игры!\n Время загрузки: {stopwatch.Elapsed:mm\\:ss}", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
        //                if (done == MessageBoxResult.OK)
        //                {
        //                    this.Close();
        //                }
        //            }
        //        }
        //        else
        //        {
        //            return;
        //        }
        //    }
    }
}
