using Template10.Mvvm;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
using Template10.Services.NavigationService;
using Windows.UI.Xaml.Navigation;
using Reiseappen2.Model;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using Newtonsoft.Json.Linq;
using Reiseappen2.HamburgerApp.Views;
using Windows.UI.Popups;
using Windows.Devices.Geolocation;

namespace Reiseappen2.HamburgerApp.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public MainPageViewModel()
        {
            
        }
        
        public double MoneySpent { get; set; }
        public string MostEatenDinner { get; set; }
        public double AverageTripLength { get; set; }

        public string BingMapKey = "TsQl5bdTnOVdzuMI4xFA~l3tQaMD2MukJBN84USJ3gA~AkDkQArT2bXsc0Jt6rFBQQLIgzaz8Qz3pTx5GxphkTZBxA6RzuDJ89MVOJm6mpb9";

        public List<string> Locations = new List<string>();

        public double Dist1 { get; set; }
        private double distance;
        public double Distance {
            get
            {
                return distance;
            }
            set
            {
                distance = value;
                RaisePropertyChanged(nameof(Distance));
            }
        }
        public ObservableCollection<string> Trips { get; set; } = new ObservableCollection<string>();
        public string SelectedTrip { get; set; }
        public List<int> IdList { get; set; }

        /// <summary>
        /// Gets various statistics from trips and fills the xaml.
        /// </summary>
        /// <returns></returns>
        public async Task GetStatistics()
        {
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(@"http://localhost:10051/api/");

                var json = await client.GetStringAsync("dayoftrips");

                DayOfTrip[] days = JsonConvert.DeserializeObject<DayOfTrip[]>(json);

                //Lambda expressions to find statistics
                MoneySpent =  Math.Round(days.Average(d => d.MoneySpent), 2);

                //Linq expression to find number of days
                var dayCount = (from daycount in days select daycount.Day).Count();
                AverageTripLength = dayCount / IdList.Count();


                //Anonymous type with linq expression to assert most dinners eaten
                var dinner = from dinners in days
                             select new { dinners.Dinner };
                foreach(var d in dinner)
                {
                    if (d.Equals(d))
                    {
                        MostEatenDinner = d.Dinner.ToString();
                    }
                }


            }
        }

        /// <summary>
        /// Fills the identifier list for use in GetStatistics.
        /// </summary>
        /// <returns></returns>
        public async Task FillIdList()
        {
            IdList = new List<int>();
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(@"http://localhost:10051/api/");
                
                var json = await client.GetStringAsync("trips");

                Trip[] trips = JsonConvert.DeserializeObject<Trip[]>(json);

                var identifiers = trips.Select(t => t.TripId).ToList();

                foreach(var id in identifiers)
                    IdList.Add(id);
              
            }
            
        }

        /// <summary>
        /// Gets the name of trips.
        /// </summary>
        /// <returns></returns>
        private async Task GetNameOfTrips()
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

        public async void CalculateDistanceTraveled_Click()
        {
            await CalculateDistanceTraveled();
        }

        /// <summary>
        /// Calculates the distance traveled.
        /// Gets all cities from a trip, calls bing rest api to get coordinates and fills list with these coords.
        /// Calls method with coordinate-parameters and gets distance back
        /// </summary>
        /// <returns></returns>
        public async Task CalculateDistanceTraveled()
        {
            if (SelectedTrip == null)
            {
                var dialog = new MessageDialog("You must choose a trip from the dropdown");
                dialog.Commands.Add(new UICommand("Ok") { Id = 0 });

                dialog.CancelCommandIndex = 0;

                var result = await dialog.ShowAsync();

                return;
            }
            var Selected = SelectedTrip;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(@"http://localhost:10051/api/");
                var json = await client.GetStringAsync("dayoftrips/city/" + Selected);


                string[] cities = JsonConvert.DeserializeObject<string[]>(json);

                Locations.Clear();
                foreach (var city in cities)
                    Locations.Add(city);

                
                List<Double> coordList = new List<double>();
                foreach (var loc in Locations)
                {
                    Uri geocodeRequest = new Uri(string.Format("http://dev.virtualearth.net/REST/v1/Locations?locality=" + loc + "&key=" + BingMapKey));
                    var json2 = await client.GetStringAsync(geocodeRequest);

                    dynamic response = JObject.Parse(json2);
                    JArray latlng = (JArray)response["resourceSets"][0]["resources"][0]["point"]["coordinates"];
                    coordList.Add((double)latlng[0]);
                    coordList.Add((double)latlng[1]);
                   
                 
                }
                //Number of coordinates should always be an even number, so this for loop should work for all trips.
                for (int i = 0; i <= coordList.Count(); i++)
                {
                    if (i == 2)
                    {
                        Dist1 = GetDistance(coordList[0], coordList[1], coordList[2], coordList[3]);
                    }
                        
                    else if(i > 2 && i % 2 == 1)
                    {
                        Dist1 += GetDistance(coordList[i - 1], coordList[i], coordList[i+1], coordList[i + 2]);
                    }
                    else if(i == coordList.Count() - 2)
                    {
                        Distance = Math.Round(Dist1 / 1000);
                        break;
                    }
                       
                }
                
            }

         }

        /// <summary>
        /// Gets the distance, uses the Haversines way to calculate distance.
        /// Found on http://stackoverflow.com/questions/10175724/calculate-distance-between-two-points-in-bing-maps
        /// </summary>
        /// <param name="lat1">The lat1.</param>
        /// <param name="long1">The long1.</param>
        /// <param name="lat2">The lat2.</param>
        /// <param name="long2">The long2.</param>
        /// <returns></returns>
        public double GetDistance(double lat1, double long1, double lat2, double long2)
        {
            
                double distance = 0;

                double dLat = (lat2 - lat1) / 180 * Math.PI;
                double dLong = (long2 - long1) / 180 * Math.PI;

                double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2)
                            + Math.Cos(lat1 / 180 * Math.PI) * Math.Cos(lat2 / 180 * Math.PI)
                            * Math.Sin(dLong / 2) * Math.Sin(dLong / 2);
                double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

                //Calculate radius of earth
                // For this you can assume any of the two points.
                double radiusE = 6378135; // Equatorial radius, in metres
                double radiusP = 6356750; // Polar Radius

                //Numerator part of function
                double nr = Math.Pow(radiusE * radiusP * Math.Cos(lat1 / 180 * Math.PI), 2);
                //Denominator part of the function
                double dr = Math.Pow(radiusE * Math.Cos(lat1 / 180 * Math.PI), 2)
                                + Math.Pow(radiusP * Math.Sin(lat1 / 180 * Math.PI), 2);
                double radius = Math.Sqrt(nr / dr);

                //Calculate distance in meters.
                distance = radius * c;
                return distance; // distance in meters
        }


        /// <summary>
        /// Called when [navigated to asynchronous].
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <param name="mode">The mode.</param>
        /// <param name="suspensionState">State of the suspension.</param>
        /// <returns></returns>
        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState)
         {
            Busy.SetBusy(true, "Laster inn data fra API");
            await CheckForInternetConnection();
            await FillIdList();
            await GetNameOfTrips();
            await GetStatistics();
            Busy.SetBusy(false);
            Distance = 0;
            
         }

        public async static Task<bool> CheckForInternetConnection()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    using (var stream = await client.GetAsync("http://www.google.com"))
                    {
                        return true;
                    }
                }
            }
            catch(Exception)
            {
                await ShowConnectionError();
                return false;
            }
        }

        private async static Task ShowConnectionError()
        {
            var dialog = new MessageDialog("Could not connect to server, check your internet connection");
            dialog.Commands.Add(new UICommand("Ok") { Id = 0 });

            dialog.CancelCommandIndex = 0;
            await dialog.ShowAsync();
            return;
        }

        public void GotoSettings() =>
            NavigationService.Navigate(typeof(Views.SettingsPage), 0);

        public void GotoPrivacy() =>
            NavigationService.Navigate(typeof(Views.SettingsPage), 1);

        public void GotoAbout() =>
            NavigationService.Navigate(typeof(Views.SettingsPage), 2);

    }
}

