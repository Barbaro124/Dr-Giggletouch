using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC_Movement : MonoBehaviour
{
    [SerializeField] float walkspeed = 3.5f;
    [SerializeField] float runspeed = 3.5f * 2.0f;
    Animator animator;
    float speed;
    Vector3 destination;
    NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        speed = 0.0f;
        animator = GetComponent<Animator>();
        animator.SetFloat("Speed", speed);
        StartCoroutine(ChooseBehavior());
        //destination = gameObject.transform.position + gameObject.transform.forward;
        destination = FindFirstObjectByType<Player>().transform.position;
        agent.destination = destination;
        Debug.Log("destination" + destination);
    }

    // Update is called once per frame
    void Update()
    {
        destination = FindFirstObjectByType<Player>().transform.position;
        agent.destination = destination;
        agent.speed = speed;
        animator.SetFloat("Speed", speed);
    }

    // Unity coroutine that randomly changes behavior every 5 seconds
    IEnumerator ChooseBehavior()
    {
        float waitTime = 0.0f;
        while (true)
        {
            float randSpeed = UnityEngine.Random.Range(0f, 3.0f);
            if (randSpeed < 1.0f)
                speed = 0.0f;
            else if (randSpeed < 2.0f)
                speed = walkspeed;
            else
                speed = runspeed;

            waitTime = UnityEngine.Random.Range(1.0f, 5.0f);
            animator.SetFloat("Speed", speed);
            yield return new WaitForSeconds(waitTime);
        }
    }
}
