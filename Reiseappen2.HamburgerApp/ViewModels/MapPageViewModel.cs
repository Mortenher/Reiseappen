using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Reiseappen2.HamburgerApp.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;

namespace Reiseappen2.HamburgerApp.ViewModels
{
    public class MapPageViewModel
    {
        public MapPageViewModel()
        {
            GetNameOfTrips();
        }
        public string BingMapKey = "TsQl5bdTnOVdzuMI4xFA~l3tQaMD2MukJBN84USJ3gA~AkDkQArT2bXsc0Jt6rFBQQLIgzaz8Qz3pTx5GxphkTZBxA6RzuDJ89MVOJm6mpb9";

        public ObservableCollection<string> Trips { get; set; } = new ObservableCollection<string>();
        public string SelectedTrip { get; set; }

        public MapPage mapPage { get; set; }

       
        public List<string> Locations = new List<string>();

        /// <summary>
        /// Gets the name of trips.
        /// </summary>
        private async void GetNameOfTrips()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(@"http://localhost:10051/api/");
                var json = await client.GetStringAsync("trips/name");


                string[] trips = JsonConvert.DeserializeObject<string[]>(json);

                Trips.Clear();
                foreach (var trip in trips)
                    Trips.Add(trip);
            }

        }

        /// <summary>
        /// Handles the MapIcon event of the Insert control.
        /// This method calls the code-behind because manipulating the map from the ViewModel is troublesome.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        public async void Insert_MapIcon(object sender, RoutedEventArgs e)
        {
            if (SelectedTrip == null)
            {
                var dialog = new MessageDialog("You must choose a trip from the dropdown");
                dialog.Commands.Add(new UICommand("Ok") { Id = 0 });

                dialog.CancelCommandIndex = 0;

                var result = await dialog.ShowAsync();

                return;
            }
            await GetCitiesOfTrip();
            mapPage.Insert_MapIcon_CodeBehind(Locations);

        }
        /// <summary>
        /// Gets the cities of trip.
        /// </summary>
        /// <returns></returns>
        public async Task GetCitiesOfTrip()
        {
            
            var Selected = SelectedTrip;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(@"http://localhost:10051/api/");
                var json = await client.GetStringAsync("dayoftrips/city/" + Selected);


                string[] cities = JsonConvert.DeserializeObject<string[]>(json);

                Locations.Clear();
                foreach (var city in cities)
                    Locations.Add(city);

            }
        }

        
    }
}
