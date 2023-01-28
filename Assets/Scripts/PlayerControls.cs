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
    public AudioSource footsteps;
    public AudioSource bgm;
    public bool isPlayable;
    private SpriteRenderer playerSprite; 
    public GameObject activeText;
    private TMP_Text text;
    public GameObject fadeEffect;
    private float playerSize;
    // Update is called once per frame

    void Start() {
        footsteps.Play();
        playerSprite = GetComponent<SpriteRenderer>();
        playerRigidbody = GetComponent<Rigidbody2D>();
        isMoving = false;
        isPlayable = false;

        playerSize = playerSprite.bounds.size.x;

    }

    void Update() {

        if (!fadeEffect.activeSelf) {
            MovePlayer();
            clampPlayerMovement();
        } else {
            playerRigidbody.velocity = new Vector2(0, 0);
        }

        if (Input.GetKeyDown(KeyCode.Return)) {
            if (isPlayable) {
                ViewSongs();
            }
        }

        if (isMoving) {
            footsteps.UnPause();
        } else {
            footsteps.Pause();
        }
    }

    private void MovePlayer() {
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");

        if (inputX > 0) {
            playerSprite.flipX = true;
            isMoving = true;
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
                inputX -= 0.6f;
            }
        }
        else if (inputX < 0) {
            playerSprite.flipX = false;
            isMoving = true;
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
                inputX += 0.6f;
            }
        }

        else {
            isMoving = false;
        }


        playerRigidbody.velocity = new Vector2(inputX * speed, inputY);
    }

    private void clampPlayerMovement() {

        Vector3 position = transform.position;

        float leftBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, -1)).x + 1;
        float rightBorder = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, -1)).x - 1;

        position.x = Mathf.Clamp(position.x, leftBorder, rightBorder);
        transform.position = position;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        // text = activeText.GetComponent<TMP_Text>();
        // text.enabled = true;
        if (collision.gameObject.tag == "Play") {
            text = collision.gameObject.GetComponentInChildren<TMP_Text>();
            text.enabled = true;
            isPlayable = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        // activeText = null;
        // text.enabled = false;
        if (collision.gameObject.tag == "Play") {
            text.enabled = false;
            text = null;
            isPlayable = false;
        }
    }

    public void ViewSongs() {
        StartCoroutine(DelaySecondLoad());
    }

    public IEnumerator DelaySecondLoad() {
        if (bgm) {
            bgm.Stop(); 
        } 
        if (footsteps) {
            footsteps.Stop();
        }
        fadeEffect.SetActive(true);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("songSelect");
    }




    // private void Jump() {
    //     playerRigidbody.velocity = new Vector2(0, jumpPower);
    // }

    // private bool isGrounded() {
    //     bool groundCheck = Physics2D.Raycast(transform.position, Vector2.down, 0.7f);
    //     return groundCheck.collider 
    // }
}
