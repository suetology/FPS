using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStats : MonoBehaviour
{
    private EnemyController enemyController;

    [SerializeField] private int health;
    [SerializeField] private int maxHealth;
    [SerializeField] private int attackPower;
    public int GetAttackPower() => attackPower;

    private void OnEnable()
    {
        enemyController = GetComponentInParent<EnemyController>();

        health = maxHealth;
    }
    public int Health
    {
        get => health;
        set
        {
            health = value;
            if(health <= 0)
            {
                Death();
            }
            else if(health > maxHealth)
            {
                health = maxHealth;
            }
            SetHealthbarFill();
        }
    }
    private Image healthBar;

    private void SetHealthbarFill()
    {
        if (healthBar == null)
        {
            healthBar = transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>();
        }
        healthBar.fillAmount = (float)health / maxHealth;
    }
    
    public void TakeDamage(int damage)
    {
        Health -= damage;

        enemyController.ChangeChaseDistance(Vector3.Distance(transform.position, enemyController.target.position) + 2f);
    }
    private void Death()
    {
        Destroy(transform.parent.gameObject);
    }
}
