using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponScript : MonoBehaviour
{
    public weaponData WData;
    public weaponData[] allWeapons;
    private int WIndex = 0;
    [HideInInspector] public bool shooting = false;
    private float timeSinceLastShoot;
    public Transform muzzle;
    public Camera playerCamera;
    private Transform weaponTransform;
    private bool CanShoot => !WData.reloading && WData.currentAmmo > 0 && !shooting;


    // Start is called before the first frame update
    void Start()
    {
        weaponTransform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            changeWeapon();
        }

        timeSinceLastShoot += Time.deltaTime;

        if (timeSinceLastShoot >= WData.shootingTime)
        {
            shooting = false;
        }

        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        Vector3 directionToHit = (ray.direction - muzzle.position).normalized;
        weaponTransform.forward = directionToHit;

    }



    public void shoot(Vector3 _dir, float power = 0)
    {
        if (CanShoot)
        {
            shooting = true;
            timeSinceLastShoot = 0;

            Transform bulletObj = Instantiate(WData.bulletPrefab, muzzle.position, muzzle.rotation);
            bulletObj.GetComponent<projectile>().vel = power;
            bulletObj.GetComponent<projectile>().setDirection(_dir);
            bulletObj.transform.forward = _dir;

            //WData.currentAmmo--;
            timeSinceLastShoot = 0f;
        }

    }

    public void changeWeapon()
    {
        WIndex++;

        if (WIndex >= allWeapons.Length)
        {
            WIndex = 0;
        } else if (WIndex < 0)
        {
            WIndex = allWeapons.Length - 1;
        }

        WData = allWeapons[WIndex];

    }


}
