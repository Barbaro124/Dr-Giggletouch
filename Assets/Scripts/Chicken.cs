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
    private void Update()
    {
        forwardFaceing = direction;
    }



    // Start is called before the first frame update


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(direction * speed,ForceMode.Impulse);
        Debug.Log (direction);
    }

    
}
