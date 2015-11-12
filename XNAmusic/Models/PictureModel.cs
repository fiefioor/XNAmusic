using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace XNAmusic.Models
{
    class PictureModel
    {
        public BitmapImage Art { get; set; }
        public BitmapImage Thumbnail { get; set; }
        public String Name { get; set; }

        public PictureModel(string name)
        {
            this.Name = Name;
            this.Art = new BitmapImage(new Uri("ms-appx:///Assets/AppBarIcons/question.png", UriKind.Absolute));
            this.Thumbnail = this.Art;
        }

        public PictureModel(String Name, System.IO.Stream stream)
        {
            this.Name = Name;

            this.Thumbnail = new BitmapImage();
            if (stream != null)
            {
                this.Thumbnail.SetSource(stream);
            }


        }
        /// <summary>
        /// Constructor with thumbnail and whole image
        /// </summary>
        /// <param name="Name">Pic name</param>
        /// <param name="streamArt"> Whole picture </param>
        /// <param name="streamThumb"> Thumbnail </param>

        public PictureModel(String Name, System.IO.Stream streamArt, System.IO.Stream streamThumb)
        {
            this.Name = Name;

            this.Art = new BitmapImage();
            if (streamArt != null)
            {
                this.Art.SetSource(streamArt);
            }

            this.Thumbnail = new BitmapImage();
            if (streamThumb != null)
            {
                this.Thumbnail.SetSource(streamThumb);
            }

        }
    }
}
