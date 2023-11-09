using System;
using UnityEngine;

namespace EidosDev
{
  public struct GeoPosition
  {
    private double _longitude;
    private double _latitude;
    private float _height;

    public GeoPosition(GeoVector2 position, float height)
    {
      _longitude = position.lon;
      _latitude = position.lat;
      _height = height;
    }

    public GeoVector2 Position => new GeoVector2(_longitude, _latitude);
    public float Height => _height;
  }
    [Serializable]
    public struct GeoVector2
    {
       public GeoVector2(double lon, double lat)
        {
            this.lon = lon;
            this.lat = lat;
        } 

       public static GeoVector2 zero { get { return new GeoVector2(0, 0); } }
       public static GeoVector2 one { get { return new GeoVector2(1, 1); } }
       public double lon;
       public double lat;
    }
    [Serializable]
    public struct SKVector2
    {
        public SKVector2(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public static SKVector2 zero { get { return new SKVector2(0, 0); } }
        public static SKVector2 one { get { return new SKVector2(1, 1); } }
        public double x;
        public double y;
    }
}