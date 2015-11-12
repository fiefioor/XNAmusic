using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using XNAmusic.Models;
using Microsoft.Xna.Framework.Media;

namespace XNAmusic
{
    public partial class GalleryPage : PhoneApplicationPage
    {
        List<PictureModel> gm = null;
        MediaLibrary ml = null;
        string album_name = null;
        public GalleryPage()
        {
            InitializeComponent();
            gm = new List<PictureModel>();
            ml = new MediaLibrary();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            gm.Clear();
            if (NavigationContext.QueryString.TryGetValue("Album", out album_name))
            {
                foreach (var item in ml.RootPictureAlbum.Albums)
                {
                    if (item.Name == album_name)
                    {
                        foreach (var item2 in item.Pictures)
                        {
                                gm.Add(new PictureModel(item2.Name, item2.GetThumbnail()));
                        }
                        break;
                    }                  
                }
            }
            PhotoSelector.ItemsSource = gm;
        }

        private void PhotoSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PictureModel tmp = null;
            if (PhotoSelector.SelectedItem == null) return;
            try
            {
                tmp = (PictureModel)PhotoSelector.SelectedItem;
                NavigationService.Navigate(new Uri(string.Format("/PicturePage.xaml?Photo={0}&Album={1}", tmp.Name,album_name), UriKind.Relative));
            }
            catch (Exception e1)
            {

                MessageBox.Show(e1.ToString());
            }
        }
    }
}