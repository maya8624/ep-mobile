using System.ComponentModel;
using ep.Mobile.ViewModels;
using Xamarin.Forms;

namespace ep.Mobile.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}