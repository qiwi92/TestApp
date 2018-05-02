using System;
using System.Collections.Generic;
using UnityEngine;

public class HistogramView : MonoBehaviour
{
    [SerializeField] private BarView _barViewPrefab;
    private BarView[] _bars;

    public void Setup(int numberOfBars, float minValue, float maxValue)
    {
        _bars = new BarView[numberOfBars];

        for (var i = 0; i < numberOfBars; i++)
        {
            var newBar = Instantiate(_barViewPrefab, transform);
            newBar.Setup(minValue,maxValue);
            _bars[i] = newBar;
        }
    }

    public void SetValues(IReadOnlyList<float> values)
    {
        if (values.Count != _bars.Length)
        {
            throw new Exception("Values exced bar number");
        }

        for (var i = 0; i < values.Count; i++)
        {
            _bars[i].SetValue(values[i]);
        }
    }
}