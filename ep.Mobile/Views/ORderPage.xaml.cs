using ep.Mobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ep.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]  
    public partial class OrderPage : ContentPage
    { 
        public OrderPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await (BindingContext as OrderPageModel).InitializeAsync(string.Empty);
        }
    }
}