using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    private Vector2 localscale;

    private float origscaleX;

    void Start(){
        localscale = transform.localScale;
        origscaleX = localscale.x;
    }

    void Update(){
        localscale.x = origscaleX * (float)transform.parent.gameObject.GetComponent<Enemy>().currentHealth / 
        (float)transform.parent.gameObject.GetComponent<Enemy>().maxHealth;

        transform.localScale = localscale;
    }
}
