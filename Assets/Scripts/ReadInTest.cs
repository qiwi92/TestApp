using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ReadInTest : MonoBehaviour
{
    [SerializeField] private HistogramView _histogram;
    [SerializeField] private HeatMapView _heatmap;

    private IReadOnlyList<float> _cache;
    private readonly object _syncRoot = new object();

    private void Start()
    {
        _histogram.Setup(65,25,40);
        _heatmap.Setup(25,40);

        DataReadIn.DataReceived += WriteToCache;
        DataReadIn.StartReadIn();
    }

    private void WriteToCache(IReadOnlyList<float> data)
    {
        lock (_syncRoot)
        {
            _cache = data;
        }
    }

    private void OnDestroy()
    {
        DataReadIn.StopReadIn();
        DataReadIn.DataReceived -= WriteToCache;
    }

    private void Update()
    {
        lock (_syncRoot)
        {
            if (_cache != null)
            {
                _histogram.SetValues(_cache);
                _heatmap.SetValues(_cache);
                _cache = null;
            }
        }
    }
}