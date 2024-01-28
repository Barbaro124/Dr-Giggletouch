using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class NPC_Movement : MonoBehaviour
{
    public enum NPC_State { WANDER, CHASE, IDLE, ATTACK, TICKLE };

    [SerializeField] float walkspeed = 3.5f;
    [SerializeField] float runspeed = 3.5f * 2.0f;
    [SerializeField] float sphereRadius = 5.0f;
    Animator animator;
    float speed;
    Vector3 destination;
    NavMeshAgent agent;
    NPC_State currState;
    Player player;
    float waitTime;
    int layer = 6;

    AudioManager audioManager;
    float health;

    string[] LaughSounds = new string[] { "Laugh1", "Laugh2", "Laugh3", "Laugh4", "Laugh5", "Laugh6", "Laugh7", "Laugh8", "Laugh9" };


    // Start is called before the first frame update
    void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = FindFirstObjectByType<Player>();
        gameObject.transform.rotation = Quaternion.LookRotation(new Vector3(UnityEngine.Random.Range(-1f, 1f), 0, 
                                                                            UnityEngine.Random.Range(-1f, 1f)), 
                                                                gameObject.transform.up);
        destination = gameObject.transform.position + gameObject.transform.forward;
        currState = NPC_State.WANDER;
        waitTime = 0f;
        animator.SetBool("Attack", false);
        health = 100f;

        audioManager = FindObjectOfType<AudioManager>();

    }


    public void SetTickleMode()
    {
        currState = NPC_State.TICKLE;
        animator.SetBool("Tickle", true);
        speed = 0;
        //Play Laughter Sound
        audioManager.Play(LaughSounds[Random.Range(0, LaughSounds.Length)]);

    }

    public void EndTickleMode()
    {
        currState = NPC_State.IDLE;
        animator.SetBool("Tickle", false);
        speed = 0;
    }

    public void SetChaseMode()
    {
        currState = NPC_State.CHASE;
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, sphereRadius, (1 << layer));
        Transform hand = transform.Find("Character_Priest_Male_01/Root/Hips/Spine_01/Spine_02/Spine_03/Clavicle_R/Shoulder_R/Elbow_R/Hand_R");
        Debug.Log(hand);

        if (hitColliders.Length > 0)
        {
            hitColliders[0].gameObject.transform.position = hand.position;
            hitColliders[0].gameObject.transform.SetParent(hand);
        }
    }

    public void SetAttackMode()
    {
        animator.SetBool("Attack", true);
        currState = NPC_State.ATTACK;
        agent.speed = 0f;
    }

    public void EndAttackMode()
    {
        Debug.Log("EndAttack");
        animator.SetBool("Attack", false);
        currState = NPC_State.WANDER;
        Transform hand = transform.Find("Character_Priest_Male_01/Root/Hips/Spine_01/Spine_02/Spine_03/Clavicle_R/Shoulder_R/Elbow_R/Hand_R");
        if (hand.childCount != 0)
        {
            //hand.DetachChildren();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (health < 0)
        {
            Destroy(gameObject);
        }
        if (Input.GetKey(KeyCode.O))
        {
            SetAttackMode();
        }
        if (Input.GetKey(KeyCode.P))
        {
            SetChaseMode();
        }
        
        waitTime -= Time.deltaTime;
        if (currState == NPC_State.ATTACK )
        {
            Debug.Log(Vector3.Distance(player.gameObject.transform.position, transform.position));
            if (Vector3.Distance(player.gameObject.transform.position, transform.position) > 1f)
            {
                SetChaseMode();
            }
        }
        else if (currState == NPC_State.TICKLE)
        {
            health -= Time.deltaTime * 10;
        }
        else if (currState == NPC_State.CHASE)
        {
            Vector3 dir2Player = (player.gameObject.transform.position - transform.position).normalized * 2f;
            agent.destination = player.gameObject.transform.position + dir2Player;
            speed = runspeed;
            animator.SetFloat("Speed", speed);
            agent.speed = speed;

            // If we're "too far" from the player, forget about them
            Debug.Log(Vector3.Distance(player.gameObject.transform.position, transform.position));
            if (Vector3.Distance(player.gameObject.transform.position, transform.position) > 5f)
            {
                currState = NPC_State.WANDER;
                Transform hand = transform.Find("Character_Priest_Male_01/Root/Hips/Spine_01/Spine_02/Spine_03/Clavicle_R/Shoulder_R/Elbow_R/Hand_R");
                if (hand.childCount != 0)
                {
                    //hand.DetachChildren();
                }
            }
            else if (Vector3.Distance(player.gameObject.transform.position, transform.position) < 1f)
            {
                SetAttackMode();
            }
        }
        else
        {
            // If we've reached our wait time OR arrived at our destination, select another
            //Debug.Log(Vector3.Distance(gameObject.transform.position, agent.destination));
            if (waitTime < 0.0f || Vector3.Distance(gameObject.transform.position, agent.destination) < .5f)
            {
                float choice = UnityEngine.Random.Range(0f, 10f);

                // Choose to wander randomly
                if (choice < 9.9f)
                {
                    // Choose a random direction to move
                    currState = NPC_State.WANDER;
                    Vector3 targetDir = gameObject.transform.forward + gameObject.transform.right * UnityEngine.Random.Range(-1.0f, 1.0f);
                    agent.destination = gameObject.transform.position + targetDir;
                    speed = walkspeed;
                    agent.speed = speed;
                    animator.SetFloat("Speed", speed);
                }
                // Choose to be idle
                else
                {
                    currState = NPC_State.IDLE;
                    agent.destination = player.gameObject.transform.position;
                    speed = 0f;
                    agent.speed = speed;
                    animator.SetFloat("Speed", speed);
                }
                waitTime = UnityEngine.Random.Range(1.0f, 5.0f);
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Feather") && currState != NPC_State.CHASE)
        {
            bool isDead = player.AddEnergy();
            SetTickleMode();
            if (isDead)
            {
                player.ResetLaughBar();
                Destroy(gameObject);
            }
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Feather") && currState == NPC_State.TICKLE)
        {
            bool isDead = player.AddEnergy();
            if (isDead)
            {
                player.ResetLaughBar();
                Destroy(gameObject);
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Feather"))
        {
            EndTickleMode();
            SetChaseMode();
        }
    }
}
