using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerControls : MonoBehaviour
{
    public float speed = 5.0f;
    public float jumpPower = 5.0f;
    public GameObject player; 

    private Rigidbody2D playerRigidbody;
    public bool isMoving;
    public bool isPlayable;
    private SpriteRenderer playerSprite; 
    public GameObject activeText;
    private TMP_Text text;
    // Update is called once per frame

    void Start() {
        playerSprite = GetComponent<SpriteRenderer>();
        playerRigidbody = GetComponent<Rigidbody2D>();
        isMoving = false;
        isPlayable = false;

    }

    void Update() {
        MovePlayer();

        if (Input.GetKeyDown(KeyCode.Return)) {
            if (isPlayable) {
                SceneManager.LoadScene("songSelect");
            }
        }
    }

    private void MovePlayer() {
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");

        if (inputX > 0) {
            playerSprite.flipX = true;
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
                inputX -= 0.6f;
            }
        }
        if (inputX < 0) {
            playerSprite.flipX = false;
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
                inputX += 0.6f;
            }
        }


        playerRigidbody.velocity = new Vector2(inputX * speed, inputY);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        // text = activeText.GetComponent<TMP_Text>();
        // text.enabled = true;
        Debug.Log(collision);
        if (collision.gameObject.tag == "Play") {
            text = collision.gameObject.GetComponentInChildren<TMP_Text>();
            text.enabled = true;
            isPlayable = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        // activeText = null;
        // text.enabled = false;
        Debug.Log(collision);
        if (collision.gameObject.tag == "Play") {
            text.enabled = false;
            text = null;
            isPlayable = false;
        }
    }




    // private void Jump() {
    //     playerRigidbody.velocity = new Vector2(0, jumpPower);
    // }

    // private bool isGrounded() {
    //     bool groundCheck = Physics2D.Raycast(transform.position, Vector2.down, 0.7f);
    //     return groundCheck.collider 
    // }
}
