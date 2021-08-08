using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private float maxHealth;
    private float currentHealthNormalized;
    [SerializeField]
    private float updateSpeedSeconds = 0.2f;
    private bool hit;

    public Image healthBar;
    //public RectTransform barPos;
    public Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        switch (gameObject.tag)
        {
            case "Player":
                maxHealth = gameObject.GetComponent<PlayerManager>().maxHealth;
                break;
            case "Enemy":
                maxHealth = gameObject.GetComponent<EnemyManager>().maxHealth;
                healthBar = gameObject.GetComponent<EnemyManager>().healthBarFill;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //barPos.position = transform.position - offset;

        switch (gameObject.tag)
        {
            case "Player":
                currentHealthNormalized = gameObject.GetComponent<PlayerManager>().currentHealth / maxHealth;
                hit = gameObject.GetComponent<PlayerManager>().hit;
                break;
            case "Enemy":
                currentHealthNormalized = gameObject.GetComponent<EnemyManager>().currentHealth / maxHealth;
                hit = gameObject.GetComponent<EnemyManager>().hit;
                break;
        }


        //healthBar.fillAmount = currentHealthPercentage;
        if (hit)
        {
            //Debug.Log(gameObject.tag + " hit");
            StartCoroutine(ChangeHealthBar());
        }
            

    }


    private IEnumerator ChangeHealthBar()
    {
        float preChange = healthBar.fillAmount;
        float elapsed = 0f;

        while (elapsed < updateSpeedSeconds)
        {
            elapsed += Time.deltaTime;
            healthBar.fillAmount = Mathf.Lerp(preChange, currentHealthNormalized, elapsed / updateSpeedSeconds);
            yield return null;
        }
        healthBar.fillAmount = currentHealthNormalized;
        //Debug.Log(healthBar.fillAmount.ToString());


    }

    public void ScaleHealthBar(GameObject hb, float size)
    {
        Vector2 newScale = hb.transform.localScale;
        newScale *= 0.7f;                              // NOT WORKING - USED IN 'SLIME SPAWNER' SCRIPT
        hb.transform.localScale = newScale;
    }
}
