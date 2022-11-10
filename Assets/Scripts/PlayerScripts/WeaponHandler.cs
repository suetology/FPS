using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public enum FireMode
{
    automatic,
    semiAutomatic
}
public class WeaponHandler : MonoBehaviour
{
    private Animator weaponAnimator;

    private PlayerMovement playerMovement;

    [Header("Weapon Stats")]
    [SerializeField] private int damage;
    [SerializeField] private FireMode fireMode;
    [SerializeField] private float fireRate = 15f;
    [SerializeField] private float fireRange = 100f;
    [SerializeField] private float bulletSpeed;
    private float nextTimeToFire;

    private bool isAiming;
    private bool isWeaponTakenOut = true;

    [Header("Zoom Stats")]
    [SerializeField] private Camera firstPersonCamera;
    private Camera mainCamera;
    [SerializeField] private float cameraBasicFieldOfView = 60f;
    [SerializeField] private float mainCameraZoomedFieldOfView = 50f;
    [SerializeField] private float firstPersonCameraZoomedFieldOfView = 10f;
    [SerializeField] private float zoomTime = 1000f;

    [Header("Ammo Stats")]
    [SerializeField] private int maxAmmo = 30;
    private int currentAmmo;


    [Header("Audio Clips")]
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private AudioClip reloadAmmoLeftSound;
    [SerializeField] private AudioClip reloadAmmoOutSound;
    [SerializeField] private AudioClip drawSound;
    [SerializeField] private AudioClip holsterSound;
    [SerializeField] private AudioClip aimSound;


    [Serializable]
    private class Spawnpoints
    {
        public Transform aimInBulletSpawnpoint;
        public Transform aimOutBulletSpawnpoint;

        public Transform aimInCasingSpawnpoint;
        public Transform aimOutCasingSpawnpoint;
    }
    [Serializable]
    private class Prefabs
    {
        public GameObject bulletPrefab;

        public GameObject casingPrefab;
    }

    [SerializeField] private Spawnpoints spawnpoints;
    [SerializeField] private Prefabs prefabs;


    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private GameObject impactEffect;

    private void Awake()
    {
        weaponAnimator = GetComponent<Animator>();

        playerMovement = GetComponentInParent<PlayerMovement>();

        mainCamera = Camera.main;
    }

    private void Start()
    {
        AmmoLeftText.GetInstance().SetAmmoText(currentAmmo, maxAmmo);
    }
    private void Update()
    {
        if (!IsReloading() && !IsBusy())
        {
            if (isWeaponTakenOut)
            {
                Shoot();
                AimControl();
                if(!isAiming)
                    Reload();
            }
        }
        if(!IsReloading())
            HolsterWeapon();
    }

    private void Shoot()
    {
        if (currentAmmo > 0)
        {
            if (fireMode == FireMode.automatic)
            {
                if (Input.GetMouseButton(0) && Time.time > nextTimeToFire)
                {
                    SoundManager.PlayShootSound(shootSound);

                    if (!isAiming)
                    {
                        weaponAnimator.Play("Fire", 0, 0f);
                        muzzleFlash.transform.position = spawnpoints.aimOutBulletSpawnpoint.position;
                    }
                    else
                    {
                        weaponAnimator.Play("Aim Fire", 0, 0f);
                        muzzleFlash.transform.position = spawnpoints.aimInBulletSpawnpoint.position;
                    }

                    currentAmmo--;

                    AmmoLeftText.GetInstance().SetAmmoText(currentAmmo, maxAmmo);

                    SpawnBullet();

                    ShootRaycast();

                    muzzleFlash.Play();

                    nextTimeToFire = Time.time + 1f / fireRate;
                }
            }
        }
    }

