using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour {
    public Transform follow;
    public float lambda = 40.0f;

    // Start is called before the first frame update
    void Start() {
        DontDestroyOnLoad(gameObject);
    }

    // make camera follow player smoothly, gets rid of annoying jitter 
    // while moving (with physics in fixedupdate) and turning camera (during update)
    // could just move cam to fixedupdate but then doesnt go past 50 fps which wont look as SMOOTH
    void LateUpdate() {
        //float t = 1.0f - Mathf.Pow(0.001f, Time.deltaTime);
        float t = 1.0f - Mathf.Exp(-lambda * Time.deltaTime);
        transform.position = Vector3.Lerp(transform.position, follow.position, t);
        transform.rotation = Quaternion.Slerp(transform.rotation, follow.rotation, t);
    }

    public void Snap() {
        transform.position = follow.position;
        //transform.rotation = follow.rotation;
    }
}
