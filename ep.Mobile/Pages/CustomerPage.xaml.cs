using ep.Mobile.PageModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ep.Mobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomerPage : ContentPage
    {
        public CustomerPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await (BindingContext as CustomerPageModel).InitializeAsync(string.Empty);
        }
    }
}