using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    // public variables
    public float baseSize;
    public float shrinkRatio;
    public float pShrinkRatio;
    public float growRatio;
    public float power;
    public int bounceLimit;
    public int bounceAdd;
    public int needleSpeed;
    public Transform needleDir;
    public Text bounceText;
    public RawImage promptImg;
    public Texture goImg;
    public Texture gameOverImg;
    public Texture lifeIcon;
    public Sprite shieldSprite;
    public Sprite bounceSprite;
    public Sprite normalSprite;
    public Sprite deadSprite;
    public Sprite smokeSprite;
    public Sprite winSprite;
    public Sprite frictionSprite;
    public SpriteRenderer shieldRenderer;
    public SpriteRenderer smokeRenderer;
    public Slider starBar;
	public GameObject restartButton;
	public GameObject menuButton;


	public AudioClip bounceSound;
	public AudioClip getStar;
	public AudioClip meetMonsterSound;
	public AudioClip slipSound;
	public AudioClip frictionSound;
	public AudioClip victorySound;
	public AudioClip openDoorSound;
	public AudioClip levelStartSound;
	public AudioClip defeatedSound;
	public AudioClip powerSound;
	public AudioClip getthinSound;
	public float bounceSoundVol;

    

    // constants
    private const string BOUNCE_STR = "Bounces left: ";
    private const string NO_BOUNCE_STR = "No bounces left!";
    private const string STAR_STR = "Stars: ";
    private const string DIE_STR = "YOU DIED!!";
    private const string WIN_STR = "YOU WIN! Next level >>";
    private const int LAST_LEVEL = 12;
    

	private bool ifFriction;
	private bool ifSlippery;
	private bool ifCollided;
    private bool isDead;
    private bool isShield;
    private bool isWin;
    private int bounceCount;
    private int starCount;
    private Rigidbody2D rb;
	private Transform tf;
    private Vector2 screenCorner;
    private SpriteRenderer sr;
    private float bounceTimer;
    private float startTimer;
    private float smokeTimer;
    private float winTimer;
    private float dieTimer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

		ifFriction = false;
		ifCollided = false;
		ifSlippery = false;
        isShield = false;
        isDead = false;
        isWin = false;
        starCount = 0;
        bounceCount = bounceLimit;
        bounceText.text = BOUNCE_STR + bounceCount;
        startTimer = 4;
        winTimer = 2;
        dieTimer = 3;
		Debug.Log ("started\t");
        screenCorner = new Vector2(0, Screen.height);
        sr = GetComponent<SpriteRenderer>();
		playSound (levelStartSound);
		menuButton.gameObject.SetActive (false);
		restartButton.gameObject.SetActive (false);
    }
    
    void Update()
    {
        if ((!isWin && !isDead) && (Input.GetKeyDown("space") || 
            (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)))
        {   
            Vector2 movement = new Vector2(needleDir.localPosition.x, needleDir.localPosition.y);
            rb.AddForce(movement * power );
            updateBounce();
        }

        if (bounceCount == 0)
        {
            if (dieTimer <= 0)
                updateBounce();
            else
                dieTimer -= Time.deltaTime;
        }

        if (ifFriction)
            sr.sprite = frictionSprite;
        else
            sr.sprite = normalSprite;

    }

    void FixedUpdate()
    {
        if (isWin)
        {
            if (winTimer <= 0)
            {
                Scene currScene = SceneManager.GetActiveScene();
                if (currScene.buildIndex < LAST_LEVEL) // not last scene yet
                    SceneManager.LoadScene(currScene.buildIndex + 1, LoadSceneMode.Single);
            }
            else
            {
                sr.sprite = winSprite;
                winTimer -= Time.deltaTime;
            }
            
        } else if (isDead) {
            sr.sprite = deadSprite;
            promptImg.texture = gameOverImg;
            promptImg.gameObject.SetActive(true);
			menuButton.gameObject.SetActive (true);
			restartButton.gameObject.SetActive (true);

            
        } else {
            
		    if (ifSlippery == true) {
			    rb.drag = 0.01f;
		    }
		    else if (ifFriction == true) {
			    rb.drag = 3;
                
		    } else if (ifSlippery == false && ifFriction == false){
			    rb.drag = 1f;
		    }

		    if (ifCollided == true) {
                shrinkPlayer();
                sr.sprite = bounceSprite;
                bounceTimer = 0.2f;
			    ifCollided = false;
            }
           
            if (bounceTimer <= 0) {
                sr.sprite = normalSprite;
            }
            else
                bounceTimer -= Time.deltaTime;
           

            
            if (startTimer <= 2 && startTimer > 0)
            {
                promptImg.texture = goImg;
            }
            else if (startTimer <= 0)
            {
                promptImg.gameObject.SetActive(false);
            }

            if (smokeTimer <= 0)
            {
                smokeRenderer.sprite = null;
            }

            startTimer -= Time.deltaTime;
            smokeTimer -= Time.deltaTime;

        }
    }

    void OnGUI()

    {
        for (int i = 0; i < bounceCount; i++)
        {
            GUI.DrawTexture(new Rect(screenCorner.x + 10 + 30 * i, screenCorner.y - 60, 20, 20), lifeIcon);
        }
        
    }

    void updateBounce() {
        bounceCount--;
		if (isDead)
			return;
        if (bounceCount < 0) {
            bounceText.text = NO_BOUNCE_STR;
            isDead = true;
			playSound (defeatedSound);
        } else {
            bounceText.text = BOUNCE_STR + bounceCount;
        }
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
		if (other.gameObject.CompareTag ("Friction")) {
			ifFriction = true;
			ifSlippery = false;
			playSound (frictionSound);
		} else if (other.gameObject.CompareTag ("NonFriction")) {
			ifFriction = false;
			ifSlippery = false;
		} else if (other.gameObject.CompareTag ("Slippery")) {
			ifSlippery = true;
			playSound (slipSound);
        } else if (other.gameObject.CompareTag("OpenSesame")) {
            other.gameObject.SetActive(false);
            GameObject.FindGameObjectWithTag("Gate").SetActive(false);
			playSound (openDoorSound);
        } 
        else if (other.gameObject.CompareTag("AddBounce"))
        {
            other.gameObject.SetActive(false);
            bounceCount += bounceAdd;
            bounceText.text = BOUNCE_STR + bounceCount;
			playSound (powerSound);
        }
        else if (other.gameObject.CompareTag("Pickup"))
        {
            if (isShield)
            {
                isShield = false;
                shieldRenderer.sprite = null;
				playSound (powerSound);
            }
            else { 
                other.gameObject.SetActive(false);
                tf = GetComponent<Transform>();
				if (tf.localScale.x < 2.5) {
					tf.localScale *= growRatio;
				}
                smokeRenderer.sprite = smokeSprite;
                smokeTimer = 1;
				playSound (meetMonsterSound);
            }
        }
        else if (other.gameObject.CompareTag("Getthin"))
        {
            other.gameObject.SetActive(false);
            tf = GetComponent<Transform>();
            tf.localScale /= pShrinkRatio;
            smokeRenderer.sprite = smokeSprite;
            smokeTimer = 1;
			playSound (getthinSound);
        }
        else if (other.gameObject.CompareTag("Shield"))
        {
            other.gameObject.SetActive(false);
            shieldRenderer.sprite = shieldSprite;
            isShield = true; 
			playSound (powerSound);
        }
        else if (other.gameObject.CompareTag("Goal"))
        {
            isWin = true;
			playSound (victorySound);
        }
        else if (other.gameObject.CompareTag("Star"))
        {
            other.gameObject.SetActive(false);
			playSound (getStar);
            updateStar();
        }

    }

    void updateStar()
    {
        starCount++;
        if (starCount >= 5)
        {
            bounceCount++;
            bounceText.text = BOUNCE_STR + bounceCount;
            starCount -= 5;
        }
        
        starBar.value = starCount;
    }

    void shrinkPlayer()
    {
        tf = GetComponent<Transform>();
        Vector2 baseSize2D = new Vector2(baseSize, baseSize);
        tf.localScale = shrinkRatio * ((Vector2)tf.localScale - baseSize2D) + baseSize2D;
       
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Collider2D other = collision.collider;
		if (other.gameObject.CompareTag ("Wall")) {
			ifCollided = true;
			playSound(bounceSound);
		} else if (other.gameObject.CompareTag ("WallNoShrink")) {
			playSound(bounceSound);
		} else if (other.gameObject.CompareTag ("Gate")) {
			playSound(bounceSound);
		}

    }

	void playSound(AudioClip audio)
	{
		AudioSource.PlayClipAtPoint(audio,transform.position,1.0f);
	}

}

