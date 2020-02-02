using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Gate : MonoBehaviour
{
    public int requiredHappyPeople = 10;

    public enum GateState
    {
        LOCKED, UNLOCKING, UNLOCKED
    }

    public TextMeshProUGUI text;

    Animator anim;

    public GateState gate_state;

    public Collider gateSolidCollider;

    // Start is called before the first frame update
    void Start()
    {
        gate_state = GateState.LOCKED;
        anim = GetComponentInChildren<Animator>();
    }

    void FixedUpdate()
    {
        text.SetText(PlayerInteraction.happyPeople.Count + "/" + requiredHappyPeople);
    }

    IEnumerator unlockDoor()
    {
        gate_state = GateState.UNLOCKING;
        anim.SetBool("unlock", true);
        yield return new WaitForSecondsRealtime(1f);
        gateSolidCollider.enabled = false;
        gate_state = GateState.UNLOCKED;
    }

    void OnTriggerEnter(Collider other)
    {
        //If the player has enough of the happy people's open up and be destroyed/unlocked
        if (PlayerInteraction.happyPeople.Count >= requiredHappyPeople && gate_state == GateState.LOCKED)
        {
            StartCoroutine(unlockDoor());
        }
    }

}
