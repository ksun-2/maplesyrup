//this allows climbing down ladders from standing
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prime31;

public class DownClimb : MonoBehaviour
{
    [HideInInspector]
    public bool canDownClimb = false;
    void OnTriggerEnter2D(Collider2D col) 
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Ladders"))
        {
            canDownClimb = true;
        }
    }
    void OnTriggerExit2D(Collider2D col) 
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Ladders"))
        {
            canDownClimb = false;
        }
    }
}
