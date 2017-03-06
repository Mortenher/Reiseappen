using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace Reiseappen2.HamburgerApp.Converters
{
    public class ColorFromStringConverter
    {
        public Color ConvertFromString(string HexColor)
        {
            byte A = Convert.ToByte(120);
            byte R = Convert.ToByte(HexColor.Substring(1, 2), 16);
            byte G = Convert.ToByte(HexColor.Substring(3, 2), 16);
            byte B = Convert.ToByte(HexColor.Substring(5, 2), 16);
            Color scb = new Color();
            scb = Color.FromArgb(A, R, G, B);

            return scb;
            
        }

        
    }
}
