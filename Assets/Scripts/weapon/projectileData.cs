using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileData", menuName = "Weapon/ProjectileData")]

public class projectileData : ScriptableObject
{
    public float timeToDestroy = 5f;
    public float damage = 1;
    public float weight = 1;
    public bool isPointed = false;
    public int punchingPower = 1;
}
