using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour {

    TownPerson onDeckPerson = null;
    public static List<TownPerson> happyPeople = new List<TownPerson>(); // list of all the matched friends!

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
                if (person.following) {
                    continue;
                }

                if (onDeckPerson) {
                    // check for match with nearby guy
                    if (onDeckPerson.IsMyMatch(person)) {
                        TownPerson front = null;
                        if (happyPeople.Count > 0) {
                            front = happyPeople[happyPeople.Count - 1];
                        }

                        happyPeople.Add(person);
                        happyPeople.Add(onDeckPerson);
                        onDeckPerson.FollowBehind(transform);
                        person.FollowBehind(onDeckPerson.transform);
                        if (front) {
                            front.FollowBehind(person.transform);
                        }


                    } else {
                        onDeckPerson.UnFollow();
                    }
                    onDeckPerson = null;

                } else {
                    person.FollowAtSide(transform);
                    onDeckPerson = person;
                }
                break;
            }
        }
    }

}