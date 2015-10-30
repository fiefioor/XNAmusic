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
        List<GalleryModel> gm = null;
        MediaLibrary ml = null;
        public GalleryPage()
        {
            InitializeComponent();
            gm = new List<GalleryModel>();
            ml = new MediaLibrary();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            string album_name = null;
            if (NavigationContext.QueryString.TryGetValue("Album", out album_name))
            {
                foreach (var item in ml.RootPictureAlbum.Albums)
                {
                    if (item.Name == album_name)
                    {
                        foreach (var item2 in item.Pictures)
                        {
                            gm.Add(new GalleryModel(item2.Name, item2.GetThumbnail()));
                        }
                        break;
                    }                  
                }
            }
            PhotoSelector.ItemsSource = gm;
        }
    }
}