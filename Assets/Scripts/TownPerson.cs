using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TownPerson : MonoBehaviour {
    public bool isfollowing = false;
    NavMeshAgent agent;
    Transform following;
    Vector3 offset;
    Animator anim;

    // Start is called before the first frame update
    void Start() {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
    }

    public void Follow(Transform tofollow, Vector3 offset, float stoppingDistance) {
        following = tofollow;
        this.offset = offset;
        agent.stoppingDistance = stoppingDistance;
        isfollowing = true;
        anim.SetTrigger("Interact");
    }

    // Update is called once per frame
    void Update() {
        if (following) {
            Vector3 off = following.TransformDirection(offset);
            agent.SetDestination(following.position + off);
        }
    }
}
