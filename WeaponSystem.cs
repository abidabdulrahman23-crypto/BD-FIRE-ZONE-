using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Weapon
{
    public string weaponName;
    public float damage = 25f;
    public float fireRate = 0.1f;
    public int maxAmmo = 30;
    public int currentAmmo;
    public GameObject bulletPrefab;
}

public class WeaponSystem : MonoBehaviour
{
    public List<Weapon> weapons = new List<Weapon>();
    private int currentWeaponIndex = 0;
    private float lastFireTime = 0f;
    private Weapon currentWeapon;

    void Start()
    {
        if (weapons.Count > 0)
        {
            currentWeapon = weapons[currentWeaponIndex];
            currentWeapon.currentAmmo = currentWeapon.maxAmmo;
        }
    }

    public void Shoot(Transform firePoint, Vector3 direction)
    {
        if (currentWeapon == null || currentWeapon.currentAmmo <= 0)
            return;

        if (Time.time - lastFireTime < currentWeapon.fireRate)
            return;

        lastFireTime = Time.time;
        currentWeapon.currentAmmo--;

        GameObject bullet = Instantiate(currentWeapon.bulletPrefab, firePoint.position, firePoint.rotation);
        BulletBehavior bulletBehavior = bullet.GetComponent<BulletBehavior>();
        
        if (bulletBehavior != null)
        {
            bulletBehavior.SetDamage(currentWeapon.damage);
            bulletBehavior.SetDirection(direction);
        }

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = direction * 50f;
        }
    }

    public void Reload()
    {
        if (currentWeapon != null)
        {
            currentWeapon.currentAmmo = currentWeapon.maxAmmo;
        }
    }

    public void SwitchWeapon()
    {
        currentWeaponIndex = (currentWeaponIndex + 1) % weapons.Count;
        currentWeapon = weapons[currentWeaponIndex];
        currentWeapon.currentAmmo = currentWeapon.maxAmmo;
    }

    public Weapon GetCurrentWeapon()
    {
        return currentWeapon;
    }

    public int GetCurrentAmmo()
    {
        return currentWeapon != null ? currentWeapon.currentAmmo : 0;
    }

    public int GetMaxAmmo()
    {
        return currentWeapon != null ? currentWeapon.maxAmmo : 0;
    }

    public string GetWeaponName()
    {
        return currentWeapon != null ? currentWeapon.weaponName : "None";
    }
}