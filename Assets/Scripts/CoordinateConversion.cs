using EidosDev;
using System;
using ProjNet.CoordinateSystems;
using ProjNet.CoordinateSystems.Transformations;
using ProjNet.IO.CoordinateSystems;
using UnityEngine;

public static class CoordinateConversion 
{

    static string skDegree = @"GEOGCS[""Pulkovo 1942"",
DATUM[""Pulkovo_1942"",
    SPHEROID[""Krassowsky 1940"",6378245,298.3,AUTHORITY[""EPSG"",""7024""]],
    TOWGS84[23.57,-140.95,-79.8,0,0.35,0.79,-0.22],
    AUTHORITY[""EPSG"",""6284""]],
PRIMEM[""Greenwich"",0,
    AUTHORITY[""EPSG"",""8901""]],
UNIT[""degree"",0.017453292519943278,
    AUTHORITY[""EPSG"",""9102""]],
AUTHORITY[""EPSG"",""4284""]]
";

    static string wgsDegree = @"GEOGCS[""WGS 84"",
    DATUM[""WGS_1984"",
        SPHEROID[""WGS 84"",6378137,298.257223563,
            AUTHORITY[""EPSG"",""7030""]],
        AUTHORITY[""EPSG"",""6326""]],
    PRIMEM[""Greenwich"",0,
        AUTHORITY[""EPSG"",""8901""]],
    UNIT[""degree"",0.0174532925199433,
        AUTHORITY[""EPSG"",""9122""]],
    AUTHORITY[""EPSG"",""4326""]]
";

    static double DegreesToRadians(double degrees)
    {
        return degrees * Math.PI / 180.0;
    }

    static double RadiansToDegrees(double radians)
    {
        return radians * 180.0 / Math.PI;
    }

    public static int DetermineZoneNumber(double WgsLon, double WgsLat)
    {
        SKVector2 skDegrees = ConvertWGSToSk<GeographicCoordinateSystem>(WgsLon, WgsLat, skDegree);

        //Debug.Log($"{skDegrees.x}||{skDegrees.y}");
        double ydegree = skDegrees.y;
        if (ydegree < 0)
        {
            ydegree = 180 + Math.Abs(ydegree);
        }
        int zoneNumber = (int)ydegree / 6 + 1;

       // Debug.Log("ZoneNumber: " + zoneNumber);

        return zoneNumber;
    }

    public static SKVector2 ConvertWGSToSk<T>(double lon, double lat, string sk) where T:CoordinateSystem
    {
        // Создаем объект для преобразования координат
        CoordinateTransformationFactory ctfac = new ();

        // Определяем систему координат WGS84 и СК-42 Gauss-Kruger
        // GeographicCoordinateSystem wgs84 = GeographicCoordinateSystem.WGS84;
        var wgs84 = CoordinateSystemWktReader.Parse(wgsDegree) as GeographicCoordinateSystem;

        var sk42 = CoordinateSystemWktReader.Parse(sk) as T;

        // Получаем объект преобразования координат
        ICoordinateTransformation transform = ctfac.CreateFromCoordinateSystems(wgs84, sk42);


        // Создаем объект исходных координат
        double[] input = new double[] { lon, lat };

        // Преобразуем координаты из WGS84 в СК-42 Gauss-Kruger
        var _output = transform.MathTransform.Transform(input[0], input[1]);

        //x и y перепутаны, поэтому восстанавливаем как надо 
        SKVector2 output = SKVector2.zero;
        output.x = _output.y;
        output.y = _output.x;

        return output;
    }

    public static double[] ConvertSkToWGS(double x, double y)
    {
        int intZone = (int)y;
        string zone = intZone.ToString();
        zone = zone.Remove(zone.Length - 6);
        string skZone = GaussKruggerZones.GetZoneData(zone);

        // Создаем объект для преобразования координат
        CoordinateTransformationFactory ctfac = new CoordinateTransformationFactory();

        // Определяем систему координат WGS84 и СК-42 Gauss-Kruger
        // GeographicCoordinateSystem wgs84 = GeographicCoordinateSystem.WGS84;
        GeographicCoordinateSystem wgs84 = CoordinateSystemWktReader.Parse(wgsDegree) as GeographicCoordinateSystem;

        ProjectedCoordinateSystem sk42 = CoordinateSystemWktReader.Parse(skZone) as ProjectedCoordinateSystem;

        // Получаем объект преобразования координат
        ICoordinateTransformation transform = ctfac.CreateFromCoordinateSystems(sk42, wgs84);


        // Создаем объект исходных координат
        double[] input = new double[] { y, x };

        // Преобразуем координаты из СК-42 Gauss-Kruger в WGS84 
        var _output = transform.MathTransform.Transform(input[0], input[1]);

        // Выводим преобразованные координаты
        double lon = _output.x; // Координата x
        double lat = _output.y; // Координата y
        //Debug.Log("Lon:" + lon + " Lat:" + lat);

        double[] output = new double[] { _output.x, _output.y };
        return output;
    }



    /*public static int GetZone(double longitude)
    {
        return (int)longitude[0] / 6 + 1;
    }*/

}
