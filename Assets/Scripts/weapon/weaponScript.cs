using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class weaponScript : MonoBehaviour
{
    [Header("Weapons")]
    public weaponData WData;
    public weaponData[] allWeapons;
    private int WIndex = 0;

    [Header("Tiro")]
    //[HideInInspector] public bool shooting = false;
    private float timeSinceLastShoot;
    public Transform muzzle;
    public Camera playerCamera;
    private Transform weaponTransform;

    [Header("UI")]
    public TMP_Text weaponText;
    public Image weaponTextBackGround;
    public TMP_Text weaponReloadText;
    public Image weaponReloadTextBackGround;

    private bool CanShoot => !WData.reloading && WData.currentAmmo > 0;


    // Start is called before the first frame update
    void Start()
    {
        weaponTransform = GetComponent<Transform>();
        adjustingWeaponNameText(WData.name);
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            changeWeapon();
        }


        toggleReloadText(WData.reloading);

        timeSinceLastShoot -= Time.deltaTime;

        if (timeSinceLastShoot <= 0)
        {
            WData.reloading = false;
            //shooting = false;
        }

        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        Vector3 directionToHit = (ray.direction - muzzle.position).normalized;
        weaponTransform.forward = directionToHit;

    }



    public void shoot(Vector3 _dir, float power = 0)
    {
        if (CanShoot)
        {
            //shooting = true;

            Transform bulletObj = Instantiate(WData.bulletPrefab, muzzle.position, muzzle.rotation);
            bulletObj.GetComponent<projectile>().vel = power;
            bulletObj.GetComponent<projectile>().setDirection(_dir);
            bulletObj.transform.forward = _dir;

            //WData.currentAmmo--;
            WData.reloading = true;
            timeSinceLastShoot = WData.reloadingTime;
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

        adjustingWeaponNameText(WData.name);

    }


    public void adjustingWeaponNameText(string weaponName, int gap = 30)
    {
        weaponText.text = weaponName;
        weaponTextBackGround.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, weaponText.preferredWidth + gap);
    }

    public void adjustingWeaponReloadText(string time, int gap = 50)
    {
        weaponReloadText.text = time;
        weaponReloadTextBackGround.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, weaponReloadText.preferredWidth + gap);
    }

    public void toggleReloadText(bool enable)
    {
        weaponReloadText.enabled = enable;
        weaponReloadTextBackGround.enabled = enable;
        adjustingWeaponReloadText(timeSinceLastShoot.ToString("F2"));
    }

}
