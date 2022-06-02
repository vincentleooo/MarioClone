using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;

public class PlayerController : MonoBehaviour
{
	private Animator marioAnimator;
	public float speed;
	private Rigidbody2D marioBody;
	private AudioSource marioAudio;
	// Start is called before the first frame update
	void Start()
	{
		// Set to be 30 FPS
		Application.targetFrameRate = 30;
		marioBody = GetComponent<Rigidbody2D>();
		marioSprite = GetComponent<SpriteRenderer>();
		marioAnimator = GetComponent<Animator>();
		marioAudio = GetComponent<AudioSource>();
	}

	public float maxSpeed = 10;
	public float upSpeed;
	private bool onGroundState = true;
	private SpriteRenderer marioSprite;
	private bool faceRightState = true;

	// called when the cube hits the floor
	void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject.CompareTag("Ground") || col.gameObject.CompareTag("Obstacles"))
		{
			onGroundState = true; // back on ground
			countScoreState = false; // reset score state
			scoreText.text = "Score: " + score.ToString();
		};
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.CompareTag("Enemy"))
		{
			Time.timeScale = 0.0f;
			score = 0;
			// Debug.Log("Collided with Gomba!");
			Vector2 enemyPosition = new Vector2(2.5f, -0.46f);
			Vector2 marioPosition = new Vector2(0.0f, 0.0f);
			marioBody.transform.position = marioPosition;
			enemyLocation.transform.position = enemyPosition;
			Thread.Sleep(500);
			marioBody.velocity = Vector2.zero;
		}
	}

	public Transform enemyLocation;
	public Text scoreText;
	private int score = 0;
	private bool countScoreState = false;

	void Update()
	{
		// Skidding seems to not work well with GetAxisRaw as the acceleration is a bit different. With GetAxis, it works as well as expected.
		// toggle state
		if (Input.GetKeyDown("a") && faceRightState)
		{
			faceRightState = false;
			marioSprite.flipX = true;
			if (Mathf.Abs(marioBody.velocity.x) > 1.0)
				marioAnimator.SetTrigger("onSkid");
		}

		if (Input.GetKeyDown("d") && !faceRightState)
		{
			faceRightState = true;
			marioSprite.flipX = false;
			if (Mathf.Abs(marioBody.velocity.x) > 1.0)
				marioAnimator.SetTrigger("onSkid");
		}
		// when jumping, and Gomba is near Mario and we haven't registered our score
		if (!onGroundState && countScoreState)
		{
			if (Mathf.Abs(transform.position.x - enemyLocation.position.x) < 0.5f)
			{
				countScoreState = false;
				score++;
				// Debug.Log(score);
			}
		}
		marioAnimator.SetFloat("xSpeed", Mathf.Abs(marioBody.velocity.x));
		marioAnimator.SetBool("onGround", onGroundState);
		// Debug.Log(Mathf.Abs(marioBody.velocity.x));
	}

	void FixedUpdate()
	{
		// Debugging
		// Debug.Log(marioBody.velocity);
		// Debug.Log("no space");

		// Dynamic Rigidbody2D
		float moveHorizontal = Input.GetAxis("Horizontal");
		// GetAxisRaw bypasses GetAxis smoothing when taking fingers of key press. 
		// https://stackoverflow.com/questions/58914962/rigidbody-not-stopping-instantly-when-setting-its-velocity-to-0

		if (Mathf.Abs(moveHorizontal) > 0f && Time.timeScale != 0.0f)
		{
			marioBody.WakeUp();
			Vector2 movement = new Vector2(moveHorizontal, 0);
			if (marioBody.velocity.magnitude < maxSpeed)
			{
				marioBody.AddForce(movement * speed * 50);
			}
		}
		if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
		{
			// Stop
			Vector2 stopHorizontal = new Vector2(0, marioBody.velocity.y);
			marioBody.velocity = stopHorizontal;
		}
		if (Input.GetKeyDown("space") && onGroundState)
		{
			marioBody.AddForce(Vector2.up * upSpeed * 2, ForceMode2D.Impulse);
			onGroundState = false;
			countScoreState = true; //check if Gomba is underneath
		}
	}

	void PlayJumpSound()
	{
		marioAudio.PlayOneShot(marioAudio.clip);
	}
}
