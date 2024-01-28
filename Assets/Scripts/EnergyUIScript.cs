using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class EnergyUIScript : MonoBehaviour
{

    public Slider slider;
    [SerializeField] Timer timer;
    [SerializeField] public float multiplier = 1;
    [SerializeField] public float multiplierIncrement;
    [SerializeField] public float secondsBeforeMultiplierIncreases = 5;
    float seconds;

    public void SetMaxEnergy(float energy)
    {
        slider.maxValue = energy;
    }
    public void SetEnergy(float energy)
    {
        slider.value = energy;
    }

    public void AddEnergy(float energy)
    {
        slider.value += energy;
    }

    public void DecreaseEnergy(float energy)
    {
        slider.value -= energy;
    }
    public void Update() 
    {
        SetEnergy(slider.value - multiplier * Time.deltaTime);

        if (slider.value <= 0)
        {
            slider.value = 0;
            timer.StopTimer();
            SceneManager.LoadScene("GameOver");
        }
    }

    public void SetMultiplier(float newMultiplier)
    {
        multiplier = newMultiplier;
    }
}
