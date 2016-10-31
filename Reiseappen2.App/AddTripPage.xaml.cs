using Newtonsoft.Json;
using Reiseappen2.DataModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Reiseappen2.App
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddTripPage : Page
    {
        public AddTripPage()
        {
            this.InitializeComponent();
        }

        private async void AddTrip_Click(object sender, RoutedEventArgs e)
        {
            var day = new DayOfTrip
            {
                Day = Convert.ToInt32(DayBox.Text),
                City = CityBox.Text,
                Hotel = HotelBox.Text,
                Dinner = DinnerBox.Text,
                MoneySpent = Convert.ToInt32(MoneyBox.Text),
                AdditionalInfo = InfoBox.Text,
                TripId = Convert.ToInt32(NameBox.Text)

            };
            using (var client = new System.Net.Http.HttpClient())
            {

                client.BaseAddress = new Uri(@"http://localhost:10051/api/");

                var content = JsonConvert.SerializeObject(day);

                var httpContent = new StringContent(content, Encoding.UTF8, "application/json");

                var json = await client.PostAsync("dayoftrips", httpContent);

                this.Frame.Navigate(typeof(MainPage));
            }
        }
    }
}
