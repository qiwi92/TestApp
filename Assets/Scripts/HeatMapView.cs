using System;
using System.Collections.Generic;
using UnityEngine;

public class HeatMapView : MonoBehaviour
{
    private Texture2D _texture;
    private float _min;
    private float _max;

    public void Setup (float minValue, float maxValue)
    {
        _texture = new Texture2D(SensorData.Width, SensorData.Height);
        GetComponent<Renderer>().material.mainTexture = _texture;
        _texture.filterMode = FilterMode.Point;

        _min = minValue;
        _max = maxValue;
    }

    public void SetValues(IReadOnlyList<float> values)
    {
        for (int y = 0; y < _texture.height; y++)
        {
            for (int x = 0; x < _texture.width; x++)
            {
                var currentValue = values[SensorData.Height * x + y];
                var red = (currentValue - _min) / (_max - _min);
                var blue = 1 - (currentValue - _min) / (_max - _min);
                var newColor = new Color(red,blue,0);
                _texture.SetPixel(x, y, newColor);
            }
        }
        _texture.Apply();


    }
}