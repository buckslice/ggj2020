using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TownPerson : MonoBehaviour
{
    public bool isfollowing = false;
    NavMeshAgent agent;
    Transform following;
    public void Follow(Transform tofollow) {
        following = tofollow;
        isfollowing = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (following) { 
            agent.SetDestination(following.position); 
        }
    }
}
