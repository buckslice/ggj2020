using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script : MonoBehaviour
{
    public enum GateState
    {
        LOCKED, UNLOCKING, UNLOCKED
    }

    public GateState gate_state = GateState.LOCKED;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (gate_state == GateState.LOCKED)
        {
            //if the player goes up to it, make a sound error sound
        }
        else if (gate_state == GateState.UNLOCKING)
        {
            //if the player unlocing,
        }
        else if (gate_state == GateState.UNLOCKED)
        {
            //if the 
        }
    }

    void lockDoor() { }

    void unlockDoor()
    {

    }
    



    void OnTriggerEnter(Collider other)
    {
        //If the player has enough of the happy people's open up and be destroyed/unlocked
    }

    void OnTriggerExit(Collider other)
    {
    }

    //TO


    
}
