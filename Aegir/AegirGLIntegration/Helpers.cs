using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

/// <summary> Various addon functions </summary>
public static class Helpers
{
    static Random rand = new Random();
    public static T GetRandomItem<T>(this IEnumerable<T> list)
    {
        int index = rand.Next(0, list.Count());
        return list.ElementAt(index);
    }

    public static System.Windows.Media.Color HSVtoRGB(int Hue, int Saturation, int value)
    {
        // HSV contains values scaled as in the color wheel:
        // that is, all from 0 to 255. 

        // for ( this code to work, HSV.Hue needs
        // to be scaled from 0 to 360 (it//s the angle of the selected
        // point within the circle). HSV.Saturation and HSV.value must be 
        // scaled to be between 0 and 1.

        double h;
        double s;
        double v;

        double r = 0;
        double g = 0;
        double b = 0;

        // Scale Hue to be between 0 and 360. Saturation
        // and value scale to be between 0 and 1.
        h = ((double)Hue / 255 * 360) % 360;
        s = (double)Saturation / 255;
        v = (double)value / 255;

        if (s == 0)
        {
            // If s is 0, all colors are the same.
            // This is some flavor of gray.
            r = v;
            g = v;
            b = v;
        }
        else
        {
            double p;
            double q;
            double t;

            double fractionalSector;
            int sectorNumber;
            double sectorPos;

            // The color wheel consists of 6 sectors.
            // Figure out which sector you//re in.
            sectorPos = h / 60;
            sectorNumber = (int)(Math.Floor(sectorPos));

            // get the fractional part of the sector.
            // That is, how many degrees into the sector
            // are you?
            fractionalSector = sectorPos - sectorNumber;

            // Calculate values for the three axes
            // of the color. 
            p = v * (1 - s);
            q = v * (1 - (s * fractionalSector));
            t = v * (1 - (s * (1 - fractionalSector)));

            // Assign the fractional colors to r, g, and b
            // based on the sector the angle is in.
            switch (sectorNumber)
            {
                case 0:
                    r = v;
                    g = t;
                    b = p;
                    break;

                case 1:
                    r = q;
                    g = v;
                    b = p;
                    break;

                case 2:
                    r = p;
                    g = v;
                    b = t;
                    break;

                case 3:
                    r = p;
                    g = q;
                    b = v;
                    break;

                case 4:
                    r = t;
                    g = p;
                    b = v;
                    break;

                case 5:
                    r = v;
                    g = p;
                    b = q;
                    break;
            }
        }
        // return an RGB structure, with values scaled
        // to be between 0 and 255.
        return Color.FromArgb(
            255,
            (byte)(r * 255),
            (byte)(g * 255),
            (byte)(b * 255));
    }
}

