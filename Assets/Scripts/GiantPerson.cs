using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GiantPerson : MonoBehaviour {

    Animator anim;
    NavMeshAgent agent;
    public Transform following { get; set; } = null;
    Vector3 offset;
    Vector3 spawnPoint;
    Transform player;

    public enum GiantState {
        IDLE, ALERT, CHASING, PATROLLING, WIN_STATE, HUG
    }

    public void LateUpdate() {
        if (anim == null)
            return;

        anim.SetBool("idle", giantState == GiantState.IDLE);
        anim.SetBool("alert", giantState == GiantState.ALERT);
        anim.SetBool("chasing", giantState == GiantState.CHASING);
        anim.SetBool("patrolling", giantState == GiantState.PATROLLING);
        anim.SetBool("win_state", giantState == GiantState.WIN_STATE);
        anim.SetBool("hug", giantState == GiantState.HUG);
    }

    public GiantState giantState;

    // Start is called before the first frame update
    void Start() {
        player = FindObjectOfType<PlayerInteraction>().transform;
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        spawnPoint = transform.position;
        giantState = GiantState.IDLE;
    }

    public void Follow(Transform tofollow, Vector3 offset, float stoppingDistance) {

        following = tofollow;
        this.offset = offset;
        agent.stoppingDistance = stoppingDistance;
    }

    public void FollowPlayer() {
        FollowAtSide(player);
    }

    public void FollowBehind(Transform tofollow) {
        Follow(tofollow, new Vector3(0, 0, -2), 1);
    }

    public void FollowAtSide(Transform tofollow) {
        Follow(tofollow, new Vector3(2, 0, 0), 0);
    }

    public void UnFollow() {
        following = null;
        //agent.SetDestination(spawnPoint);
    }

    public bool IsMyMatch(TownPerson other) {
        return gameObject.name.Split(' ')[0] == other.gameObject.name.Split(' ')[0];
    }

    public float max_awareness = 10f;
    public float max_chase_range = 20f;

    public Transform[] patrolPoints;

    Transform currentPatrolPoint;

    // Update is called once per frame
    void Update() {

        if (Vector3.Distance(this.transform.position, player.position) < max_awareness && (giantState == GiantState.IDLE || giantState == GiantState.PATROLLING)) {
            giantState = GiantState.CHASING;
            FollowAtSide(player);
        } else if (Vector3.Distance(this.transform.position, player.position) > max_chase_range && giantState == GiantState.CHASING) {
            giantState = GiantState.PATROLLING;
            UnFollow();
        } else if (giantState == GiantState.PATROLLING) {
            //go through a list of points on the island
            currentPatrolPoint = patrolPoints[Random.Range(0,patrolPoints.Length)];
            //if (currentPatrolPoint.position) {
                //TODO: If the distance of the giant is 
            //}
        }

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