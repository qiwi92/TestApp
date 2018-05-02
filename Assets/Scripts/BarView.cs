using UnityEngine;
using UnityEngine.UI;

public class BarView : MonoBehaviour
{
    [SerializeField] private Image _barImage;
    private float _max;
    private float _min;

    public void Setup(float min, float max)
    {
        _min = min;
        _max = max;
    }

    public void SetValue(float value)
    {
        _barImage.fillAmount = (value - _min) /(_max - _min);
    }
}