using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FadeOut : MonoBehaviour
{
    public TextMeshPro tex;
    Color col;

    void Start()
    {
        col=tex.color;
    }

    // Update is called once per frame
    void Update()
    {
        if(col.a>0)
        {
            col.a-=Time.deltaTime;
            tex.color=col;
        }
    }
}