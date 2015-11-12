using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Xna.Framework.Media;
using System.Windows.Input;

namespace XNAmusic
{
    public partial class PicturePage : PhoneApplicationPage
    {
        //Uri pic_URI = null;
        MediaLibrary ml = null;

        private double m_Zoom = 1;
        private double m_Width = 0;
        private double m_Height = 0;

        // these two fully define the zoom state:
     /*   private double TotalImageScale = 1d;
        private Point ImagePosition = new Point(0, 0);

        private Point _oldFinger1;
        private Point _oldFinger2;
        private double _oldScaleFactor;
        */
        public PicturePage()
        {
            InitializeComponent();
            ml = new MediaLibrary();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            String pic_name = null;
            String album_name = null;
            if (NavigationContext.QueryString.TryGetValue("Album", out album_name) && NavigationContext.QueryString.TryGetValue("Photo", out pic_name))
            {
                   
                    foreach (var item in ml.RootPictureAlbum.Albums)
                    {
                        if (item.Name == album_name)
                        {
                            foreach (var item2 in item.Pictures)
                            {
                                if(item2.Name == pic_name)
                                 {
                               BitmapImage tmp = new BitmapImage();
                                tmp.SetSource(item2.GetImage());
                                myImage.Source = tmp;
                                
                                }
                            }
                            break;
                        }
                    }
                }
        }

        private void viewport_ManipulationDelta(object sender, System.Windows.Input.ManipulationDeltaEventArgs e)
        {
            if (e.PinchManipulation != null)
            {

                double newWidth, newHieght;


                if (m_Width < m_Height)  // box new size between image size and viewport actual size 
                {
                    newHieght = m_Height * m_Zoom * e.PinchManipulation.CumulativeScale;
                    newHieght = Math.Max(viewport.ActualHeight, newHieght);
                    newHieght = Math.Min(newHieght, m_Height);
                    newWidth = newHieght * m_Width / m_Height;
                }
                else
                {
                    newWidth = m_Width * m_Zoom * e.PinchManipulation.CumulativeScale;
                    newWidth = Math.Max(viewport.ActualWidth, newWidth);
                    newWidth = Math.Min(newWidth, m_Width);
                    newHieght = newWidth * m_Height / m_Width;
                }


                if (newWidth < m_Width && newHieght < m_Height)
                {
                    // Tells image positione in viewport (offset) 
                    MatrixTransform transform = myImage.TransformToVisual(viewport) as MatrixTransform;
                    // Calculate center of pinch gesture on image (not screen) 
                    Point pinchCenterOnImage = transform.Transform(e.PinchManipulation.Original.Center);
                    // Calculate relative point (0-1) of pinch center in image 
                    Point relativeCenter = new Point(e.PinchManipulation.Original.Center.X / myImage.Width, e.PinchManipulation.Original.Center.Y / myImage.Height);
                    // Calculate and set new origin point of viewport 
                    Point newOriginPoint = new Point(relativeCenter.X * newWidth - pinchCenterOnImage.X, relativeCenter.Y * newHieght - pinchCenterOnImage.Y);
                    viewport.SetViewportOrigin(newOriginPoint);
                }

                myImage.Width = newWidth;
                myImage.Height = newHieght;

                // Set new view port bound 
                viewport.Bounds = new Rect(0, 0, newWidth, newHieght);
            }
        }

        private void viewport_ManipulationCompleted(object sender, System.Windows.Input.ManipulationCompletedEventArgs e)
        {
            m_Zoom = myImage.Width / m_Width;
        }

        private void myImage_Loaded(object sender, RoutedEventArgs e)
        {
            this.UpdateLayout();
            myImage.Width = myImage.ActualWidth;
            myImage.Height = myImage.ActualHeight;
            ActualH.Text = myImage.ActualHeight.ToString();
            ActualW.Text = myImage.ActualWidth.ToString();
            m_Width = myImage.Width;
            m_Height = myImage.Height;
            // Initaialy we put Stretch to None in XAML part, so we can read image ActualWidth i ActualHeight (otherwise values are strange)
            // After that we set Stretch to UniformToFill in order to be able to resize image
            myImage.Stretch = Stretch.UniformToFill;
            viewport.Bounds = new Rect(0, 0, myImage.Width, myImage.Height);
        }


