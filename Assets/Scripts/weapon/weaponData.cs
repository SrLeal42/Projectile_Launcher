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
    public float shootingTime;
    public float powerIncrease = 0.8f;
    public float maxPower = 50f;
    [Header("Recarregar")]
    public float currentAmmo;


    [HideInInspector] public bool reloading = false;
}
