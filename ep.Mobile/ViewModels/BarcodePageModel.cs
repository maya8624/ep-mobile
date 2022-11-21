using MvvmHelpers.Commands;
using System;
using System.Threading.Tasks;
using ep.Mobile.ViewModels.Base;
using Xamarin.Forms;
using ep.Mobile.Interfaces.IServices;
using Xamarin.Essentials;
using ep.Mobile.Reference;

namespace ep.Mobile.ViewModels
{
    public class BarcodePageModel : ViewModelBase
    {
        private readonly IShopService _service;
        private readonly IPageService _pageService;

        public AsyncCommand<string> ClickCommand { get; private set; }
        private string _url = Constant.CustomerRegUrl;

        public string Url
        {
            get => _url;
            private set => SetProperty(ref _url, value);
        }

        public bool _isVisible;
        public bool IsVisible
        {
            get => _isVisible;
            private set => SetProperty(ref _isVisible, value);
        }

        public BarcodePageModel()
        {
            ClickCommand = new AsyncCommand<string>(CreateUrlAsync);
            _pageService = DependencyService.Get<IPageService>();
            _service = DependencyService.Get<IShopService>();
        }

        private async Task CreateUrlAsync(string orderNo)
        {

            try
            {
                if (string.IsNullOrWhiteSpace(orderNo))
                {
                    await _pageService.DisplayAlert("Validation", "Order No is required", "Ok");
                    return;
                }

                var shop = await _service.GetShopAsync();
                if (shop == null)
                {
                    await _pageService.DisplayAlert("Validation", "There is no shop details", "Ok");
                    return;
                }

                var symKey = await SecureStorage.GetAsync("symKey");
                if (string.IsNullOrEmpty(symKey))
                {
                    //await SecureStorage.SetAsync("symKey", "0LRB6tEqL6XpOq2MkZK/QealKohwIgMiIqHReYcoJHA=");
                    await _pageService.DisplayAlert("Error", "No Key", "Cancel");
                    return;
                }

                var (eText, iv) = _service.Encrytp(symKey, orderNo);
                Url = $"{_url}?qi={shop.Id}&qo{eText}&qv={iv}";
                IsVisible = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await _pageService.DisplayAlert("Error", ex.Message, "Cancel");
            }
        }

        private async Task Close()
        {
        }
    }
}
