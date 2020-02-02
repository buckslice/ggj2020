using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasFader : MonoBehaviour {

    public Image background;

    public IEnumerator FadeOut(float time) {
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
