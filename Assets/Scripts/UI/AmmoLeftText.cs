using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AmmoLeftText : MonoBehaviour
{
    private static AmmoLeftText instance;
    public static AmmoLeftText GetInstance() => instance;

    private TextMeshProUGUI ammoLeftText;
    void Awake()
    {
        instance = this;

        ammoLeftText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    public void SetAmmoText(int ammo, int maxAmmo)
    {
        ammoLeftText.text = $"{ammo}/{maxAmmo}";
    }
}
