using ProjNet.CoordinateSystems;
using System.Collections;
using UnityEngine;

namespace EidosDev
{
  public class CoordinatesPanel : MonoBehaviour
  {
    [SerializeField] protected CoordinatesInput NX_input;
    [SerializeField] protected CoordinatesInput EY_input;

    private InputState _inputState = InputState.Point;
    
    public GeoVector2 Coordinates => GetWGSPositionFromInputs();
    public SKVector2 CoordinatesSK => GetSKPositionFromInputs();

    protected virtual void Start()
    {

      NX_input.OnSKChanged += SKChanged;
      EY_input.OnSKChanged += SKChanged;
      NX_input.OnWGSChanged += WGSChanged;
      EY_input.OnWGSChanged += WGSChanged;

      UpdatePosition(Coordinates);
    }

    protected virtual void OnDestroy()
    {
        NX_input.OnSKChanged -= SKChanged;
        EY_input.OnSKChanged -= SKChanged;
        NX_input.OnWGSChanged -= WGSChanged;
        EY_input.OnWGSChanged -= WGSChanged;
    }


        #region Other

    private GeoVector2 GetWGSPositionFromInputs()
    {
      GeoVector2 position = GeoVector2.zero;

      if (EY_input.GetWgsValue(out double lon))
        position.lon = lon;
      if (NX_input.GetWgsValue(out double lat))
        position.lat = lat;

      return position;
    }

    private SKVector2 GetSKPositionFromInputs()
    {
            SKVector2 position = SKVector2.zero;

        if (NX_input.GetSkValue(out double x))
            position.x = x;
        if (EY_input.GetSkValue(out double y))
            position.y = y;

        return position;
    }

    #endregion


    #region UIInput

    //UI Input

    public void Btn_ChangeCoordinatesType()
    {
        NX_input.ChangeCoordinateInput();
        EY_input.ChangeCoordinateInput();

        UpdatePosition(Coordinates);
    }

    public void WGSChanged(double doubleValue, CoordinatesInputType type)
    {
        if (_inputState == InputState.Point)
            _inputState = InputState.Field;

        //_selected.GetPosition(out double selectedposx, out double selectedposy);

        if (type == CoordinatesInputType.N)
        {
            //_selected.SetPosition(selectedposx, doubleValue);
            UpdatePosition(new GeoVector2(Coordinates.lon, doubleValue));
            return;
        }

        if(type == CoordinatesInputType.E)
        {
            //_selected.SetPosition(doubleValue, selectedposy);
            UpdatePosition(new GeoVector2(doubleValue, Coordinates.lat));
        }
    }
    public void SKChanged(double doubleValue, CoordinatesInputType type)
    {
        if (_inputState == InputState.Point)
            _inputState = InputState.Field;

        SKVector2 data = CoordinatesSK;

        if (data.x.ToString().Length < 7|| data.y.ToString().Length < 7)
            return;


        double[] output = new double[2];
        if (type == CoordinatesInputType.X)
        {
             output = CoordinateConversion.ConvertSkToWGS(doubleValue, data.y);
        }

        if (type == CoordinatesInputType.Y)
        {
             output = CoordinateConversion.ConvertSkToWGS(data.x, doubleValue);
        }

        //_selected?.SetPosition(output[0], output[1]);
        UpdatePosition(new GeoVector2(output[0], output[1]));
    }



        #endregion


    #region UpdatePosition

    public virtual void UpdatePosition(GeoVector2 position)
    {

        bool isGaussZone = true ;
        int zone = CoordinateConversion.DetermineZoneNumber(position.lon, position.lat);

        SKVector2 output = CoordinateConversion.ConvertWGSToSk<ProjectedCoordinateSystem>(position.lon, position.lat, GaussKruggerZones.GetZoneData(zone.ToString()));

        NX_input.UpdateValues(position, output, isGaussZone);
        EY_input.UpdateValues(position, output, isGaussZone);

    }

    public void UpdatePosition()
    {
        GeoVector2 pos = GeoVector2.zero;
        //OnlineMapsControlBase.instance.GetCoords(out pos.lon, out pos.lat);

        UpdatePosition(pos);
    }

    #endregion

    #region OnActivitis

  /*  protected void OnPositionChanged(OnlineMapsMarkerBase obj)
    {
      if (_inputState == InputState.Field)
      {
        _inputState = InputState.Point;
        return;
      }
            GeoVector2 pos;
            _selected.GetPosition(out pos.lon, out pos.lat);
      UpdatePosition(pos);
      OnlineMapsControlBase.instance.map.Redraw();
    }
  */
    #endregion
  }
}