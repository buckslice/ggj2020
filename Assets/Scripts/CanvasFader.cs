using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasFader : MonoBehaviour {

    public Image background;

    // Start is called before the first frame update
    void Start() {
        StartCoroutine(FadeOut(3.0f));
    }

    // Update is called once per frame
    void Update() {

    }

    IEnumerator FadeOut(float time) {
        yield return new WaitForSeconds(1);
        float t = 0.0f;
        var c = background.color;
        while (t < time) {
            t += Time.deltaTime;
            c.a = t / time;
            background.color = c;
            yield return null;
        }
    }
}
