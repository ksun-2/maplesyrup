//https://elthen.itch.io/2d-pixel-art-portal-sprites
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Prime31;

public class Portal : MonoBehaviour
{
    public GameObject player;
    public string sceneDest;
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
        if (Input.GetKeyDown("up") && _controller.isGrounded && attackActionable)
        {
            SceneManager.LoadScene(sceneDest);
        }
    }
    void Update()
    {
        attackActionable = _combat.actionable;
    }
}