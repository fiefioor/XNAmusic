using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using XNAmusic.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Windows.Storage;
using Coding4Fun.Toolkit.Controls;
using XNAmusic.Models;
using Microsoft.Phone.Maps.Controls;
using XNAmusic.MapHelpers;
using Windows.Devices.Geolocation;
using System.Device.Location;
using System.Windows.Shapes;
using System.Windows.Media;

namespace XNAmusic
{
    public partial class MainPage : PhoneApplicationPage
    {
        MediaLibrary ml = null;
        List<GalleryModel> gm = null;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            ml = new MediaLibrary();
            gm = new List<GalleryModel>();

        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
           
            if(MediaPlayer.State == MediaState.Playing)
            {
                NavigationService.Navigate(new Uri(string.Format("/PlaySongs.xaml?Album={0}&Song={1}", MediaPlayer.Queue.ActiveSong.Album.Name, MediaPlayer.Queue.ActiveSong.Name), UriKind.Relative));
            }
            PopulateList();
            populateGalleryList();

        }

        private void PopulateList()
        {
            ArtistsList.ItemsSource = ml.Artists;
        }

        private void ArtistsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(e.AddedItems.Count>0)
            {
                var selecedItem = e.AddedItems[0] as Artist;
                if (null != selecedItem) // prevents errors if casting fails
                {
                    NavigationService.Navigate(new Uri(string.Format("/AlbumPage.xaml?Artist={0}", selecedItem.Name), UriKind.Relative));
                }
            }
        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            AboutPrompt aboutMe = new AboutPrompt();
            aboutMe.Show("Michal Warkoczynski", null, "mjwarkoczynski@wi.zut.edu.pl", "http://wi.zut.edu.pl");
        }

        private void populateGalleryList()
        {       
            if (gm.Count == 0)
            {
                Random rnd = new Random();
                foreach (var item in ml.RootPictureAlbum.Albums)
                {
                    try
                    {
                        gm.Add(new GalleryModel(item.Name, item.Pictures[rnd.Next(item.Pictures.Count)].GetThumbnail()));
                    }
                    catch
                    {
                    }

                }
            }
           
            GallerySelector.ItemsSource = gm;
        }

        private void OnResolveCompleted(object sender, MapResolveCompletedEventArgs e)
        {
            ShowMyLocationOnTheMap();
        }

        private async void ShowMyLocationOnTheMap()
        {
            try
            {
                Geolocator myGeolocator = new Geolocator();
                Geoposition myGeoposition = await myGeolocator.GetGeopositionAsync();
                Geocoordinate myGeocoordinate = myGeoposition.Coordinate;
                GeoCoordinate myGeoCoordinate =
                    CoordinateConverter.ConvertGeocoordinate(myGeocoordinate);

                // Make my current location the center of the Map.
                this.Map.Center = myGeoCoordinate;
                this.Map.ZoomLevel = 13;

                // Create a small circle to mark the current location.
                Ellipse myCircle = new Ellipse();
                myCircle.Fill = new SolidColorBrush(Colors.Blue);
                myCircle.Height = 20;
                myCircle.Width = 20;
                myCircle.Opacity = 50;
                MapOverlay myLocationOverlay = new MapOverlay();
                myLocationOverlay.Content = myCircle;
                myLocationOverlay.PositionOrigin = new System.Windows.Point(0.5, 0.5);
                myLocationOverlay.GeoCoordinate = myGeoCoordinate;
                MapLayer myLocationLayer = new MapLayer();
                myLocationLayer.Add(myLocationOverlay);
                Map.Layers.Add(myLocationLayer);
            }
            catch (Exception e1)
            {
               MessageBox.Show( e1.ToString() );
            }
            // Get my current location.
           
        }

        private void GallerySelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GalleryModel tmp = null;
            if (GallerySelector.SelectedItem == null) return;
            try
            {
                tmp = (GalleryModel)GallerySelector.SelectedItem;
                NavigationService.Navigate(new Uri(string.Format("/GalleryPage.xaml?Album={0}", tmp.Name), UriKind.Relative));
            }
            catch (Exception e1)
            {

                MessageBox.Show(e1.ToString());
            }
        }
    }
}