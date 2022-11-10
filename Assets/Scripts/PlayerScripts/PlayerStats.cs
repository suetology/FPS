using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private int health;
    [SerializeField] private int maxHealth;

    private void OnEnable()
    {
        health = maxHealth;
    }
    public int Health
    {
        get => health;
        set
        {
            health = value;
            if (health <= 0)
            {
                Death();
            }
            else if (health > maxHealth)
            {
                health = maxHealth;
            }
            SetHealthbarFill();
        }
    }
    [SerializeField] private Image healthBar;

    private void SetHealthbarFill()
    {
        healthBar.fillAmount = (float)health / maxHealth;
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
    }
    private void Death()
    {
        //game over
    }
}
