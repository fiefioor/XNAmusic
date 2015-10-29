using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace XNAmusic.Models
{
    class GalleryModel
    {
        public BitmapImage Art { get; set; }
        public String Name { get; set; }

        public GalleryModel(string name)
        {
            this.Name = Name;
            this.Art = new BitmapImage(new Uri("ms-appx:///Assets/AppBarIcons/question.png", UriKind.Absolute));
        }

        public GalleryModel(String Name, System.IO.Stream stream)
        {
            this.Name = Name;

            this.Art = new BitmapImage();
            if (stream != null)
            {
                this.Art.SetSource(stream);
            }


        }
    }
}
