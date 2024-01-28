using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.Windows.Speech;


public class Player : MonoBehaviour
{
    private CharacterController characterController;
    private bool ShouldCrouch => Input.GetKeyDown(crouchKey) && !duringCrouchAnimation && characterController.isGrounded;
    public bool ShouldTickle => Input.GetMouseButton(0) && closeToEnemy == true;

    [Header("Functional Options")]
    [SerializeField] private bool canCrouch = true;
    [SerializeField] private bool tickleMode = false;

    [Header("Controls")]
    [SerializeField] private KeyCode crouchKey = KeyCode.LeftControl;
    /// <summary>
    /// [SerializeField] private MouseButton tickleButton = MouseButton.Left;
    /// </summary>

    [Header("Crouch Parameters")]
    [SerializeField] private float crouchHeight = 0.5f;
    [SerializeField] private float StandingHeight = 2f;
    [SerializeField] private float timeToCrouch = 0.25f;
    [SerializeField] private Vector3 crouchingCenter = new Vector3(0, 0.5f, 0);
    [SerializeField] private Vector3 standingCenter = new Vector3(0, 0, 0);

    [Header("Tickle Parameters")]
    private Vector2 tickleVector;
    private int tickleMeter;
    bool enemyTickled = false;
    //previous Vector
    //Current Vector


    public bool isCrouching;
    public bool isTickling;
    private bool duringCrouchAnimation;
    public bool closeToEnemy = false;


    Vector2 twoFrameOldPoint = Vector2.zero;
    Vector2 oneFrameOldPoint = Vector2.zero;
    Vector2 currentFramePoint = Vector2.zero;
    Vector2 mouseMovementOld = Vector2.zero;
    Vector2 mouseMovementNew = Vector2.zero;
    int count = 0;

    [SerializeField] public UIScript laughBar;
    public int maxLaughter = 10;
    public int currentLaughter;

    Vector3 playerPosition;
    public Camera fpsCam;
    float handTickleRange = 3.0f;

    [SerializeField] EnergyUIScript energyUIScript;

    RaycastHit hitInfo;

    [SerializeField] GameObject feather;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }


    // Start is called before the first frame update
    void Start()
    {
        feather.SetActive(false);
        currentLaughter = 0;
        laughBar.SetMaxLaughter(maxLaughter);
    }


    void Update()
    {
        playerPosition = gameObject.transform.position;
        if (canCrouch == true)
        {
            HandleCrouch();
        }
        if (ShouldTickle)
        {
            Tickle();
        }
        if (!Input.GetMouseButton(0))
        {
            feather.SetActive(false);
        }
        //rayCast();
    }


    private void HandleCrouch()
    {
        if (ShouldCrouch)
        {
            StartCoroutine(CrouchStand());
        }

    }
    private IEnumerator CrouchStand()
    {
        duringCrouchAnimation = true;

        float timeElapsed = 0;
        float targetHeight = isCrouching ? StandingHeight : crouchHeight;
        float currentHeight = characterController.height;
        Vector3 targetCenter = isCrouching ? standingCenter : crouchingCenter;
        Vector3 currentCenter = characterController.center;

        while (timeElapsed < timeToCrouch)
        {
            characterController.height = Mathf.Lerp(currentHeight, targetHeight, timeElapsed / timeToCrouch);
            characterController.center = Vector3.Lerp(currentCenter, targetCenter, timeElapsed / timeToCrouch);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        //sanity check 
        characterController.height = targetHeight;
        characterController.center = targetCenter;

        isCrouching = !isCrouching;

        duringCrouchAnimation = false;
    }

    /*
    private void HandleTickle()
    {
       if (ShouldTickle)
        {
            Tickle();
        }
    }*/

    private void Tickle()
    {
        feather.SetActive(true);

        if (count == 0)
        {
            currentFramePoint = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            // Debug.Log("Count: " + count.ToString() + "\n" + "Current Frame Point: " + currentFramePoint.ToString() + "\n" + "One Frame Behind: " + oneFrameOldPoint.ToString() + "\n" + "Two Frames Behind: " + twoFrameOldPoint.ToString() + "\n");
        }
        else if (count == 10)
        {
            oneFrameOldPoint = currentFramePoint;
            currentFramePoint = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            //Debug.Log("Count: " + count.ToString() + "\n" + "Current Frame Point: " + currentFramePoint.ToString() + "\n" + "One Frame Behind: " + oneFrameOldPoint.ToString() + "\n" + "Two Frames Behind: " + twoFrameOldPoint.ToString() + "\n");
        }
        else if (count == 20)
        {
            twoFrameOldPoint = oneFrameOldPoint;
            oneFrameOldPoint = currentFramePoint;
            currentFramePoint = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            //Debug.Log("Count: " + count.ToString() + "\n" + "Current Frame Point: " + currentFramePoint.ToString() + "\n" + "One Frame Behind: " + oneFrameOldPoint.ToString() + "\n" + "Two Frames Behind: " + twoFrameOldPoint.ToString() + "\n");
            mouseMovementOld = oneFrameOldPoint - twoFrameOldPoint;
            mouseMovementNew = currentFramePoint - oneFrameOldPoint;
            float movementDot = Vector2.Dot(mouseMovementOld, mouseMovementNew);
            //Debug.Log("\nDot Product: " + movementDot.ToString());
            if (movementDot < 0)
            {
                currentLaughter += 1;
                laughBar.SetLaughter(currentLaughter);
                energyUIScript.AddEnergy(5);
                //Debug.Log("\nAdded to Tickle Meter");
            }
            else if (movementDot >= 0)
            {
                //Debug.Log("\nNO ADDITION");
            }
        }
        if (count >= 30)
        {
            count = 20;
        }
        else
        {
            count++;
        }
    }

    private void rayCast()
    {
        Debug.DrawLine(fpsCam.transform.position, fpsCam.transform.forward * handTickleRange);
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hitInfo, handTickleRange))
        {
            closeToEnemy = true;
            if (hitInfo.collider != null && ShouldTickle)
            {
                hitInfo.collider.gameObject.GetComponent<NPC_Movement>().SetTickleMode();
            }
        }
        else
        {
            closeToEnemy = false;
            if (hitInfo.collider != null)
            {
                hitInfo.collider.gameObject.GetComponent<NPC_Movement>().SetChaseMode();
            }
        }
    }

}
