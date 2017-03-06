using Newtonsoft.Json;
using Reiseappen2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Template10.Common;
using Template10.Mvvm;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

namespace Reiseappen2.HamburgerApp.ViewModels
{
    public class UpdateOrDeleteTripPageViewModel : ViewModelBase
    {
        public string Name { get; set; }
        public int TripId { get; set; }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {

            var tripToUpdate = (Trip)parameter;
            Name = tripToUpdate.Name;
            TripId = tripToUpdate.TripId;


            await Task.CompletedTask;
        }

        /// <summary>
        /// Handles the Click event of the UpdateDayOfTrip control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        public async void UpdateTrip_Click(object sender, RoutedEventArgs e)
        {
            var trip = new Trip
            {
                Name = Name,
                TripId = TripId
            };
            using (var client = new System.Net.Http.HttpClient())
            {

                client.BaseAddress = new Uri(@"http://localhost:10051/api/");

                var content = JsonConvert.SerializeObject(trip);

                var httpContent = new StringContent(content, Encoding.UTF8, "application/json");

                var json = await client.PutAsync("trips/" + trip.TripId, httpContent);

                var nav = WindowWrapper.Current().NavigationServices.FirstOrDefault();
                nav.Navigate(typeof(Views.GetTripPage));

            }
        }
        /// <summary>
        /// Handles the Click event of the DeleteDayOfTrip control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        public async void DeleteTrip_Click(object sender, RoutedEventArgs e)
        {
            var id = TripId;
            using (var client = new System.Net.Http.HttpClient())
            {

                client.BaseAddress = new Uri(@"http://localhost:10051/api/");

                await client.DeleteAsync("trips/" + id);

                var nav = WindowWrapper.Current().NavigationServices.FirstOrDefault();
                nav.Navigate(typeof(Views.GetTripPage));

            }
        }
    }
}

