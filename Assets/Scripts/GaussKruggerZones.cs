using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GaussKruggerZones : MonoBehaviour
{
    [SerializeField] private List<TextAsset> zones = new List<TextAsset>();
    public static List<TextAsset> Zones = new List<TextAsset>();


    private void Awake()
    {
        Zones.AddRange(zones);
    }
    public static string GetZoneData(string name)
    {
        int central_meridian = int.Parse(name) * 6 - 3;
        int false_easting = 1000000 * int.Parse(name) + 500000;
        int EPSG = 28400 + int.Parse(name);
        string data = $@"PROJCS[""Pulkovo 1942 / Gauss - Kruger zone 23"",
    GEOGCS[""Pulkovo 1942"",
        DATUM[""Pulkovo_1942"",
            SPHEROID[""Krassowsky 1940"", 6378245, 298.3],
            TOWGS84[23.57, -140.95, -79.8, 0, 0.35, 0.79, -0.22]],
        PRIMEM[""Greenwich"", 0,
            AUTHORITY[""EPSG"", ""8901""]],
        UNIT[""degree"", 0.0174532925199433,
            AUTHORITY[""EPSG"", ""9122""]],
        AUTHORITY[""EPSG"", ""4284""]],
    PROJECTION[""Transverse_Mercator""],
    PARAMETER[""latitude_of_origin"", 0],
    PARAMETER[""central_meridian"", {central_meridian}],
    PARAMETER[""scale_factor"", 1],
    PARAMETER[""false_easting"", {false_easting}],
    PARAMETER[""false_northing"", 0],
    UNIT[""metre"", 1,
        AUTHORITY[""EPSG"", ""9001""]],
    AUTHORITY[""EPSG"", ""{EPSG}""]]";


        //return Zones.Find(x => x.name == name).text;
        return data;
    }
}
