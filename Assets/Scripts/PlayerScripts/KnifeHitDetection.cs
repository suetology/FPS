using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeHitDetection : MonoBehaviour
{
    [SerializeField] private float radius = 1f;
    [SerializeField] private LayerMask layer;

    private KnifeAttack knife;

    public void SetKnifeAttack(KnifeAttack knifeAttack)
    {
        knife = knifeAttack;
    }


    private void Update()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, radius, layer);

        if (hits.Length > 0)
        {
            hits[0].gameObject.GetComponentInChildren<EnemyStats>().Health -= knife.GetKnifeAttackPower();
        }
        gameObject.SetActive(false);
    }
}
