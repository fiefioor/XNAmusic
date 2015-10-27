using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace XNAmusic.Models
{
    class AlbumModel
    {
        public BitmapImage AlbumArt { get; set; }
        public String Name { get; set; }

        public AlbumModel(String Name, System.IO.Stream stream)
        {
            this.Name = Name;

            this.AlbumArt = new BitmapImage();
            if(stream != null)
            {
                AlbumArt.SetSource(stream);
            }
            

        }

    }
}
