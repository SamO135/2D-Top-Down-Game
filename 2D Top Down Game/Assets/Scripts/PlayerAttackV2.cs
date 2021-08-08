using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackV2 : MonoBehaviour
{
    public float attackSpeed;   //Number of attacks per second
    private float attackDelay;      // Delay between each attack
    private float attackStart;      // Stores the time each attack starts
    private float attackDuration;   // The duration of the attack (animation)
    //public Vector2 damageCircleOffset;
    public float damageRadius;
    public float attackMovementSpeed;
    public float damage = 10f;      // Attack damage
    public float knockback = 8f;    // Knockback force

    public Animator animator;
    public PlayerManager playerManager;



    private void Start()
    {
        attackDelay = 1f / attackSpeed; 

        this.transform.localScale *= damageRadius;

        string[] clipNames = new string[10];
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            switch (clip.name)
            {
                case "Player_Attack_Right":
                    attackDuration = clip.length * 0.7f; //The *0.7f makes the attack hitbox disappear a bit before the animation ends, 
                    break;                               //otherwise it will linger and could damage enemies after the sword has already been swung.

            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (attackDelay <= 0)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                animator.SetTrigger("Attack");
                attackStart = Time.time;
                attackDelay = 1/attackSpeed;
            }
        }
        else
        {
            attackDelay -= Time.deltaTime;
        }


        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") && Time.time < attackStart + attackDuration)
        {
            gameObject.GetComponent<CircleCollider2D>().enabled = true;
            //gameObject.GetComponent<SpriteRenderer>().enabled = true;
            playerManager.moveSpeed = attackMovementSpeed;
        }
        else if (Time.time > attackStart + attackDuration)
        {
            gameObject.GetComponent<CircleCollider2D>().enabled = false;
            //gameObject.GetComponent<SpriteRenderer>().enabled = false;
            playerManager.moveSpeed = playerManager.defaultMoveSpeed;
        }
    }

    

    /*private void OnTriggerEnter2D(Collider2D collision)
    {
            //Debug.Log("HIT");
            StartCoroutine(collision.GetComponent<EnemyManager>().TakeDamage(damage));
    }*/

}
