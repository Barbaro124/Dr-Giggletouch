using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feather : MonoBehaviour
{
    bool inTickleMode;
    float maxDist;
    float currTime;
    float currTimeMultiplier;
    Vector3 originalOffset;
    [SerializeField] GameObject featherHolder;

    // Start is called before the first frame update
    void Start()
    {
        inTickleMode = false;
        maxDist = .25f;
        originalOffset = transform.parent.position - transform.position;
        currTime = 0;
        currTimeMultiplier = 3f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            originalOffset = transform.parent.position - transform.position;
            inTickleMode = true;
            currTime = 0;
            if (currTimeMultiplier < 0)
            {
                currTimeMultiplier *= -1;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            inTickleMode = false;
            transform.position = transform.parent.position - originalOffset;
        }

        if (inTickleMode)
        {
            currTime += Time.deltaTime * currTimeMultiplier;
            if (currTime >= 1.0 || currTime <= 0)
            {
                currTimeMultiplier *= -1;
            }

            transform.Translate(Time.deltaTime * Vector3.up * maxDist * currTimeMultiplier, Space.World);
        }
    }
}
