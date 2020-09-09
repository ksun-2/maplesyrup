//maplestory-like combat, where hits and stuff are detected
//on the first frame of the attack. next time I should use 
//collider hitboxes so it doesn't have to be like that

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prime31;

public class Combat : MonoBehaviour
{
    [HideInInspector]
    public bool actionable = true;
    public Vector2 attackRange;
    public LayerMask enemyLayers;
    public Transform attackPoint;
    //public GameObject fireball;
    private float attackDiffX;
    private Animator animator;
    private float attackTime = 0.800f; //from LightGuard_Attack.anim
    private float actionableTime;

    void Start()
    {
        animator = GetComponent<Animator>();
        actionable = true;
        attackDiffX = transform.position.x - attackPoint.transform.position.x;
    }

    void Update()
    {
        if (Input.GetKeyDown("f") && actionable){
            animator.Play(Animator.StringToHash("Attack"));
            Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackPoint.position, attackRange, 0, enemyLayers);
            foreach (Collider2D enemy in hitEnemies){
                Debug.Log("HIT");
                System.Random random = new System.Random();
                enemy.GetComponent<Enemy>().takeDamage(random.Next(8, 13));
            }

            actionable = false;
            actionableTime = Time.time + attackTime;
        }
        else if (!actionable && Time.time >= actionableTime){
            actionable = true;
        }
        /*else if (Input.GetKeyDown("g") && actionable){
            var position = GetComponent<Transform>().position;
            var fireballObject = Instantiate(fireball, position, Quaternion.identity);

            Vector3 shootDir;
            if (GetComponent<Movement>()._renderer.flipX == true){
                shootDir = new Vector3(1,0,0);
            }
            else{
                shootDir = new Vector3(-1,0,0);
            }
            fireballObject.GetComponent<Fireball>().Setup(shootDir);
        }*/

        //hitbox is to the left, but player facing right
        if (attackDiffX > 0 && GetComponent<SpriteRenderer>().flipX == true){
            attackPoint.transform.Translate(new Vector2(2*attackDiffX, 0));
            attackDiffX *= -1;
        }
        else if (attackDiffX < 0 && GetComponent<SpriteRenderer>().flipX == false){
            attackPoint.transform.Translate(new Vector2(2*attackDiffX, 0));
            attackDiffX *= -1;
        }
    }

    private void OnDrawGizmos() {
        Gizmos.DrawWireCube(attackPoint.position, attackRange);
    }
}
