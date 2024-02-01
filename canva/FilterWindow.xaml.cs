using Emgu.CV.CvEnum;
using Emgu.CV;
using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.Windows.Interop;
using Emgu.CV.Structure;
using System.Drawing.Imaging;
using System.IO;

namespace canva
{
    /// <summary>
    /// Logika interakcji dla klasy FilterWindow.xaml
    /// </summary>
    public partial class FilterWindow : Window
    {
        public FilterWindow()
        {
            InitializeComponent();
            
        }
        Uri uri ;
        private void Upload_Click(object sender, RoutedEventArgs e)
        {
           OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "png Files(*.png)|*.png|jpg files(*.jpg)|*.jpg";
            if (ofd.ShowDialog()==true)
            {
                string path = ofd.FileName;
                DisplayImage(path);
            }
            
        }
        BitmapImage bitmap = new BitmapImage();
        private void DisplayImage(string path)
        {
            try
            {
                
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(path);
                bitmap.EndInit();
                Image.Source = bitmap;
                uri = new Uri(path);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading image: {ex.Message}");
            }
        }
        private void Transform_Click(object sender, RoutedEventArgs e)
        {
            Mat image = new Mat();
            if (uri != null)
            {
                image = CvInvoke.Imread(uri.LocalPath, ImreadModes.Grayscale);
            }
            if (image.IsEmpty)
            {
                Console.WriteLine("Failed to load image.");
                return;
            }


            Mat sobelImage = new Mat();
            CvInvoke.Sobel(image, sobelImage, DepthType.Cv8U, 1, 1);


            Bitmap bitmap = sobelImage.ToBitmap();


            ImageSource imageSource = ToImageSource(bitmap);

            Transformed.Source = imageSource;
        }
        static ImageSource ToImageSource(Bitmap bitmap)
        {
            BitmapSource bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(
                bitmap.GetHbitmap(),
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());

            return bitmapSource;
        }
        private void ApplyLinearFilterButton_Click(object sender, RoutedEventArgs e)
        {
          
            WriteableBitmap writeableBitmap = new WriteableBitmap(bitmap);

            double[,] filterMatrix = {
                { Int32.Parse( N0.Text), Int32.Parse(N1.Text),Int32.Parse(N2.Text) },
                { Int32.Parse(N3.Text), Int32.Parse(N4.Text), Int32.Parse(N5.Text) },
                { Int32.Parse(N6.Text), Int32.Parse(N7.Text), Int32.Parse(N8.Text) }
            };

            ApplyLinearFilter(writeableBitmap, filterMatrix);

            Transformed.Source = writeableBitmap;
        }

        private void ApplyLinearFilter(WriteableBitmap image, double[,] filter)
        {
            int width = image.PixelWidth;
            int height = image.PixelHeight;

            int stride = width * 4; // Każdy piksel zajmuje 4 bajty (ARGB)

            byte[] pixels = new byte[height * stride];
            image.CopyPixels(pixels, stride, 0);

            byte[] resultPixels = new byte[height * stride];

            int filterSize = filter.GetLength(0);
            int filterOffset = filterSize / 2;

            for (int y = filterOffset; y < height - filterOffset; y++)
            {
                for (int x = filterOffset; x < width - filterOffset; x++)
                {
                    double red = 0, green = 0, blue = 0;

                    for (int i = 0; i < filterSize; i++)
                    {
                        for (int j = 0; j < filterSize; j++)
                        {
                            int pixelIndex = ((y + i - filterOffset) * stride) + ((x + j - filterOffset) * 4);

                            red += pixels[pixelIndex + 2] * filter[i, j];
                            green += pixels[pixelIndex + 1] * filter[i, j];
                            blue += pixels[pixelIndex] * filter[i, j];
                        }
                    }

                    int resultIndex = (y * stride) + (x * 4);
                    resultPixels[resultIndex + 2] = Clamp((int)red);
                    resultPixels[resultIndex + 1] = Clamp((int)green);
                    resultPixels[resultIndex] = Clamp((int)blue);
                    resultPixels[resultIndex + 3] = 255; // Alpha

                }
            }

            image.WritePixels(new Int32Rect(0, 0, width, height), resultPixels, stride, 0);
        }

        private byte Clamp(int value)
        {
            return (byte)(value < 0 ? 0 : (value > 255 ? 255 : value));
        }
    }
}

