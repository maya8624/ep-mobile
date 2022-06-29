using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ep.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AutoComplete : ContentPage
    {
        ObservableCollection<string> data = new ObservableCollection<string>();

        public AutoComplete()
        {
            InitializeComponent();
            ListOfStore();
        }

        public async void ListOfStore() //List of Countries  
        {
            try
            {
                data.Add("Afghanistan");
                data.Add("Austria");
                data.Add("Australia");
                data.Add("Azerbaijan");
                data.Add("Bahrain");
                data.Add("Bangladesh");
                data.Add("Belgium");
                data.Add("Botswana");
                data.Add("China");
                data.Add("Colombia");
                data.Add("Denmark");
                data.Add("Kmart");
                data.Add("Pakistan");
            }
            catch (Exception ex)
            {
                await DisplayAlert("", "" + ex, "Ok");
            }
        }

        private void SearchBar_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            countryListView.IsVisible = true;
            countryListView.BeginRefresh();
            try
            {
                var dataEmpty = data.Where(i => i.ToLower().Contains(e.NewTextValue.ToLower()));

                if (string.IsNullOrWhiteSpace(e.NewTextValue))
                    countryListView.IsVisible = false;
                else if (dataEmpty.Max().Length == 0)
                    countryListView.IsVisible = false;
                else
                    countryListView.ItemsSource = data.Where(i => i.ToLower().Contains(e.NewTextValue.ToLower()));
            }
            catch (Exception ex)
            {
                countryListView.IsVisible = false;

            }
            countryListView.EndRefresh();
        }

        private void ListView_OnItemTapped(Object sender, ItemTappedEventArgs e)
        {
            //EmployeeListView.IsVisible = false;  

            String listsd = e.Item as string;
            searchBar.Text = listsd;
            countryListView.IsVisible = false;

            ((ListView)sender).SelectedItem = null;
        }
    }
}