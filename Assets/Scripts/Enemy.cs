using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public int maxHealth = 50;
    [HideInInspector]
    public int currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    public void takeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0){
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;
            transform.Find("HealthBar").gameObject.SetActive(false);
            this.enabled = false;
        }
    }
}
