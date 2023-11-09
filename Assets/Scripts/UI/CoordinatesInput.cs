using System;
using UnityEngine;
using EidosDev;
using System.Globalization;

public class CoordinatesInput : MonoBehaviour
{
    [SerializeField] CoordinatesInputType WGS_InputType;
    [SerializeField] CoordinatesInputType SK_InputType;

    [SerializeField] protected UIInput SK_input;
    [SerializeField] protected UIInput WGS_input;
    [SerializeField] protected UILabel additionalLabel;

    [SerializeField] private GameObject WGS;
    [SerializeField] private GameObject SK;


    public Action<double, CoordinatesInputType> OnWGSChanged;
    public Action<double, CoordinatesInputType> OnSKChanged;

    public bool GetWgsValue(out double value)
    {
        if (Helper.TryParse(WGS_input.value, out double doubleValue))
        {
            value = doubleValue;
            return true;
        }
        else
        {
            value = 0;
            return false;
        }
    }

    public bool GetSkValue(out double value)
    {
        if (Helper.TryParse(SK_input.value, out double doubleValue))
        {
            value = doubleValue;
            return true;
        }
        else
        {
            value = 0;
            return false;
        }
    }

    public void ChangeCoordinateInput()
    {
        WGS.SetActive(!WGS.activeInHierarchy);
        SK.SetActive(!SK.activeInHierarchy);
    }

    public void UpdateValues(GeoVector2 wgs, SKVector2 sk, bool isGaussZone)
    {
        string wgs_value = UpdateWGS(wgs);

        string sk_value = "";
        if (isGaussZone)
        {
            sk_value = UpdateSK(sk);
        }
        else
        {
            sk_value = SK_input.value;
        }


        if (SK.activeInHierarchy)
        {
            if (SK_InputType == CoordinatesInputType.X)
                additionalLabel.text = "N: " + wgs_value;
            else
                additionalLabel.text = "E: " + wgs_value;
        }

        if (WGS.activeInHierarchy)
        {
            if (WGS_InputType == CoordinatesInputType.N)
                additionalLabel.text = "X: " + sk_value;
            else
                additionalLabel.text = "Y: " + sk_value;
        }
    }

    public void InputField_WgsChanged(string value)
    {
        if (!WGS.activeInHierarchy)
            return;

        if (CheckInput(value, out double wgs_value))
            return;

        OnWGSChanged?.Invoke(wgs_value, WGS_InputType);
    }

    public void InputField_SkChanged(string value)
    {
        if (!SK.activeInHierarchy)
            return;

        if (value.Length < 7)
            return;

        if (!Helper.TryParse(value, out double sk_value))
            return;

        OnSKChanged?.Invoke(sk_value, SK_InputType);
    }

    private string UpdateWGS(GeoVector2 wgs)
    {
        string wgs_value = "";
        if (WGS_InputType == CoordinatesInputType.E)
        {
            wgs_value = wgs.lon.ToString(CultureInfo.InvariantCulture).Replace(',', '.');

        }
        if (WGS_InputType == CoordinatesInputType.N)
        {
            wgs_value = wgs.lat.ToString(CultureInfo.InvariantCulture).Replace(',', '.');
        }

        if (wgs_value.Length > 10)
            wgs_value = wgs_value.Remove(10);
        WGS_input.value = wgs_value;
        return wgs_value;
    }
    private string UpdateSK(SKVector2 sk)
    {
        string sk_value = "";
        if (SK_InputType == CoordinatesInputType.X)
        {
            sk_value = Math.Round(sk.x).ToString(CultureInfo.InvariantCulture).Replace(',', '.');
        }
        if (SK_InputType == CoordinatesInputType.Y)
        {
            sk_value = Math.Round(sk.y).ToString(CultureInfo.InvariantCulture).Replace(',', '.');
        }
        string[] sk_splited = sk_value.Split(".");
        sk_value = sk_splited[0];
        SK_input.value = sk_value;
        return sk_value;
    }

    // Start is called before the first frame update
    void Start()
    {
        SK_input.submitOnUnselect = true;
        WGS_input.submitOnUnselect = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private bool CheckInput(string value, out double doubleValue)
    {
        doubleValue = -1;

        if (value.Length >= 1)
        {

            char lastChar = value[value.Length - 1];

            if (lastChar.Equals('0') || lastChar.Equals(',') || lastChar.Equals('.'))
                return true;

            if (!Helper.TryParse(value, out doubleValue))
                return true;
        }

        return false;
    }
}

public enum CoordinatesInputType
{
    X,
    Y,
    N,
    E
}
