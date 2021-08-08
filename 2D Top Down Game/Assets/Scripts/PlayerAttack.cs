using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private float timebtwAttack;
    public float startTimebtwAttack;

    public Transform attackPos;
    public float attackRange;
    public LayerMask whatIsEnemy;
    public float damage = 10f;

    public Animator animator;

    // Update is called once per frame
    void Update()
    {
        if (timebtwAttack <= 0)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                animator.SetTrigger("Attack");
                Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemy);
                //Debug.Log("ATTACK");
                for (int i = 0; i < enemiesToDamage.Length; i++)
                {
                    StartCoroutine(enemiesToDamage[i].GetComponent<EnemyManager>().TakeDamage(damage));
                }

                timebtwAttack = startTimebtwAttack;
            }
        }else
        {
            timebtwAttack -= Time.deltaTime;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}
