using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSpriteShadows : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // enable shadows on sprites (no option for this in editor for some reason)
        //var sr = GetComponent<SpriteRenderer>();
        var sprites = GetComponentsInChildren<SpriteRenderer>();
        foreach(var sprite in sprites) {
            sprite.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
            sprite.receiveShadows = true;
        }



    }

}
