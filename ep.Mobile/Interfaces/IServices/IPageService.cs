using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ep.Mobile.Interfaces.IServices
{
    public interface IPageService
    {
        Task DisplayAlert(string title, string message, string cancel);
        Task<bool> DisplayAlert(string title, string message, string ok, string cancel);
        Task<string> DisplayPromptAsync(string title, string message, string ok, string cancel, string placeholder);
    }
}
