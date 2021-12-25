using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public float defaultMoveSpeed;
    //[HideInInspector]
    public float moveSpeed;
    public float maxHealth;
    [HideInInspector]
    public float currentHealth;
    public List<GameObject> objectsNotToFlip;
    public Transform damageCirclePos;
    private Vector2 damageCircleOffset;
    [HideInInspector]
    public bool hit = false;

    private float collisionCooldown = 0f;
    public float startCollisionCooldown = 0.3f;

    public Rigidbody2D player_rb;
    public Animator animator;

    private Vector2 movement;


    private void Start()
    {
        currentHealth = maxHealth;
        moveSpeed = defaultMoveSpeed;
        damageCircleOffset = transform.position - damageCirclePos.position;
    }


    // Update is called once per frame
    void Update()
    {
        //Input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);
        if (movement.x > 0)
        {
            animator.SetFloat("Facing", 1f);
            //transform.localScale = new Vector3(1f, 1f, 1f);
            FlipDamageCircle(1);
        }
        else if (movement.x < 0)
        {
            animator.SetFloat("Facing", -1f);
            //transform.localScale = new Vector3(-1f, 1f, 1f);
            FlipDamageCircle(-1);
        }

        // Counts down the invincibility time after getting hit.
        if (collisionCooldown > 0f)
            collisionCooldown -= Time.deltaTime;


        if (currentHealth <= 0)
        {
            StartCoroutine(Die());
        }

        //Press space to pause/unpause
        if (Input.GetKeyDown(KeyCode.Space))
            if (Time.timeScale == 1f)
                Time.timeScale = 0f;
            else
                Time.timeScale = 1f;

    }

    void FixedUpdate()
    {
        //Movement
        player_rb.MovePosition(player_rb.position + (movement.normalized * moveSpeed) * Time.fixedDeltaTime);
        
    }

    private void LateUpdate()
    {
        hit = false;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        
        if (collisionCooldown <= 0f && collision.gameObject.CompareTag("Enemy"))
        {
            //Debug.Log("COLLISION");
            collisionCooldown = startCollisionCooldown;
            TakeDamage(collision.gameObject.GetComponent<EnemyManager>().damageDealtOnCollision);
            hit = true;
        }
       
    }

    private void TakeDamage(float damage)
    {
        currentHealth -= damage;
    }

    private IEnumerator Die()
    {
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene("GameScene");
    }

    private void FlipDamageCircle(int direction)
    {
        damageCirclePos.transform.position = new Vector2 
            (transform.position.x + damageCircleOffset.x * -direction, transform.position.y - damageCircleOffset.y);

    }

}
