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
    /// <summary>
    /// ViewModel for UpdatePage.xaml
    /// </summary>
    /// <seealso cref="Template10.Mvvm.ViewModelBase" />
    public class UpdatePageViewModel : ViewModelBase
    {

        int DayId { get; set; }
        int TripId { get; set; }
        public int Day { get; set; }
        public string City { get; set; }
        public string Hotel { get; set; }
        public string Dinner { get; set; }
        public int Money { get; set; }
        public string Info { get; set; }
        /// <summary>
        /// Called when [navigated to asynchronous].
        /// Sets values to xaml items that currently exists, editable after.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <param name="mode">The mode.</param>
        /// <param name="state">The state.</param>
        /// <returns></returns>
        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {

            var TripDay = (DayOfTrip)parameter;
            Day = TripDay.Day;
            City = TripDay.City;
            Hotel = TripDay.Hotel;
            Dinner = TripDay.Dinner;
            Money = TripDay.MoneySpent;
            Info = TripDay.AdditionalInfo;
            DayId = TripDay.TripDayId;
            TripId = TripDay.TripId;

            await Task.CompletedTask;
        }




        /// <summary>
        /// Handles the Click event of the UpdateDayOfTrip control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        public async void UpdateDayOfTrip_Click(object sender, RoutedEventArgs e)
        {
            var day = new DayOfTrip
            {
                TripDayId = DayId,
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

                var json = await client.PutAsync("dayoftrips/" + day.TripDayId, httpContent);

                var nav = WindowWrapper.Current().NavigationServices.FirstOrDefault();
                nav.Navigate(typeof(Views.GetTripPage));

            }
        }
        /// <summary>
        /// Handles the Click event of the DeleteDayOfTrip control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        public async void DeleteDayOfTrip_Click(object sender, RoutedEventArgs e)
        {
            var id = DayId;
            using (var client = new System.Net.Http.HttpClient())
            {

                client.BaseAddress = new Uri(@"http://localhost:10051/api/");

                await client.DeleteAsync("dayoftrips/" + id);

                var nav = WindowWrapper.Current().NavigationServices.FirstOrDefault();
                nav.Navigate(typeof(Views.GetTripPage));

            }
        }
    }
}
