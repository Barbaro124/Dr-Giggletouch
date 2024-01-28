using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GunSystem : MonoBehaviour
{
    [SerializeField] public GameObject firePoint;
    [SerializeField] public GameObject chickenPrefab;
    public GameObject chicken;
    public int damage;
    public float timeBetweenShooting, spread, range, reloadTime, timeBetweenShots;
    public int magazineSize, BulletsPerTap;
    public bool allowButtonHold;
    int bulletsLeft, BulletsShot;
    public Vector3 forward;

    //bools
    bool shooting, readytToShoot, reloading;

    //Reference
    public Camera fpsCam;
    public Transform attackPoint;
    public RaycastHit rayHit;
    public LayerMask whatIsEnemy;
    AudioManager audioManager;


    private void Awake()
    {
        bulletsLeft = magazineSize;
        readytToShoot = true;
        audioManager = FindAnyObjectByType<AudioManager>();
    }


    private void Update()
    {
        MyInput();
        forward = Vector3.forward;

        
    }


    private void MyInput()

    {
        if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse2);
        else shooting = Input.GetKeyDown(KeyCode.Mouse2);

        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading) Reload();

        //shoot
        if (readytToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            BulletsShot = BulletsPerTap;
            Shoot();
        }
    }


    private void Shoot()
    {

        readytToShoot = false;
      audioManager.Play("Chicken Bawk");
        //Spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        //Calculate Direction with Spread
        Vector3 direction = transform.forward;


        //Raycast
        if (Physics.Raycast(fpsCam.transform.position, direction, out rayHit, range, whatIsEnemy))
        {
            Debug.Log(rayHit.collider.name);
            

            if (rayHit.collider.CompareTag("Enemy"));
               // rayHit.collider.GetComponent<ShootingAi>().TakeDamage(damage);
        }
        chicken = Instantiate(chickenPrefab, firePoint.transform.position, Quaternion.identity);
        chicken.GetComponent<Chicken>().direction = firePoint.transform.forward;
        chicken.GetComponent<Chicken>().transform.rotation = firePoint.transform.rotation;
        BulletsShot--;
        Invoke("ResetShot", timeBetweenShooting);

        if (BulletsShot > 0 && bulletsLeft > 0)
        Invoke("Shoot", timeBetweenShots);
    }

    private void ResetShot()
    {
        readytToShoot = true;
    }

    private void Reload()

    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }

    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }
}
