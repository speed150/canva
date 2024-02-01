using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace canva
{
    public partial class MainWindow : Window
    {
        
        Point currentPoint = new Point();
        public Color color = Color.FromRgb(0, 0, 0);
        
        int DrawStyle = 0;
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Canvas_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
                currentPoint = e.GetPosition(this);
        }
        private void ColorButton_Click(object sender, RoutedEventArgs e)
        {
            PenColor.Background = new SolidColorBrush(color);
            ColorPicker colorPicker = new ColorPicker();
            colorPicker.Red.Value = color.R;
            colorPicker.Green.Value = color.G;
            colorPicker.Blue.Value = color.B;

            colorPicker.colorChanged += ColorPicker_ColorChanged;
            colorPicker.Show();

        }
        private void ColorPicker_ColorChanged(object sender, Color color)
        {
            this.color =color;
            PenColor.Background= new SolidColorBrush(color);
        }

        private void Canvas_MouseMove_1(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && DrawStyle == 0)
            {
                Line line = new Line();
                line.Stroke = PenColor.Background;
                line.X1 = currentPoint.X;
                line.Y1 = currentPoint.Y;
                line.X2 = e.GetPosition(this).X;
                line.Y2 = e.GetPosition(this).Y;
                currentPoint = e.GetPosition(this);
                paintSurface.Children.Add(line);
            }
            if (DrawStyle == 9 && e.LeftButton == MouseButtonState.Pressed)
            {
                var clickedElement = e.Source as FrameworkElement;

                if (clickedElement != null)
                {
                    if (paintSurface.Children.Contains(clickedElement))
                    {
                        paintSurface.Children.Remove(clickedElement);
                    }
                }
            }
        }


        private void DrawButton_Click(object sender, RoutedEventArgs e)
        {
            DrawStyle = 0;
        }

        private void PointButton_Click(object sender, RoutedEventArgs e)
        {
            DrawStyle = 1;
        }
        private void LineButton_Click(object sender, RoutedEventArgs e)
        {
            DrawStyle = 2;
            firstClick = true;
        }
        private Ellipse DrawPoint(double size, double x, double y)
        {
            Ellipse point = new Ellipse();
            point.Width = size;
            point.Height = size;
            Canvas.SetTop(point, y);
            Canvas.SetLeft(point, x);
            Brush brush = PenColor.Background;
            point.Fill = brush;
            paintSurface.Children.Add(point);
            return point;
        }
        double xStart;
        double yStart;
        double xStartM;
        double yStartM;
        bool firstClick = true;
        bool firstClickM = true;
        private void paintSurface_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (DrawStyle == 1)
            {
                Ellipse ellipse = DrawPoint(6, e.GetPosition(this).X, e.GetPosition(this).Y);
            }
            if (DrawStyle == 2)
            {
                if (firstClick)
                {
                    xStart = e.GetPosition(this).X;
                    yStart = e.GetPosition(this).Y;
                    firstClick = false;
                }
                if(xStart!= e.GetPosition(this).X && yStart != e.GetPosition(this).Y && !firstClick)
                {
                    Line l = new Line {X1 = xStart,Y1=yStart,X2 = e.GetPosition(this).X,Y2= e.GetPosition(this).Y ,Stroke=PenColor.Background};
                    paintSurface.Children.Add(l);
                    firstClick = true;
                }

            }
            if(DrawStyle == 3)
            {
                if (firstClickM)
                {
                    xStartM = e.GetPosition(this).X;
                    yStartM = e.GetPosition(this).Y;
                    firstClickM = false;
                }
                if (xStartM != e.GetPosition(this).X && yStartM != e.GetPosition(this).Y && !firstClickM)
                {
                    Line l = new Line { X1 = xStartM, Y1 = yStartM, X2 = e.GetPosition(this).X, Y2 = e.GetPosition(this).Y, Stroke = PenColor.Background };
                    xStartM = e.GetPosition(this).X;
                    yStartM = e.GetPosition(this).Y;
                    paintSurface.Children.Add(l);
                    
                }
            }
            if (DrawStyle == 6)
            {
                Polygon polygon = new Polygon();
                double x = e.GetPosition(this).X;
                double y = e.GetPosition(this).Y;
                
                double polysize =20.0;
                PointCollection points = new PointCollection {
                    new Point(x - polysize, y + 2 * polysize),
                    new Point(x + polysize, y + 2 * polysize),
                    new Point(x + 2 * polysize, y),
                    new Point(x + polysize, y - 2 * polysize),
                    new Point(x - polysize, y - 2 * polysize),
                    new Point(x - 2 * polysize, y)
                };

                polygon.Points = points;
                polygon.Stroke = PenColor.Background;
                paintSurface.Children.Add(polygon);
            }
            if(DrawStyle == 7)
            {
                Polygon poly = new Polygon();
                double x = e.GetPosition(this).X;
                double y = e.GetPosition (this).Y;
                double polysize = 20.0;
                PointCollection points1 = new PointCollection
                {
                   new Point(x,y),
                   new Point(x+polysize,y),
                   new Point (x+polysize,y-polysize),
                   new Point(x, y-polysize)
                };
                poly.Points = points1;
                poly.Stroke = PenColor.Background;
                paintSurface.Children.Add(poly);
            }
            if (DrawStyle == 8)
            {
                
                double x = e.GetPosition(this).X;
                double y = e.GetPosition(this).Y;
                double polysize = 40.0;
                Ellipse poly = new Ellipse
                {
                    Width=polysize,
                    Height=polysize,
                    Stroke= PenColor.Background
                };
                Canvas.SetLeft(poly, x - (polysize/2));
                Canvas.SetTop(poly, y - (polysize / 2));
                paintSurface.Children.Add(poly);
            }
            if (DrawStyle == 9 && e.LeftButton == MouseButtonState.Pressed)
            {
                var clickedElement = e.Source as FrameworkElement;

                if (clickedElement != null)
                {
                    if (paintSurface.Children.Contains(clickedElement))
                    {
                        paintSurface.Children.Remove(clickedElement);
                    }
                }
            }
        }

        private void DrawPoly_Click(object sender, RoutedEventArgs e)
        {
            DrawStyle = 6;
        }

        private void DrawRect_Click(object sender, RoutedEventArgs e)
        {
            DrawStyle = 7;
        }
        private void Ellipse_Click(object sender, RoutedEventArgs e)
        {
            DrawStyle= 8;
        }
        public void Save_to_png(Uri uri,Canvas canvas)
        {
            if(uri == null)
            {
                return;
            }
            Transform transform = canvas.LayoutTransform;
            canvas.LayoutTransform = null;
            Size size = new Size(canvas.ActualWidth, canvas.ActualHeight);
            canvas.Arrange(new Rect(size));
            RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap((int)size.Width, (int)size.Height,96d,96d,PixelFormats.Pbgra32);
            renderTargetBitmap.Render(canvas);
            using (FileStream outstream = new(uri.LocalPath,FileMode.Create))
            {
                PngBitmapEncoder pngBitmapEncoder = new PngBitmapEncoder();
                pngBitmapEncoder.Frames.Add(BitmapFrame.Create(renderTargetBitmap));
                pngBitmapEncoder.Save(outstream);
            }
            paintSurface.LayoutTransform = transform;
        }
        public void Save_to_jpg(Uri uri, Canvas canvas)
        {
            if (uri == null)
            {
                return;
            }
            Transform transform = canvas.LayoutTransform;
            canvas.LayoutTransform = null;
            Size size = new Size(canvas.ActualWidth, canvas.ActualHeight);
            canvas.Arrange(new Rect(size));
            RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap((int)size.Width, (int)size.Height, 96d, 96d, PixelFormats.Pbgra32);
            renderTargetBitmap.Render(canvas);
            using (FileStream outstream = new(uri.LocalPath, FileMode.Create))
            {
                JpegBitmapEncoder jpegBitmapEncoder = new JpegBitmapEncoder();
                jpegBitmapEncoder.Frames.Add(BitmapFrame.Create(renderTargetBitmap));
                jpegBitmapEncoder.Save(outstream);
            }
            paintSurface.LayoutTransform = transform;
        }
        private void pngSave_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new();
            saveFileDialog.Filter = "Image File (*.png)|*.png|Image File(*.jpeg)|*.jpeg";
            saveFileDialog.FilterIndex = 1;
            if (saveFileDialog.ShowDialog() == true) {
                Uri uri = new Uri(saveFileDialog.FileName);
                string ext = System.IO.Path.GetExtension(uri.LocalPath);
                if(ext == ".png")
                {
                    Save_to_png(uri, paintSurface);
                }
                else
                {
                    Save_to_jpg(uri, paintSurface);
                }
               

            }

        }
        private void Filter_Click(object sender, RoutedEventArgs e)
        {
            FilterWindow filterWindow = new FilterWindow();
            filterWindow.Show();
        }

        private void mLine_Click(object sender, RoutedEventArgs e)
        {
            DrawStyle = 3;
            bool firstClickM = true;
        }

        private void Rubber_Click(object sender, RoutedEventArgs e)
        {
            DrawStyle = 9;
        }
    }
}
