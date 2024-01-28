using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Control : MonoBehaviour
{
    public void ResetTheGame()
    {
        SceneManager.LoadScene("Town");
        //print("The button is working");
    }

    public void QuitTheGame()
    {
        Application.Quit();
    }

}
