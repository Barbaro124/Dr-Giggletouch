using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{

    public Slider slider;


    public void SetMaxLaughter(int laughter)
    {
        slider.maxValue = laughter;
    }
    public void SetLaughter(int laughter)
    {
        slider.value = laughter; 
    }
}
