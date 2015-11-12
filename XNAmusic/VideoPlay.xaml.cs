using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Windows.Storage;

namespace XNAmusic
{
    public partial class VideoPlay : PhoneApplicationPage
    {

        public VideoPlay()
        {
            InitializeComponent();
        }

        private async void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            string video_name = null;
            if (NavigationContext.QueryString.TryGetValue("Video", out video_name))
            {
                StorageFolder videosFolder = KnownFolders.VideosLibrary;

                IReadOnlyList<StorageFile> fileList = await videosFolder.GetFilesAsync();
                IReadOnlyList<StorageFolder> folderList = await videosFolder.GetFoldersAsync();

                var count = fileList.Count + folderList.Count;
                foreach (StorageFile file in fileList)
                {
                    if (file.Name == video_name)
                    {
                        try {
                            path.Text = file.Path;
                            videoPlayer.Source = new Uri(file.Path, UriKind.Absolute);
                            videoPlayer.Play();
                        }catch(Exception e1)
                        {
                            MessageBox.Show(e1.ToString());
                        }
                    }
                }
            }
        }
    }
}