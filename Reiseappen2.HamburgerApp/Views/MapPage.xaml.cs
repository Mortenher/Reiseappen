using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Reiseappen2.HamburgerApp.Converters;
using Reiseappen2.HamburgerApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization.Json;
using System.Text;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Reiseappen2.HamburgerApp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MapPage : Page
    {
        public MapPage()
        {
            this.InitializeComponent();
            ((MapPageViewModel)this.DataContext).mapPage = this;
            
        }
        public string BingMapKey = "TsQl5bdTnOVdzuMI4xFA~l3tQaMD2MukJBN84USJ3gA~AkDkQArT2bXsc0Jt6rFBQQLIgzaz8Qz3pTx5GxphkTZBxA6RzuDJ89MVOJm6mpb9";

        /// <summary>
        /// Handles the Loaded event of the myMap control.
        /// Sets center of map to be Halden.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void myMap_Loaded(object sender, RoutedEventArgs e)
        {
            myMap.Center =
              new Geopoint(new BasicGeoposition()
              {
                  //Geopoint for Halden 
                  Latitude = 59.1141,
                  Longitude = 11.3937
              });
            myMap.ZoomLevel = 8;
        }

        /// <summary>
        /// Inserts the map icon on the map from Code-behind, due to accessing the map from ViewModel is troublesome. 
        /// </summary>
        /// <param name="Locations">The locations.</param>
        public async void Insert_MapIcon_CodeBehind(List<string> Locations)
        {
            myMap.MapElements.Clear();
            var geoList = new List<BasicGeoposition>();
            using (var client = new HttpClient())
            {
                foreach (var loc in Locations)
                {
                    Uri geocodeRequest = new Uri(string.Format("http://dev.virtualearth.net/REST/v1/Locations?locality=" + loc + "&key=" + BingMapKey));
                    var json = await client.GetStringAsync(geocodeRequest);

                    dynamic response = JObject.Parse(json);
                    JArray latlng = (JArray)response["resourceSets"][0]["resources"][0]["point"]["coordinates"];

                    BasicGeoposition basicGeoposition = new BasicGeoposition() { Latitude = (double)latlng[0], Longitude = (double)latlng[1] };
                    geoList.Add(basicGeoposition);

                    Geopoint geoPoint = new Geopoint(basicGeoposition);
                    MapIcon mapIcon = new MapIcon();
                    mapIcon.Location = geoPoint;

                    myMap.MapElements.Add(mapIcon);
                    

                }
                AddLineToMap(geoList);
                
            }
        }
        /// <summary>
        /// Adds a line to map between geopositions, called after mapicon-method has run to draw a line starting in day 1 and ending in day n.
        /// </summary>
        /// <param name="geoList">The geo list.</param>
        public void AddLineToMap(List<BasicGeoposition> geoList)
        {
            var polyline = new MapPolyline();
            polyline.Path = new Geopath(geoList);
            polyline.StrokeColor = Colors.Blue;
            polyline.StrokeThickness = 2;
            polyline.StrokeDashed = true;

            myMap.MapElements.Add(polyline);
        }
    }
}
