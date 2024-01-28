using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class Chicken : MonoBehaviour
{
    [SerializeField] GunSystem gunSystem;
    public Vector3 forwardFaceing;
    Rigidbody rb;
    public Vector3 direction;
    float speed = 10.0f;

    int addedLaugh = 10;

     UIScript laughBar;
     EnergyUIScript energyBar;
    AudioManager audioManager;


    private void Update()
    {
        forwardFaceing = direction;
    }



    // Start is called before the first frame update


    void Start()
    {
        //currentLaughter = GameObject.Find("PlayerCapsule").GetComponent<Player>().currentLaughter;
        rb = GetComponent<Rigidbody>();
        rb.AddForce(direction * speed, ForceMode.Impulse);
        Debug.Log(direction);
        laughBar = FindObjectOfType<UIScript>();
        energyBar = FindAnyObjectByType<EnergyUIScript>();
        audioManager = FindAnyObjectByType<AudioManager>();
    }

    private void Tickle()

    {
        laughBar.SetLaughter(addedLaugh);
        energyBar.AddEnergy(30f);
        audioManager.Play("Laugh1");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "NPC")
        {
            Tickle();
            Debug.Log("rocket tickle");
        }
    }

}
