using AegirLib.Simulation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Aegir.Converter
{
    public class PlaybackButtonConverter : EnumToBoolConverter
    {
        public override object ConvertBack(object value, Type targetType, 
                                object parameter, CultureInfo culture)
        {
            Debug.WriteLine("Converting Back");
            //Check that our value is bool and parameter enum
            if (!(value is bool) || !(parameter is EPlaybackMode)) return Binding.DoNothing;
            //If value is false, we did not click on the other button.. And therefor
            //we know that we are not chaning from play to rewind or vice versa
            //and can safely set the result to paused
            //If value is true we want to set to the type defined in our 
            //converter parameter (set in XAML).
            if ((bool)value == true) return parameter;
            return EPlaybackMode.PAUSED;
        }
    }
}
