using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ep.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditEntryOutlined : ContentView
    {
        public EditEntryOutlined()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty TextProperty =
           BindableProperty.Create(nameof(Text), typeof(string), typeof(EditEntryOutlined), null);

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly BindableProperty PlaceholderProperty =
            BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(EditEntryOutlined), null);

        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderProperty); }
            set { SetValue(PlaceholderProperty, value); }
        }

        public static readonly BindableProperty PlaceholderColorProperty =
            BindableProperty.Create(nameof(PlaceholderColor), typeof(Color), typeof(EditEntryOutlined), Color.Blue);

        public Color PlaceholderColor
        {
            get { return (Color)GetValue(PlaceholderColorProperty); }
            set { SetValue(PlaceholderColorProperty, value); }
        }

        public static readonly BindableProperty BorderColorProperty =
            BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(EditEntryOutlined), Color.Blue);

        public Color BorderColor
        {
            get { return (Color)GetValue(BorderColorProperty); }
            set { SetValue(BorderColorProperty, value); }
        }

        public static readonly BindableProperty IsPasswordProperty =
            BindableProperty.Create(nameof(IsPassword), typeof(bool), typeof(EditEntryOutlined), false);

        public bool IsPassword
        {
            get { return (bool)GetValue(IsPasswordProperty); }
            set { SetValue(IsPasswordProperty, value); }
        }

        private async void TextBox_Focused(object sender, FocusEventArgs e)
        {
            await TranslateLabelToTitle();
        }

        private async void TextBox_Unfocused(object sender, FocusEventArgs e)
        {
            await TranslateLabelToPlaceHolder();
        }

        private async Task TranslateLabelToTitle()
        {
            var placeHolder = this.PlaceHolderLabel;
            var distance = GetPlaceholderDistance(placeHolder);
            await placeHolder.TranslateTo(0, -distance);
        }

        private async Task TranslateLabelToPlaceHolder()
        {
            if (string.IsNullOrEmpty(this.Text))
            {
                await this.PlaceHolderLabel.TranslateTo(0, 0);
            }
        }

        private double GetPlaceholderDistance(Label control)
        {
            var distance = 0d;
            if (Device.RuntimePlatform == Device.iOS) distance = 0;
            else distance = 5;

            distance = control.Height + distance;
            return distance;
        }
    }
}