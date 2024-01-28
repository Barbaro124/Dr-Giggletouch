using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{

    public float timePassed;
    public bool TimerOn = false;

    public Text TimerTxt;
    public static Timer instance;

    // Start is called before the first frame update

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        TimerOn = true;
    }

    // Update is called once per frame
    void Update()
    {

        if (TimerOn)
        {
            timePassed += Time.deltaTime;
            updateTimer(timePassed);
        }
        
    }

    public void StopTimer()
    {
        TimerOn = false;
    }

    void updateTimer(float currentTime)
    {
        currentTime += 1;

        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);

        TimerTxt.text = string.Format("{0:00} : {1:00}", minutes, seconds);
    }
}
