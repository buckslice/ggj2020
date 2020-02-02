using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceChanger : MonoBehaviour {

    public Sprite[] happyFaces;
    public Sprite[] sadFaces;
    int faceIndex = 0;
    SpriteRenderer sr;
    // Start is called before the first frame update
    void Start() {
        faceIndex = Random.Range(0, happyFaces.Length);
        SetHappy(false);
    }

    public void SetHappy(bool isHappy) {
        sr = GetComponent<SpriteRenderer>();
        if (isHappy) {
            if (happyFaces.Length > 0) {
                sr.sprite = happyFaces[faceIndex];
            }
        } else {
            if (sadFaces.Length > 0) {
                sr.sprite = sadFaces[faceIndex];
            }
        }
    }

}
