using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public static class Helper 
{
    public static bool TryParse(string value, out float floatValue)
    {
        try
        {
            if (value.Contains(","))
                value = value.Replace(",", ".");
            floatValue = float.Parse(value, CultureInfo.InvariantCulture);
            return true;
        }
        catch
        {
            floatValue = 0;
            return false;
        }
    }

    public static bool TryParse(string value, out double doubleValue)
    {
        try
        {
            doubleValue = double.Parse(value, CultureInfo.InvariantCulture);
            return true;
        }
        catch
        {
            doubleValue = -1;
            return false;
        }
    }
}
