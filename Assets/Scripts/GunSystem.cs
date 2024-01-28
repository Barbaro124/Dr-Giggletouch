using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSystem : MonoBehaviour
{
    public int damage;
    public float timeBetweenShooting, spread, range, reloadTime, timeBetweenShots;
    public int magazineSize, BulletsPerTap;
    public bool allowButtonHold;
    int bulletsLeft, BulletsShot;

    //bools
    bool shooting, readytToShoot, reloading;

    //Reference
    public Camera fpsCam;
    public Transform attackPoint;
    public RaycastHit rayHit;
    public LayerMask whatIsEnemy;

    private void Awake()
    {
        bulletsLeft = magazineSize;
        readytToShoot = true;
    }


    private void Update()
    {
        MyInput();
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

        //Spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        //Calculate Direction with Spread
        Vector3 direction = fpsCam.transform.forward + new Vector3(x, y, 0);


        //Raycast
        if (Physics.Raycast(fpsCam.transform.position, direction, out rayHit, range, whatIsEnemy))
        {
            Debug.Log(rayHit.collider.name);

            if (rayHit.collider.CompareTag("Enemy"));
               // rayHit.collider.GetComponent<ShootingAi>().TakeDamage(damage);
        }
        bulletsLeft--;
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

    private void reloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }
}
