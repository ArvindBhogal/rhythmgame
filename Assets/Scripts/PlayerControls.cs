using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerControls : MonoBehaviour
{
    public float speed = 5.0f;
    public float jumpPower = 5.0f;
    private bool justJumped = false;
    public GameObject player; 
    private Rigidbody2D playerRigidbody;
    public bool isMoving;
    public bool onGround = false;
    public BoxCollider2D floorCollider;
    public ContactFilter2D floorFilter;
    public AudioSource footsteps;
    public AudioSource bgm;
    public bool isPlayable;
    private SpriteRenderer playerSprite; 
    public GameObject activeText;
    private TMP_Text text;
    private FadeIn fade;
    public GameObject fadeEffect;
    private float playerSize;
    private int collectionNumber;

    public ParticleSystem pianoParticle1;
    public ParticleSystem pianoParticle2;

    public ParticleSystem swordParticle1;
    public ParticleSystem swordParticle2;
    private bool fadeInActive;
    private bool fadeOutActive;
    private Coroutine activeFadeEffect;


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
            // MovePlayer();
            clampPlayerMovement();
        } else {
            
            playerRigidbody.velocity = new Vector2(0, 0);
        }

        if (Input.GetKeyDown(KeyCode.Return)) {
            if (isPlayable) {
                ViewMainSongs();
            }
        }

        onGround = floorCollider.IsTouching(floorFilter);

        if (isMoving && onGround) {
            footsteps.UnPause();
        } else {
            footsteps.Pause();
        }

        if (!justJumped && Input.GetKeyDown(KeyCode.Space) && onGround) {
            justJumped = true;
        }

        // if (fadeInActive) {
        //     StartCoroutine(fade.FadeInItem());
        // } else if (fadeOutActive && !fadeInActive) {
        //     StartCoroutine(fade.FadeOutItem());
        // }


    }

    void FixedUpdate() {
        MovePlayer();
        if (justJumped) {
            JumpPlayer();
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


        playerRigidbody.velocity = new Vector2(inputX * speed, playerRigidbody.velocity.y);
    }

    private void JumpPlayer() {
        justJumped = false;
        playerRigidbody.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
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
            fade = collision.gameObject.GetComponentInChildren<FadeIn>();

            if (activeFadeEffect != null) {
                StopCoroutine(activeFadeEffect);
                activeFadeEffect = null;
            }
            activeFadeEffect = StartCoroutine(fade.FadeInItem()); 

            isPlayable = true;

            if (collision.gameObject.name == "Piano") {
                collectionNumber = 1;
                pianoParticle1.Play();
                pianoParticle2.Play();

            } else if (collision.gameObject.name == "Sword") {
                collectionNumber = 2;
                swordParticle1.Play();
                swordParticle2.Play();
            }


        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        // activeText = null;
        // text.enabled = false;
        if (collision.gameObject.tag == "Play") {
            // text = null;
            isPlayable = false;
            fade = collision.gameObject.GetComponentInChildren<FadeIn>();

            if (activeFadeEffect != null) {
                StopCoroutine(activeFadeEffect);
                activeFadeEffect = null;
            }
            activeFadeEffect = StartCoroutine(fade.FadeOutItem());

            if (collision.gameObject.name == "Piano") {
                pianoParticle1.Stop();
                pianoParticle2.Stop();

            } else if (collision.gameObject.name == "Sword") {
                swordParticle1.Stop();
                swordParticle2.Stop();
            }
        }
    }

    public void ViewMainSongs() {
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
        playerRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;

        PlayerPrefs.SetInt("collectionNumber", collectionNumber);

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
