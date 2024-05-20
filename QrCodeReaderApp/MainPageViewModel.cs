using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ZXing.Net.Maui;

namespace QrCodeReaderApp
{
    internal class MainPageViewModel : INotifyPropertyChanged
    {
        public ICommand ScanResultsCommand { get; }
        public ICommand CopyItemCommand { get; }
        public ICommand OpenWebPageItemCommand { get; }
        public ICommand DeleteItemCommand { get; }

        public ObservableHashSet<string> Results { get; } = [];

        public BarcodeReaderOptions BarcodeReaderOptions { get; } = new BarcodeReaderOptions
        {
            Formats = BarcodeFormats.All,
            AutoRotate = true,
            Multiple = true,
            TryHarder = true
        };

        public event PropertyChangedEventHandler? PropertyChanged;

        public MainPageViewModel()
        {
            ScanResultsCommand = new Command<BarcodeResult[]>((r) => this.ReadResults(r));
            CopyItemCommand = new Command<string>(async (r) => await this.Copy(r));
            OpenWebPageItemCommand = new Command<string>(async (r) => await this.Open(r));
            DeleteItemCommand = new Command<string>((r => this.Remove(r)));
        }

        private async Task Copy(string r)
        {
            try
            {
                await Clipboard.Default.SetTextAsync(r);
            }
            catch
            {
                await ShowToast("An error occurred");
            }
        }

        private async Task Open(string r)
        {
            try
            {
                if (Uri.TryCreate(r, UriKind.Absolute, out var uri))
                {
                    await Browser.Default.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
                }
            }
            catch
            {
                await ShowToast("An error occurred");
            }
        }

        private void Remove(string r)
        {
            this.Results.Remove(r);
        }

        private void ReadResults(BarcodeResult[] barcodeResults)
        {
            if (barcodeResults is null || barcodeResults.Length == 0)
            {
                return;
            }

            foreach (var result in barcodeResults)
            {
                if (!string.IsNullOrWhiteSpace(result?.Value))
                {
                    this.Results.Insert(0, result.Value);
                }
            }
        }

        public void OnPropertyChanged([CallerMemberName] string name = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private async Task ShowToast(string message)
        {
            var now = DateTime.Now;

            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                var toast = Toast.Make(message, ToastDuration.Short, 14);
                await toast.Show();
            });
        }
    }
}
