using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Prime31;

public class Portal : MonoBehaviour
{
    public GameObject player;
    private bool attackActionable;
    private CharacterController2D _controller;
    private Combat _combat;
    void Awake()
    {
        _controller = player.GetComponent<CharacterController2D>();
        _combat = player.GetComponent<Combat>();
    }
    void OnTriggerStay2D(Collider2D col)
    {
        //ladder
        if (Input.GetKey("up") && _controller.isGrounded && attackActionable)
        {
            SceneManager.LoadScene("2");
        }
    }
    void Update()
    {
        attackActionable = _combat.actionable;
    }
}