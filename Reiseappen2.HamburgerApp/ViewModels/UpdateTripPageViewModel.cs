using Newtonsoft.Json;
using Reiseappen2.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Template10.Common;
using Template10.Mvvm;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

namespace Reiseappen2.HamburgerApp.ViewModels
{
    public class UpdateTripPageViewModel : ViewModelBase
    {

        public UpdateTripPageViewModel()
        {
            GetTrips();
        }

        public string TripName { get; set; }
        public int TripId { get; set; }
        public ObservableCollection<Trip> Trips { get; set; } = new ObservableCollection<Trip>();
        public string SelectedTrip { get; set; }

        /// <summary>
        /// Gets the trips and fills combobox.
        /// </summary>
        private async void GetTrips()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(@"http://localhost:10051/api/");
                var json = await client.GetStringAsync("trips");


                Trip[] trips = JsonConvert.DeserializeObject<Trip[]>(json);


                Trips.Clear();
                foreach (var trip in trips)
                {
                    Trips.Add(trip);
                }
            }

        }

        /// <summary>
        /// Handles the Selected event of the Item control. Gets the double tapped item and sends it and its values to another page.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="args">The <see cref="DoubleTappedRoutedEventArgs"/> instance containing the event data.</param>
        public void Item_Selected(Object sender, DoubleTappedRoutedEventArgs args)
        {
            var Selected = ((ListBox)sender).SelectedItem;

            var nav = WindowWrapper.Current().NavigationServices.FirstOrDefault();
            nav.Navigate(typeof(Views.UpdateOrDeleteTripPage), Selected);
        }

    }           

}
