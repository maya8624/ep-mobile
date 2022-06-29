using System.Threading.Tasks;
using ep.Mobile.Interfaces.IServices;
using Xamarin.Forms;

namespace ep.Mobile.Services
{
    public class PageService : IPageService
    {
        public async Task DisplayAlert(string title, string message, string cancel)
        {
            await Application.Current.MainPage.DisplayAlert(title, message, cancel);
        }

        public async Task<bool> DisplayAlert(string title, string message, string ok, string cancel)
        {
            return await Application.Current.MainPage.DisplayAlert(title, message, ok, cancel);
        }

        public async Task<string> DisplayPromptAsync(string title, string message, string ok, string cancel, string placeholder)
        {            
            return await Application.Current.MainPage.DisplayPromptAsync(title, message, ok, cancel, placeholder);
        }
    }
}
