using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerControls : MonoBehaviour, IDataPersistence {
    public float speed = 5.0f;
    public float jumpPower = 5.0f;
    private bool justJumped = false;
    public GameObject player;
    public PlayerControls instance;
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
    public TMP_Text cutsceneTextDisplay;
    private List<string> cutsceneTextArray;
    private FadeIn fade;
    public GameObject fadeEffect;
    private float playerSize;
    private int collectionNumber;
    public bool cutsceneActive = false;

    public ParticleSystem pianoParticle1;
    public ParticleSystem pianoParticle2;

    public ParticleSystem swordParticle1;
    public ParticleSystem swordParticle2;
    private bool fadeInActive;
    private bool fadeOutActive;
    private Coroutine activeFadeEffect;
    public GDTFadeEffect cutsceneBlackScreen;
    private int count = 0;

    // Update is called once per frame

    void Start() {
        instance = this;
        footsteps.Play();
        playerSprite = GetComponent<SpriteRenderer>();
        playerRigidbody = GetComponent<Rigidbody2D>();
        isMoving = false;
        isPlayable = false;

        playerSize = playerSprite.bounds.size.x;

        DataPersistenceManager.instance.LoadGame();
    }

    void Update() {
        if (!cutsceneActive) {
            if (!fadeEffect.activeSelf) {
                // MovePlayer();
                clampPlayerMovement(); 
            }
            else {

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
            }
            else {
                footsteps.Pause();
            }

            if (!justJumped && Input.GetKeyDown(KeyCode.Space) && onGround) {
                justJumped = true;
            }
            // } else {
            //     int numberOfDialogue = cutsceneTextArray.Count;
            //
            //     if (count < cutsceneTextArray.Count) {
            //         cutsceneTextDisplay.text = cutsceneTextArray[count];
            //
            //         if ( (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space) ) ) {
            //         count++;
            //         cutsceneTextDisplay.text = cutsceneTextArray[count];
            //         }
            //     }
            //
            //     else {
            //         cutsceneTextDisplay.enabled = false;
            //         cutsceneBlackScreen.StartEffect();
            //         bgm.Play();
            //         footsteps.UnPause();
            //         cutsceneActive = false;
            //     }
            // }
        }
        

        // private void Jump() {
        //     playerRigidbody.velocity = new Vector2(0, jumpPower);
        // }

        // private bool isGrounded() {
        //     bool groundCheck = Physics2D.Raycast(transform.position, Vector2.down, 0.7f);
        //     return groundCheck.collider 
        // }
    }
    
    void FixedUpdate()
    {
        if (!cutsceneActive)
        {
            MovePlayer();
            if (justJumped)
            {
                JumpPlayer();
            }
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

            }
            else if (collision.gameObject.name == "Sword") {
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

            }
            else if (collision.gameObject.name == "Sword") {
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

    public void LoadData(GameData data) {
        Debug.Log(data.storyProgression);
        // int tmp = -1;
        // float tmp2 = 0f;
        // data.songList.TryGetValue(1, out tmp);
        // tmp = 0;
        
        if (data.storyProgression == 0) {
            cutsceneActive = true;
            TriggerIntroCutscene();
        }

    }

    public void SaveData(ref GameData data) {

    }

    private void TriggerIntroCutscene() {
        this.bgm.Stop();
        this.footsteps.Pause();

        this.cutsceneTextArray = new List<string>();

        this.cutsceneTextArray.Add("...");
        this.cutsceneTextArray.Add("... Where am I..?");
        this.cutsceneTextArray.Add("... Are these chains..?");
        this.cutsceneTextArray.Add("... Oh, right...");
        this.cutsceneTextArray.Add("... I gave up.");
    }
}