        /*
        // 2 podejscie
        private void Image_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            if (e.PinchManipulation != null)
            {
                var transform = (CompositeTransform)myImage.RenderTransform;

                // Scale Manipulation
                transform.ScaleX = e.PinchManipulation.CumulativeScale;
                transform.ScaleY = e.PinchManipulation.CumulativeScale;

                // Translate manipulation
                var originalCenter = e.PinchManipulation.Original.Center;
                var newCenter = e.PinchManipulation.Current.Center;
                transform.TranslateX = newCenter.X - originalCenter.X;
                transform.TranslateY = newCenter.Y - originalCenter.Y;

                // Rotation manipulation
                transform.Rotation = angleBetween2Lines(
                    e.PinchManipulation.Current,
                    e.PinchManipulation.Original);

                // end 
                e.Handled = true;
            }
        }
        public static double angleBetween2Lines(PinchContactPoints line1, PinchContactPoints line2)
        {
            if (line1 != null && line2 != null)
            {
                double angle1 = Math.Atan2(line1.PrimaryContact.Y - line1.SecondaryContact.Y,
                                           line1.PrimaryContact.X - line1.SecondaryContact.X);
                double angle2 = Math.Atan2(line2.PrimaryContact.Y - line2.SecondaryContact.Y,
                                           line2.PrimaryContact.X - line2.SecondaryContact.X);
                return (angle1 - angle2) * 180 / Math.PI;
            }
            else { return 0.0; }
        }

        */

        /*
        // 1 podejscie
        private void OnPinchStarted(object s, PinchStartedGestureEventArgs e)
        {
            _oldFinger1 = e.GetPosition(myImage, 0);
            _oldFinger2 = e.GetPosition(myImage, 1);
            _oldScaleFactor = 1;
        }

        private void OnPinchDelta(object s, PinchGestureEventArgs e)
        {
            var scaleFactor = e.DistanceRatio / _oldScaleFactor;

            var currentFinger1 = e.GetPosition(myImage, 0);
            var currentFinger2 = e.GetPosition(myImage, 1);

            var translationDelta = GetTranslationDelta(
                currentFinger1,
                currentFinger2,
                _oldFinger1,
                _oldFinger2,
                ImagePosition,
                scaleFactor);

            _oldFinger1 = currentFinger1;
            _oldFinger2 = currentFinger2;
            _oldScaleFactor = e.DistanceRatio;

            UpdateImage(scaleFactor, translationDelta);
        }

        private void UpdateImage(double scaleFactor, Point delta)
        {
            TotalImageScale *= scaleFactor;
            ImagePosition = new Point(ImagePosition.X + delta.X, ImagePosition.Y + delta.Y);

            var transform = (CompositeTransform)myImage.RenderTransform;
            transform.ScaleX = TotalImageScale;
            transform.ScaleY = TotalImageScale;
            transform.TranslateX = ImagePosition.X;
            transform.TranslateY = ImagePosition.Y;
        }

        private Point GetTranslationDelta(
            Point currentFinger1, Point currentFinger2,
            Point oldFinger1, Point oldFinger2,
            Point currentPosition, double scaleFactor)
        {
            var newPos1 = new Point(
                currentFinger1.X + (currentPosition.X - oldFinger1.X) * scaleFactor,
                currentFinger1.Y + (currentPosition.Y - oldFinger1.Y) * scaleFactor);

            var newPos2 = new Point(
                currentFinger2.X + (currentPosition.X - oldFinger2.X) * scaleFactor,
                currentFinger2.Y + (currentPosition.Y - oldFinger2.Y) * scaleFactor);

            var newPos = new Point(
                (newPos1.X + newPos2.X) / 2,
                (newPos1.Y + newPos2.Y) / 2);

            return new Point(
                newPos.X - currentPosition.X,
                newPos.Y - currentPosition.Y);
        }*/

    }
}