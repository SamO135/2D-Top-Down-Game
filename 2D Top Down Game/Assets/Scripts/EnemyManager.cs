using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour
{

    public float maxHealth;
    [HideInInspector]
    public float currentHealth;
    public float damageDealtOnCollision;

    public float defaultMoveSpeed;
    private float moveSpeed;
    private float damageFlash;
    [HideInInspector]
    public bool hit = true;

    public Rigidbody2D enemy_rb;
    public Rigidbody2D player_rb;
    public Animator animator;
    public Image healthBarFill;

    private Vector2 movement;

    //bool restart = false;

    void Start()
    {
        moveSpeed = defaultMoveSpeed;
        player_rb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        healthBarFill = this.GetComponentInChildren<Image>();
    }


    // Update is called once per frame
    void Update()
    {
        movement = player_rb.position - enemy_rb.position;
        //moveSpeed = defaultMoveSpeed;
        
        //if (currentHealth <= 0)
        //{
        //    StartCoroutine(Kill(gameObject, ));
            //Debug.Log("Destroyed");
        //}
    }


    private void FixedUpdate()
    {
        enemy_rb.MovePosition(enemy_rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
    }


    private void LateUpdate()
    {
        hit = false;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Weapon"))
        {
            StartCoroutine(TakeDamage(collision.GetComponent<PlayerAttackV2>().damage, collision.GetComponent<PlayerAttackV2>().knockback));
            hit = true;
        }      
    }


    public IEnumerator TakeDamage(float damage, float knockback = 0f)
    {
        float stunDuration = knockback / 10f;
        currentHealth -= damage;
        gameObject.GetComponent<SpriteRenderer>().color = Color.HSVToRGB(0, 50, 89);

        StartCoroutine(Knockback(knockback, stunDuration));

        yield return new WaitForSeconds(stunDuration);
        gameObject.GetComponent<SpriteRenderer>().color = Color.HSVToRGB(0, 0, 100);

        if (currentHealth <= 0)
        {
            StartCoroutine(Kill(gameObject));
            //Debug.Log("Destroyed");
        }
    }


    public IEnumerator Knockback(float force, float duration)
    {
        float start = force * -1;
        float end = 0f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            moveSpeed = Mathf.Lerp(start, end, elapsed / duration);
            yield return null;
        }
        moveSpeed = defaultMoveSpeed;
    }


    public IEnumerator Kill(GameObject gameObject, float stunTime = 0f)
    {
        gameObject.GetComponent<CircleCollider2D>().enabled = false;
        moveSpeed = 0f;

        yield return new WaitForSeconds(stunTime);
        Destroy(gameObject);
    }

    public IEnumerator Stun(float time)
    {
        moveSpeed = 0f;
        yield return new WaitForSeconds(time);
        moveSpeed = 1f;
    }

    
}
