using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    private SpriteRenderer spriteR;
    public Sprite defaultImage;
    public Sprite pressedImage;
    public LaneController laneController; 

    public KeyCode keyToPress;
    public Color tmpColor;

    // Start is called before the first frame update
    void Start() {
        spriteR = GetComponent<SpriteRenderer>();
        tmpColor = spriteR.color;
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(keyToPress)) {
            spriteR.sprite = pressedImage;
            spriteR.color = new Color(1, 1, 1, 1);

        }

        else if (Input.GetKeyUp(keyToPress)) {
            spriteR.sprite = defaultImage;
            spriteR.color = tmpColor;
        }
    }
}
