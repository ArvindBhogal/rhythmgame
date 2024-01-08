using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FadeIn : MonoBehaviour
{
    public TMP_Text tex;
    Color col;

    void Start() {
        col = tex.color;
    }

    public IEnumerator FadeInItem() {
        // while (col.a < 256) {
        for (float a = col.a; a <= 1; a += 0.02f) {
            col.a = a;
            tex.color = col;
            Debug.Log(col); 
            yield return null;
        }
    }

    public IEnumerator FadeOutItem() {
        // while (col.a > 0) {
        for (float a = col.a; a >= 0; a -= 0.02f) {
            col.a = a;
            tex.color = col;
            Debug.Log(col);   
            yield return null;
        }
        tex.enabled = false;
    }
}