using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ep.Mobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QRPopupPage// : Popup
    {
        public QRPopupPage()
        {
            InitializeComponent();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            Dismiss("Data from popup");
            //TODO: update OrderNo column in Message table
        }
    }
}