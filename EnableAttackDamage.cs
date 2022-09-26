using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableAttackDamage : MonoBehaviour
{

    private AttackCombosController Attackcs;
    public EnemyAi enemyScript;
    
    public void OnTriggerEnter(Collider col)
    {

        if (col.gameObject.tag == "Enemy")
        {
            enemyScript = col.GetComponent<EnemyAi>();
        }
    }
    
}
