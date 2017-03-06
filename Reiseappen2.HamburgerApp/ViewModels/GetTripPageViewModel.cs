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
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Reiseappen2.HamburgerApp.ViewModels
{
    public class GetTripPageViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetTripPageViewModel"/> class.
        /// </summary>
        public GetTripPageViewModel()
        {
            GetNameOfTrips();
        }

        
        public ObservableCollection<DayOfTrip> Days { get; set; } = new ObservableCollection<DayOfTrip>();
        public ObservableCollection<string> Trips { get; set; } = new ObservableCollection<string>();

        public string SelectedTrip { get; set; }
        public DayOfTrip SelectedDay { get; set; }

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
        /// Gets the information click.
        /// </summary>
        public void GetInfo_Click()
        {
            var Selected = SelectedTrip;
            GetInfo(Selected);
           
        }
        /// <summary>
        /// Gets the information of selected trip called by clickevent.
        /// </summary>
        /// <param name="s">The s.</param>
        public async void GetInfo(string s)
        {
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(@"http://localhost:10051/api/");

                var json = await client.GetStringAsync("dayoftrips/" + s);
          
                DayOfTrip[] days = JsonConvert.DeserializeObject<DayOfTrip[]>(json);

                //Lambda for å sortere listen
                IEnumerable<DayOfTrip> daylist = days.OrderBy(d => d.Day);
            
                Days.Clear();
                foreach (var day in daylist)
                    Days.Add(day);
            }
        }

        /// <summary>
        /// Handles the Selected event of the Item control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="args">The <see cref="DoubleTappedRoutedEventArgs"/> instance containing the event data.</param>
        public void Item_Selected(Object sender, DoubleTappedRoutedEventArgs args)
        {
            var Selected = ((ListBox)sender).SelectedItem;

            var nav = WindowWrapper.Current().NavigationServices.FirstOrDefault();
            nav.Navigate(typeof(Views.UpdatePage), Selected);
        }
        


    }
}
