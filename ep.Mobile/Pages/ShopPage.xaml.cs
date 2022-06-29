using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
namespace ep.Mobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShopPage : ContentPage
    {
        private const string _baseUrl = "https://maps.googleapis.com/maps/api/place/autocomplete/json?";
        private readonly HttpClient _client;
        ObservableCollection<string> data = new ObservableCollection<string>();

        public ShopPage()
        {
            InitializeComponent();
            _client = new HttpClient();
            //ListOfStore();
        }    

        private void GetAddress(string text)
        {
            try
            {
                var endPoint = $"{_baseUrl}input={text}&types=establishment&components=country:au&key=AIzaSyBgYSuNmeLsZQnYv4d1cOlDpuURxRVndNE";
                //https://maps.googleapis.com/maps/api/place/autocomplete/json?
                //input=24%20john%20road%20&components=country:au&key=AIzaSyBgYSuNmeLsZQnYv4d1cOlDpuURxRVndNE
                _client.MaxResponseContentBufferSize = 256000;
                
                var response = _client.GetStringAsync(endPoint).Result;
                //https://maps.googleapis.com/maps/api/place/autocomplete/json?
                //  ? input = amoeba
                //  & location = 37.76999 % 2C - 122.44696
                //  & radius = 500
                //  & types = establishment
                //  & key = YOUR_API_KEY
                //$"&components=country:TN");
                //componentRestrictions: { country: "us" },
                var items = JsonConvert.DeserializeObject<Root>(response);
                if (!items.predictions.Any())
                { 
                    foreach (var item in items.predictions)
                    {
                        data.Add(item.description);
                    }
                }
                //return items;
            }
            catch (Exception ex)
            {
                //await DisplayAlert("Error", ex.Message, "OK");
                throw;
            }
        }

        //private void AddressEntry_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    if (e.NewTextValue.Length <= 5)
        //    {
        //        return;
        //    }
        //    countryListView.IsVisible = true;
        //    //if (!data.Any())
        //    //{ 
        //        GetAddress(e.NewTextValue.ToLower().Replace(" ", "%20"));
        //    //}
        //    countryListView.BeginRefresh();
        //    try
        //    {
        //        //var dataEmpty = data.predictions.Where(i => i.description.ToLower().Contains(e.NewTextValue.ToLower()));

        //        if (string.IsNullOrWhiteSpace(e.NewTextValue))
        //            countryListView.IsVisible = false;
        //        //else if (dataEmpty.Max().Length == 0)
        //        //    countryListView.IsVisible = false;
        //        else
        //            countryListView.ItemsSource = data.Where(i => i.ToLower().Contains(e.NewTextValue.ToLower()));
        //    }
        //    catch (Exception ex)
        //    {
        //        countryListView.IsVisible = false;

        //    }
        //    countryListView.EndRefresh();
        //}

        //private void ListView_OnItemTapped(Object sender, ItemTappedEventArgs e)
        //{
        //    //EmployeeListView.IsVisible = false;  

        //    String listsd = e.Item as string;
        //    address.Text = listsd;
        //    countryListView.IsVisible = false;

        //    ((ListView)sender).SelectedItem = null;
        //}
    }

    public class MatchedSubstring
    {
        public int length { get; set; }
        public int offset { get; set; }
    }

    public class MainTextMatchedSubstring
    {
        public int length { get; set; }
        public int offset { get; set; }
    }

    public class StructuredFormatting
    {
        public string main_text { get; set; }
        public List<MainTextMatchedSubstring> main_text_matched_substrings { get; set; }
        public string secondary_text { get; set; }
    }

    public class Term
    {
        public int offset { get; set; }
        public string value { get; set; }
    }

    public class Prediction
    {
        public string description { get; set; }
        public List<MatchedSubstring> matched_substrings { get; set; }
        public string place_id { get; set; }
        public string reference { get; set; }
        public StructuredFormatting structured_formatting { get; set; }
        public List<Term> terms { get; set; }
        public List<string> types { get; set; }
    }

    public class Root
    {
        public List<Prediction> predictions { get; set; }
        public string status { get; set; }
    }

}
