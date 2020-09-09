using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public Vector3 shootDir;

    public void Setup(Vector3 shootDir){
        this.shootDir = shootDir;
        Destroy(gameObject, 3f);
    }
    void Update(){
        var speed = 1f;
        transform.position += shootDir * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.layer == LayerMask.NameToLayer("Enemy")){
            System.Random random = new System.Random();
            col.gameObject.GetComponent<Enemy>().takeDamage(random.Next(8, 13));
            Destroy(gameObject);
        }
    }
}
