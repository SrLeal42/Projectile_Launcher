using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Weapon/WeaponData")]

public class weaponData : ScriptableObject
{
    [Header("Info")]
    public new string name;

    [Header("Tiro")]
    public Transform bulletPrefab;
    public float powerIncrease = 0.8f;
    public float maxPower = 50f;
    [Header("Recarregar")]
    public float currentAmmo;
    public float reloadingTime;

    [HideInInspector] public bool reloading = false;
}
