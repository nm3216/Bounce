using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    // public variables
    public float baseSize;
    public float shrinkRatio;
    public float growRatio;
    public float power;
    public int bounceLimit;
    public int bounceAdd;
    public int needleSpeed;
    public Text bounceText;
    public Text starText;
    public Texture dirNeedle;
    public RawImage promptImg;
    public Texture goImg;
    public Texture gameOverImg;
    public Sprite shieldSprite;
    public Sprite bounceSprite;
    public Sprite normalSprite;
    public Sprite deadSprite;
    public Sprite smokeSprite;
    public SpriteRenderer shieldRenderer;
    public SpriteRenderer smokeRenderer;
    

    // constants
    private const string BOUNCE_STR = "Bounces left: ";
    private const string NO_BOUNCE_STR = "No bounces left!";
    private const string STAR_STR = "Stars: ";
    private const string DIE_STR = "YOU DIED!!";
    private const string WIN_STR = "YOU WIN! Next level >>";
    

	private bool ifFriction;
	private bool ifSlippery;
	private bool ifCollided;
    private bool isDead;
    private bool isShield;
    private bool isWin;
    private int angle;
    private int bounceCount;
    private int starCount;
    private Rigidbody2D rb;
	private Transform tf;
    private Vector2 midScreen;
    private SpriteRenderer sr;
    private float bounceTimer;
    private float startTimer;
    private float smokeTimer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        angle = 0;
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
		Debug.Log ("started\t");
        midScreen = new Vector2(Screen.width / 2, Screen.height / 2);
        sr = GetComponent<SpriteRenderer>();

    }

    void OnGUI()
    {
        GUIUtility.RotateAroundPivot(angle, midScreen);
        GUI.DrawTexture(new Rect(midScreen.x - 50, midScreen.y -50, 50, 50), dirNeedle);
    }

    void FixedUpdate()
    {
        if (isWin)
        {
            SceneManager.LoadScene("Level2", LoadSceneMode.Single);
        } else if (isDead) {
            sr.sprite = deadSprite;
            promptImg.texture = gameOverImg;
            promptImg.gameObject.SetActive(true);
            
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
            else
            {
                if (bounceTimer <= 0)
                    sr.sprite = normalSprite;
                else
                    bounceTimer -= Time.deltaTime;
            }

            if (Input.GetKeyDown("space"))
            {
                float angleRad = angle * Mathf.Deg2Rad;
                float moveHorizontal = Mathf.Sin(angleRad);
                float moveVertical = Mathf.Cos(angleRad);
                Vector2 movement = new Vector2(moveHorizontal, moveVertical);
                rb.AddForce(movement * power );
                updateBounce();
            }

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

            angle = (angle + needleSpeed) % 360;
            startTimer -= Time.deltaTime;
            smokeTimer -= Time.deltaTime;

        }
    }

    void updateBounce() {
        bounceCount--;
        if (bounceCount <= 0) {
            bounceText.text = NO_BOUNCE_STR;
            isDead = true;
        } else {
            bounceText.text = BOUNCE_STR + bounceCount;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
		if (other.gameObject.CompareTag ("Friction")) {
			ifFriction = true;
			ifSlippery = false;
		} else if (other.gameObject.CompareTag ("NonFriction")) {
			ifFriction = false;
			ifSlippery = false;
		} else if (other.gameObject.CompareTag ("Slippery")) {
			ifSlippery = true;
        } else if (other.gameObject.CompareTag("OpenSesame")) {
            other.gameObject.SetActive(false);
            GameObject.FindGameObjectWithTag("Gate").SetActive(false);
        } 
        else if (other.gameObject.CompareTag("AddBounce"))
        {
            other.gameObject.SetActive(false);
            bounceCount += bounceAdd;
            bounceText.text = BOUNCE_STR + bounceCount;
        }
        else if (other.gameObject.CompareTag("Pickup") && !isShield)
        {
            other.gameObject.SetActive(false);
            tf = GetComponent<Transform>();
            tf.localScale *= growRatio;
        }
        else if (other.gameObject.CompareTag("Getthin"))
        {
            other.gameObject.SetActive(false);
            tf = GetComponent<Transform>();
            tf.localScale /= growRatio;
            smokeRenderer.sprite = smokeSprite;
            smokeTimer = 1;
        }
        else if (other.gameObject.CompareTag("Shield"))
        {
            other.gameObject.SetActive(false);
            shieldRenderer.sprite = shieldSprite;
            isShield = true; 
        }
        else if (other.gameObject.CompareTag("Goal"))
        {
            isWin = true;
        }
        else if (other.gameObject.CompareTag("Star"))
        {
            other.gameObject.SetActive(false);
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
        
        starText.text = STAR_STR + starCount;
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
        if (other.gameObject.CompareTag("Wall"))
        {
			ifCollided = true;
		}
    }		

}
