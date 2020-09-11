//this fixes the "bouncing" on top of ladder
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prime31;

public class FeetBox : MonoBehaviour
{
    public GameObject player;
    private CharacterController2D _controller;
    void Awake(){
        _controller = player.GetComponent<CharacterController2D>();
    }
    private void OnTriggerExit2D(Collider2D col) {
        if (col.gameObject.layer == LayerMask.NameToLayer("Ladders")){
            player.GetComponent<Movement>().stopClimbing();
            _controller.velocity.y = 0;
        }
    }
}
