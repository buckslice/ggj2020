using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TownPerson : MonoBehaviour {
    NavMeshAgent agent;
    public Transform following { get; set; } = null;
    Vector3 offset;
    Animator anim;
    Vector3 spawnPoint;
    public FaceChanger face;

    // Start is called before the first frame update
    void Start() {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        spawnPoint = transform.position;
        face = GetComponentInChildren<FaceChanger>();
    }

    public void Follow(Transform tofollow, Vector3 offset, float stoppingDistance) {
        following = tofollow;
        this.offset = offset;
        agent.stoppingDistance = stoppingDistance;
        anim.SetTrigger("Interact");
    }

    public void FollowBehind(Transform tofollow) {
        Follow(tofollow, new Vector3(0, 0, -2), 1);
    }

    public void FollowAtSide(Transform tofollow) {
        Follow(tofollow, new Vector3(2, 0, 0), 0);
    }

    public void UnFollow() {
        following = null;
        agent.SetDestination(spawnPoint);
    }

    public bool IsMyMatch(TownPerson other) {
        return gameObject.name.Split(' ')[0] == other.gameObject.name.Split(' ')[0];
    }

    // Update is called once per frame
    void Update() {
        if (following) {
            Vector3 off = following.TransformDirection(offset);
            agent.SetDestination(following.position + off);

            Vector3 dir = following.position - transform.position;
            dir.y = 0;
            dir = dir.normalized;

            //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(transform.position + dir), Time.deltaTime * 1.0f);

            transform.LookAt(transform.position + dir);
        }
    }
}
