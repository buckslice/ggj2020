using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour {

    TownPerson active_follower = null;
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.E)) {
            Collider[] nearby = Physics.OverlapSphere(transform.position, 2, 1 << 8);
            //Debug.Log(nearby.Length);
            foreach (var near in nearby) {
                var person = near.GetComponent<TownPerson>();
                if (!person.isfollowing) {
                    person.Follow(transform, new Vector3(2,0,0), 0);
                }
            }
        }
    }
}
