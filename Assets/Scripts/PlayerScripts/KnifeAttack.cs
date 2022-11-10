using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeAttack : MonoBehaviour
{
    [SerializeField] private Transform weapon;

    private WeaponHandler weaponHandler;
    private Animator weaponAnimator;

    private GameObject knifeHitbox;

    [SerializeField] private int KnifeAttackPower = 10;
    public int GetKnifeAttackPower() => KnifeAttackPower;
    

    private void Awake()
    {
        weaponHandler = weapon.GetComponent<WeaponHandler>();
        weaponAnimator = weapon.GetComponent<Animator>();

        knifeHitbox = transform.GetChild(0).gameObject;
        knifeHitbox.GetComponent<KnifeHitDetection>().SetKnifeAttack(this);
        knifeHitbox.SetActive(false);
    }
    private void Update()
    {
        Attack();
    }

    private void Attack()
    {
        if(Input.GetKeyDown(KeyCode.V) && !weaponHandler.IsBusy()
       && !weaponHandler.IsReloading() && !weaponAnimator.GetCurrentAnimatorStateInfo(0).IsName("Knife Attack 2"))
        {
            weaponAnimator.Play("Knife Attack 2");

            Invoke("ActivateKnifeHitbox", 0.08f);
        }
    }
    private void ActivateKnifeHitbox()
    {
        knifeHitbox.SetActive(true);
    }
}
