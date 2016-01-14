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
using Microsoft.Xna.Framework.Media;
using System.Windows.Threading;

namespace XNAmusic
{
    public partial class VideoPlay : PhoneApplicationPage
    {
        DispatcherTimer playTimer;
        StorageFile video;
        Windows.Storage.FileProperties.VideoProperties x;
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
                            video = file;
                            x = await video.Properties.GetVideoPropertiesAsync();
                            videoPlayer.Play();
                            //pbVideo.Maximum = (int)videoPlayer.NaturalDuration.TimeSpan.TotalSeconds;
                            changeProgressBar();
                        }
                        catch(Exception e1)
                        {
                            MessageBox.Show(e1.ToString());
                        }
                    }
                }
            }
        }

        private void changeProgressBar()
        {
            pbVideo.Maximum = 100.00;
            playTimer = new DispatcherTimer();
                playTimer.Interval = TimeSpan.FromMilliseconds(1000); //one second
                playTimer.Tick += new EventHandler(playTimer_Tick);
                playTimer.Start();
        }

        public void playTimer_Tick(object sender, EventArgs e)
        {
             pbVideo.Value = (videoPlayer.Position.TotalSeconds/ videoPlayer.NaturalDuration.TimeSpan.TotalSeconds) * 100;
            path.Text = ((int)((pbVideo.Value * videoPlayer.NaturalDuration.TimeSpan.TotalSeconds) / 100)).ToString() + " / " + ((int)videoPlayer.NaturalDuration.TimeSpan.TotalSeconds).ToString();
        }

        private void videoPlayer_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if(videoPlayer.CurrentState == System.Windows.Media.MediaElementState.Playing)
            {
                videoPlayer.Pause();
            }
            else
            {
                videoPlayer.Play();
            }
        }

        private void pbVideo_ManipulationCompleted(object sender, System.Windows.Input.ManipulationCompletedEventArgs e)
        {
            //path.Text = pbVideo.Value.ToString();
            videoPlayer.Position = TimeSpan.FromSeconds(((pbVideo.Value* videoPlayer.NaturalDuration.TimeSpan.TotalSeconds)/100));
            //path.Text = ((pbVideo.Value * videoPlayer.NaturalDuration.TimeSpan.TotalSeconds) / 100).ToString();
        }
    }
}