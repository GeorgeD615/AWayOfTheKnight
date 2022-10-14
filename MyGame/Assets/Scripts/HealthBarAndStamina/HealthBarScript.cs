using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour
{

    public Slider _slider;

    public void SetMaxHelth(int helth)
    {
        _slider.maxValue = helth;
        _slider.value = helth;

    }
    public void SetHelth(int helth)
    {
        _slider.value = helth;
    }
}
