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

namespace XNAmusic
{
    public partial class MainPage : PhoneApplicationPage
    {
        MediaLibrary ml = null;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            ml = new MediaLibrary();
            

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            PopulateList();
            if(MediaPlayer.State == MediaState.Playing)
            {
                NavigationService.Navigate(new Uri(string.Format("/PlaySongs.xaml?Album={0}&Song={1}", MediaPlayer.Queue.ActiveSong.Album.Name, MediaPlayer.Queue.ActiveSong.Name), UriKind.Relative));
            }
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
    }
}