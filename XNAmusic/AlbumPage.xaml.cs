using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Xna.Framework.Media;

namespace XNAmusic
{
    public partial class AlbumPage : PhoneApplicationPage
    {
        MediaLibrary ml = null;
        public AlbumPage()
        {
            InitializeComponent();       
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            ml = new MediaLibrary();
            String artist_name = null;
            if (NavigationContext.QueryString.TryGetValue("Artist", out artist_name))
            {
                ArtistTextBlock.Text = artist_name;
                foreach(Artist a in ml.Artists)
                {
                    if(a.Name == artist_name)
                    {
                        AlbumList.ItemsSource = a.Albums;
                    }
                }
            }
        }

        private void AlbumList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                var selecedItem = e.AddedItems[0] as Album;
                if (null != selecedItem) // prevents errors if casting fails
                {
                    NavigationService.Navigate(new Uri(string.Format("/PlaySongs.xaml?Album={0}", selecedItem.Name), UriKind.Relative));
                }
            }
        }
    }
}