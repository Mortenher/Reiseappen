using Newtonsoft.Json;
using Reiseappen2.HamburgerApp.Views;
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
using Windows.UI.Popups;

namespace Reiseappen2.HamburgerApp.ViewModels
{
    
    public class AddTripPageViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddTripPageViewModel"/> class.
        /// Calls GetNameOfTrips to fill list.
        /// </summary>
        public AddTripPageViewModel()
        {
            GetNameOfTrips();
        }
        public ObservableCollection<string> Trips { get; set; } = new ObservableCollection<string>();
        public string SelectedTrip { get; set; }
        public int TripId { get; set; }
        public int Day { get; set; }
        public string City { get; set; }
        public string Hotel { get; set; }
        public string Dinner { get; set; }
        public int Money { get; set; }
        public string Info { get; set; }
        public string ErrorText { get; set; }

        /// <summary>
        /// Gets the name of trips and fills combobox with found items.
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
        /// Gets the identifier of the selected trip.
        /// </summary>
        /// <returns>id of selected trip</returns>
        public async Task<int> GetId()
        {
            var Selected = SelectedTrip;
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(@"http://localhost:10051/api/");

                var json = await client.GetStringAsync("trips/" + Selected);
                
                Trip id = JsonConvert.DeserializeObject<Trip>(json);

                return id.TripId;
                
            }
        }

        /// <summary>
        /// Adds a day of trip to the database when button clicked.
        /// </summary>
        public async void AddDay_Click()
        {
            if(SelectedTrip == null)
            {
                var dialog = new MessageDialog("You must choose a trip from the dropdown");
                dialog.Commands.Add(new UICommand("Ok") { Id = 0 });

                dialog.CancelCommandIndex = 0;

                var result = await dialog.ShowAsync();

                return;
            }
            TripId = await GetId();
            if(Day == 0 || City == null || Hotel == null || Dinner ==  null || Info == null || City == "" || Hotel == "" || Dinner == "" || Info == "")
            {
                var dialog = new MessageDialog("You must fill all fields");
                dialog.Commands.Add(new UICommand("Ok") { Id = 0 });

                dialog.CancelCommandIndex = 0;

                var result = await dialog.ShowAsync();
                
                return;
            }
            var day = new DayOfTrip
            {
                Day = Day,
                City = City,
                Hotel = Hotel,
                Dinner = Dinner,
                MoneySpent = Money,
                AdditionalInfo = Info,
                TripId = TripId

            };
            using (var client = new System.Net.Http.HttpClient())
            {

                client.BaseAddress = new Uri(@"http://localhost:10051/api/");

                var content = JsonConvert.SerializeObject(day);

                var httpContent = new StringContent(content, Encoding.UTF8, "application/json");

                var json = await client.PostAsync("dayoftrips", httpContent);

                var nav = WindowWrapper.Current().NavigationServices.FirstOrDefault();
                nav.Navigate(typeof(Views.GetTripPage));
            }
        }
    }
}
