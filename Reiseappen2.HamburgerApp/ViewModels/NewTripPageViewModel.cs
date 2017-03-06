using Newtonsoft.Json;
using Reiseappen2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Template10.Common;
using Windows.UI.Popups;

namespace Reiseappen2.HamburgerApp.ViewModels
{
    public class NewTripPageViewModel
    {
        public string Name { get; set; }

        /// <summary>
        /// Adds a trip when button clicked.
        /// </summary>
        public async void AddTrip_Click()
        {
            
            if(Name == null || Name == "")
            {
                var dialog = new MessageDialog("You must fill all fields");
                dialog.Commands.Add(new UICommand("Ok") { Id = 0 });

                dialog.CancelCommandIndex = 0;

                var result = await dialog.ShowAsync();

                return;
            }
            Trip trip = new Trip();
            trip.Name = Name;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(@"http://localhost:10051/api/");

                var content = JsonConvert.SerializeObject(trip);

                var httpContent = new StringContent(content, Encoding.UTF8, "application/json");

                var json = await client.PostAsync("trips", httpContent);

                var nav = WindowWrapper.Current().NavigationServices.FirstOrDefault();
                nav.Navigate(typeof(Views.GetTripPage));

            }
        }
    }
}
