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
    public partial class PlaySongs : PhoneApplicationPage
    {

        MediaLibrary ml = null;
        Album CurrentAlbum = null;
        Song currentSong = null;
        int song_num = 0;
        public PlaySongs()
        {
            InitializeComponent();
            SongName.Text = "";
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            ml = new MediaLibrary();
            String album_name = null;
            if (NavigationContext.QueryString.TryGetValue("Album", out album_name))
            {
                AlbumName.Text = album_name;
                foreach (Album a in ml.Albums)
                {
                    if (a.Name == album_name)
                    {
                        CurrentAlbum = a;
                        SongsList.ItemsSource = a.Songs;
                    }
                }
            }
        }

        private void nextButton_Click(object sender, RoutedEventArgs e)
        {
            MediaPlayer.MoveNext();
            changePlayButtonStatus();
            changeSongName();
            song_num++;
            if (song_num > SongsList.Items.Count - 1) song_num = SongsList.Items.Count - 1;
        }

        private void prevButton_Click(object sender, RoutedEventArgs e)
        {
            MediaPlayer.MovePrevious();
            changePlayButtonStatus();
            changeSongName();
            song_num--;
            if (song_num < 0) song_num = 0;

        }

        private void playButton_Click(object sender, RoutedEventArgs e)
        {
            if(MediaPlayer.State == MediaState.Playing)
            {
                MediaPlayer.Pause();
                changePlayButtonStatus();
                changeSongName();
            }
            else
            {
                if (currentSong != null)
                {
                    MediaPlayer.Play(CurrentAlbum.Songs, song_num);
                    changePlayButtonStatus();
                    changeSongName();
                }
            }
        }

        private void SongsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                var selecedItem = e.AddedItems[0] as Song;
                if (null != selecedItem) // prevents errors if casting fails
                {
                    song_num = SongsList.SelectedIndex;
                    currentSong = selecedItem;
                    MediaPlayer.Stop();
                    MediaPlayer.Play(CurrentAlbum.Songs, song_num);
                    changePlayButtonStatus();
                    changeSongName();
                }
            }
        }

        private void changePlayButtonStatus()
        {
            if(MediaPlayer.State == MediaState.Playing)
            {
                playButton.Content = "Pause";
            }
            else
            {
                playButton.Content = "Play";
            }
        }

        private void changeSongName()
        {
            if (currentSong != null) SongName.Text = currentSong.Name;
            else SongName.Text = "";
        }

        private void changeProgressBar()
        {
            if (currentSong != null)
            {
                SongProgress.Maximum = currentSong.Duration.TotalSeconds;
            }
        }
    }
}