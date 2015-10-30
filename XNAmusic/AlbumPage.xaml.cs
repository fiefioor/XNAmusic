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
using XNAmusic.Models;

namespace XNAmusic
{
    public partial class AlbumPage : PhoneApplicationPage
    {
        MediaLibrary ml = null;
        List<AlbumModel> albums = null;
        public AlbumPage()
        {
            InitializeComponent();       
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            ml = new MediaLibrary();
            albums = new List<AlbumModel>();
            String artist_name = null;
            if (NavigationContext.QueryString.TryGetValue("Artist", out artist_name))
            {
                ArtistTextBlock.Text = artist_name;
                foreach(Artist a in ml.Artists)
                {
                    if(a.Name == artist_name)
                    {

                        foreach (Album item in a.Albums)
                        {
                            albums.Add(new AlbumModel(item.Name, item.GetThumbnail()));
                        }
                        break;                    
                    }
                }
                AlbumSelector.ItemsSource = albums;
            }
        }

        private void AlbumSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            /* if (e.AddedItems.Count > 0)
             {
                 var selecedItem = e.AddedItems[0] as Album;
                 if (null != selecedItem) // prevents errors if casting fails
                 {
                     NavigationService.Navigate(new Uri(string.Format("/PlaySongs.xaml?Album={0}", selecedItem.Name), UriKind.Relative));
                 }
             }*/
            AlbumModel tmp = null;
            if (AlbumSelector.SelectedItem == null) return;
            try
            {
                tmp = (AlbumModel)AlbumSelector.SelectedItem;
                NavigationService.Navigate(new Uri(string.Format("/PlaySongs.xaml?Album={0}", tmp.Name), UriKind.Relative));
            }
            catch (Exception e1)
            {

                MessageBox.Show(e1.ToString());
            }

            

        }
    }
}