    private void ShootRaycast()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, fireRange))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                hit.collider.GetComponent<EnemyStats>().TakeDamage(damage);
            }
        }

        //GameObject impact = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));

        //Destroy(impact, 2f);
       
    }
    private void SpawnBullet()
    {
        Rigidbody bullet;

        if (isAiming)
        {
             bullet = Instantiate(prefabs.bulletPrefab,
                                            spawnpoints.aimInBulletSpawnpoint.position,
                                            spawnpoints.aimInBulletSpawnpoint.rotation
                                            ).GetComponent<Rigidbody>();
        }
        else
        {
             bullet = Instantiate(prefabs.bulletPrefab,
                                            spawnpoints.aimOutBulletSpawnpoint.position,
                                            spawnpoints.aimOutBulletSpawnpoint.rotation
                                            ).GetComponent<Rigidbody>();
        }
        if (bullet != null)
            bullet.velocity =  bullet.transform.forward * bulletSpeed;

    }

    private void AimControl()
    {
        if (playerMovement.playerState != PlayerMovement.PlayerState.sprint)
        {
            if ((Input.GetMouseButtonDown(1) || Input.GetMouseButton(1)) && !isAiming)
            {
                StartCoroutine(AimIn());
            }
        }
        else if(playerMovement.playerState == PlayerMovement.PlayerState.sprint && isAiming)
        {
            StartCoroutine(AimOut());
        }

        if (Input.GetMouseButtonUp(1) && isAiming)
        {
            StartCoroutine(AimOut());
        }
        
    }

    private float zoom = 0f;
    private IEnumerator AimIn()
    {
        SoundManager.PlaySound(aimSound);
        weaponAnimator.SetBool("aim", true);
        weaponAnimator.Play("Aim In");

        while (zoom < 1f)
        {
            mainCamera.fieldOfView = Mathf.Lerp(cameraBasicFieldOfView, mainCameraZoomedFieldOfView, zoom);
            firstPersonCamera.fieldOfView = Mathf.Lerp(cameraBasicFieldOfView, firstPersonCameraZoomedFieldOfView, zoom);
            zoom += Time.deltaTime * 2;
            yield return null;  
        } 

        isAiming = true;
        CrosshairHandler.GetInstance().HideCrosshair();
    }
    private IEnumerator AimOut()
    {
        weaponAnimator.SetBool("aim", false);
        weaponAnimator.Play("Aim Out");

        while (zoom > 0f)
        {
            mainCamera.fieldOfView = Mathf.Lerp(cameraBasicFieldOfView, mainCameraZoomedFieldOfView, zoom);
            firstPersonCamera.fieldOfView = Mathf.Lerp(cameraBasicFieldOfView, firstPersonCameraZoomedFieldOfView, zoom);
            zoom -= Time.deltaTime * 2;
            yield return null;
        }

        isAiming = false;
        CrosshairHandler.GetInstance().ShowCrosshair();
    }

    public bool IsReloading()
    {
        return weaponAnimator.GetCurrentAnimatorStateInfo(0).IsName("Reload Out Of Ammo") ||
               weaponAnimator.GetCurrentAnimatorStateInfo(0).IsName("Reload Ammo Left");
    }
    public bool IsBusy()
    {
        return weaponAnimator.GetCurrentAnimatorStateInfo(0).IsName("Draw") ||
               weaponAnimator.GetCurrentAnimatorStateInfo(0).IsName("Holster");
    }
    private void Reload()
    {
        if (Input.GetKeyDown(KeyCode.R) && currentAmmo != maxAmmo)
        {
            if (currentAmmo == 0)
            {
                weaponAnimator.Play("Reload Out Of Ammo");
                SoundManager.PlaySound(reloadAmmoOutSound);
            }
            else
            {
                weaponAnimator.Play("Reload Ammo Left");
                SoundManager.PlaySound(reloadAmmoLeftSound);
            }


            currentAmmo = maxAmmo;

            AmmoLeftText.GetInstance().SetAmmoText(currentAmmo, maxAmmo);
        }
    }

    private void HolsterWeapon()
    { 
        if(Input.GetKeyDown(KeyCode.F))
        {
            if (isWeaponTakenOut)
            {
                SoundManager.PlaySound(holsterSound);
                
                weaponAnimator.SetBool("Holster", true);
                weaponAnimator.Play("Holster");
                isWeaponTakenOut = false;
            }
            else if(!isWeaponTakenOut && weaponAnimator.GetCurrentAnimatorStateInfo(0).IsName("Holster"))
            {
                SoundManager.PlaySound(drawSound);

                weaponAnimator.SetBool("Holster", false);
                weaponAnimator.Play("Draw");
                isWeaponTakenOut = true;
            }
        }
    }

}
