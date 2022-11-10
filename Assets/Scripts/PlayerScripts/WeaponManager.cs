using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private WeaponHandler[] weapons;

    private int currentWeaponIndex;

    private void Start()
    {
        currentWeaponIndex = 0;
        weapons[currentWeaponIndex].gameObject.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            TurnOnSelectedWeapon(0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            //TurnOnSelectedWeapon(1);
        }
    }

    private void TurnOnSelectedWeapon(int index)
    {
        if(index == currentWeaponIndex)
        {
            return;
        }

        weapons[currentWeaponIndex].gameObject.SetActive(false);

        currentWeaponIndex = index;

        weapons[currentWeaponIndex].gameObject.SetActive(true);

    }

}
