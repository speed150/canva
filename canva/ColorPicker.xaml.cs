using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace canva
{
    

    /// <summary>
    /// Logika interakcji dla klasy ColorPicker.xaml
    /// </summary>
    public partial class ColorPicker : Window
    {
        public event EventHandler<Color> colorChanged;
        public Color color=new Color();
        public bool changed = false;
        public ColorPicker()
        {
            InitializeComponent();
        }
        private void ColorChanged(object sender, RoutedPropertyChangedEventArgs<double> e){
            color = Color.FromRgb((byte)Red.Value, (byte)Green.Value, (byte)Blue.Value);
            (Hue.Value, Saturation.Value,Lightness.Value ) = RGBToHSL((int)Red.Value, (int)Green.Value, (int)Blue.Value);
            colorChanged?.Invoke(sender, color);
            PenColor.Background = new SolidColorBrush(color);
            changed = true;
        }


        public static (int, double, double) RGBToHSL(int r, int g, int b)
        {
            double hue = 0, saturation = 0, lightness = 0;

            double rNormalized = r / 255.0;
            double gNormalized = g / 255.0;
            double bNormalized = b / 255.0;

            double max = Math.Max(rNormalized, Math.Max(gNormalized, bNormalized));
            double min = Math.Min(rNormalized, Math.Min(gNormalized, bNormalized));
            lightness = (max + min) / 2;
            if (max == min)
            {
                hue = saturation = 0; 
            }
            else
            {
                double delta = max - min;
                saturation = lightness > 0.5 ? delta / (2 - max - min) : delta / (max + min);
                if (max == rNormalized)
                    hue = (gNormalized - bNormalized) / delta + (gNormalized < bNormalized ? 6 : 0);
                else if (max == gNormalized)
                    hue = (bNormalized - rNormalized) / delta + 2;
                else if (max == bNormalized)
                    hue = (rNormalized - gNormalized) / delta + 4;
            }

            hue *= 60;
            saturation *= 100;
            lightness *= 100;
            return ((int)hue, (int)saturation, (int)lightness);
        }
    }
}

