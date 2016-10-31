using Newtonsoft.Json;
using Reiseappen2.DataModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Reiseappen2.App
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public ObservableCollection<DayOfTrip> Days { get; set; } = new ObservableCollection<DayOfTrip>();
        public MainPage()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }

        private async void GetTrips_Click(object sender, RoutedEventArgs e)
        {
            using (var client = new System.Net.Http.HttpClient())
            {
                client.BaseAddress = new Uri(@"http://localhost:10051/api/");

                var json = await client.GetStringAsync("dayoftrips");

                DayOfTrip[] days = JsonConvert.DeserializeObject<DayOfTrip[]>(json);

                Days.Clear();
                foreach (var day in days)
                    Days.Add(day);

            }
        }

        private void GoToTripAdder_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AddTripPage));
        }

        
        private async void NewTrip_Click(object sender, RoutedEventArgs e)
        {
            var trip = new Trip
            {
                Name = NewTripBox.Text
            };
            using (var client = new System.Net.Http.HttpClient())
            {
                client.BaseAddress = new Uri(@"http://localhost:10051/api/");


                var content = JsonConvert.SerializeObject(trip);

                var httpContent = new StringContent(content, Encoding.UTF8, "application/json");

                var json = await client.PostAsync("trips", httpContent);
               

              


            }
        }

        private async void DeleteTrip_Click(object sender, RoutedEventArgs e)
        {
            
            var name = Convert.ToInt32(DeleteTripBox.Text);

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(@"http://localhost:10051/api/");

                await client.DeleteAsync("trips/" + name);
            }
        }
    }
}
