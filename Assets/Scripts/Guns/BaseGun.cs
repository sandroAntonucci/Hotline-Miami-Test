using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseGun : MonoBehaviour
{

    public string gunName;
    public int damage;
    public float fireRate;
    public int maxAmmo;
    public int currentAmmo;

    public abstract void Shoot();

}
