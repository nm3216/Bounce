using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    // public variables
    public float baseSize;
    public float shrinkRatio;
    public float growRatio;
    public float power;
    public int bounceLimit;
    public int needleSpeed;
    public Text bounceText;
    public Text winDieText;
    public Texture dirNeedle;
    

    // constants
    private const string BOUNCE_STR = "Bounces left = ";
    private const string NO_BOUNCE_STR = "No bounces left!";
    private const string DIE_STR = "YOU DIED!!";
    private const string WIN_STR = "YOU WIN! Next level >>";
    

	private bool ifFriction;
	private bool ifSlippery;
	private bool ifCollided;
    private bool isDead;
    private bool isWin;
    private Rigidbody2D rb;
	private Transform tf;
    private int angle;
    private int bounceCount;
    private Vector2 midScreen;
    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        angle = 0;
		ifFriction = false;
		ifCollided = false;
		ifSlippery = false;
        isDead = false;
        isWin = false;
        bounceCount = bounceLimit;
        bounceText.text = BOUNCE_STR + bounceCount;
        winDieText.text = "";
		Debug.Log ("started\t");
        midScreen = new Vector2(Screen.width / 2, Screen.height / 2);

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
            winDieText.text = WIN_STR;
        } else if (isDead) {
            winDieText.text = DIE_STR;
        } else {
            angle = (angle + needleSpeed) % 360;
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
			    ifCollided = false;
		    }

            if (Input.GetKeyDown("space"))
            {
                float angleRad = angle * Mathf.Deg2Rad;
                float moveHorizontal = Mathf.Sin(angleRad);
                float moveVertical = Mathf.Cos(angleRad);
                Vector2 movement = new Vector2(moveHorizontal, moveVertical);
                rb.AddForce(movement * power );
            }


        }
    }

    void UpdateBounce() {
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
        else if (other.gameObject.CompareTag("Pickup"))
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
        }
        else if (other.gameObject.CompareTag("Goal"))
        {
            isWin = true;
        }

    }

    void shrinkPlayer()
    {
        tf = GetComponent<Transform>();
        Vector2 baseSize2D = new Vector2(baseSize, baseSize);
        tf.localScale = shrinkRatio * ((Vector2)tf.localScale - baseSize2D) + baseSize2D;
        UpdateBounce();
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
