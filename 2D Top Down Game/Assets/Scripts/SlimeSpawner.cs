using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeSpawner : MonoBehaviour
{

    public float spawn_delay = 2f;
    private float next_spawn = 0.0f;
    public int max_enemy_count;

    public GameObject enemy_prefab;
    public GameObject healthBar;

    private int enemyCount;


    // Update is called once per frame
    void Update()
    {
        enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;

        if (Time.time > next_spawn && enemyCount < max_enemy_count)
        {
            int x = Random.Range(-11, 11);
            int y = Random.Range(-6, 6);

            GameObject newEnemy = Instantiate(enemy_prefab, new Vector2(x, y), Quaternion.identity);
            GameObject newHealthBar = Instantiate(healthBar, new Vector2(x, y), Quaternion.identity);
            newHealthBar.transform.localScale *= 0.8f;
            newHealthBar.transform.SetParent(newEnemy.transform);
            //newHealthBar.GetComponent<HealthBar>().ScaleHealthBar(newHealthBar, 0.6f);
            
            next_spawn = Time.time + spawn_delay;
        }
    }
}
