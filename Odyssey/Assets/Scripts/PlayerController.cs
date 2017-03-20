using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour 
{
    public GameObject Bite;
    public GameObject Bark;

	public float speed = 6f;
    public float barkSpd =1f;
    bool ableToBark = true;
    bool ableToBite = true;
    float lastBark;
    float lastBite;
	public float barkDelay = .5f;
	public float biteDelay = .5f;
	float movement;
    float distanceFromAtk = 1.5f;

	Color normalColor;
	Rigidbody2D rigidBody;
	Vector2 currentPos,projectedPos;
	BoxCollider2D boxCollider;
	RaycastHit2D hit;

	public Text healthText;
	[HideInInspector]
	public int health;

	SpriteRenderer renderer;
	Animator animator;
	Transform playerTransform;

	/*
	 * Starts out facing LEFT
	 * Directions are like this:
	 * 		7	0	4
	 * 		3		1
	 * 		6	2	5
	 */
	private int direction = 2;
	private Vector3 rotat = new Vector3 (0f, 0f, 0f);
	private bool playerMove = false;

	void Start () 
	{
		boxCollider = gameObject.GetComponent<BoxCollider2D> ();
		rigidBody = gameObject.GetComponent<Rigidbody2D> ();
		renderer = gameObject.GetComponent<SpriteRenderer> ();
		animator = gameObject.GetComponent<Animator> ();
		playerTransform = gameObject.GetComponent<Transform> ();
		currentPos = playerTransform.position;
		projectedPos = currentPos;
		normalColor = renderer.color;

		health = GameManager.instance.playerHealth;
		healthText.text = "x " + health; 
	}

	void Update () 
	{
        // Can't move and animations won't play if paused
        if (!GameManager.isPaused())
        {
            // Doesn't move until button is pressed
            playerMove = false;
            //projectedPos.Set(currentPos.x,currentPos.y);
            if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) && (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)))
            {
                direction = 7;
                Move(0); // Move UP
                Move(3); // Move LEFT
                Rotate(30f);
            }
            else if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) && (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)))
            {
                direction = 4;
                Move(0); // Move UP
                Move(1); // Move RIGHT
                Rotate(-30f);
            }
            else if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) && (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)))
            {
                direction = 5;
                Move(2); // Move DOWN
                Move(1); // Move RIGHT
                Rotate(30f);
            }
            else if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) && (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)))
            {
                direction = 6;
                Move(2); // Move DOWN
                Move(3); // Move LEFT
                Rotate(-30f);
            }
            else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                // Flips the sprite to face Left
                if (renderer.flipX)
                    renderer.flipX = false;
                // If the player is facing Left
                direction = 3;
                Move(direction);
                // Reset Rotation
                Rotate(0f);
            }
            else if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            {
                // If the player is facing Up
                direction = 0;
                Move(direction);
                // Reset Rotation
                Rotate(0f);
            }
            else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            {
                // If the player is facing Down
                direction = 2;
                Move(direction);
                // Reset Rotation
                Rotate(0f);
            }
            else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                // Flips the sprite to face Right
                if (!renderer.flipX)
                    renderer.flipX = true;
                // If the player is facing Right
                direction = 1;
                Move(direction);
                // Reset Rotation
                Rotate(0f);
            }
            // If player is going left or right set that trigger (For Animation)
            if (direction == 1 || direction == 3)
                animator.SetBool("playerLR", true);
            else
                animator.SetBool("playerLR", false);

            // Sets if the player is moving (For Animation)
            animator.SetBool("playerMove", playerMove);
            animator.SetInteger("playerDir", direction);
            if (Input.GetKey(KeyCode.J) && ableToBite)
            { 
                ableToBite = false;
                BiteMake(direction);
                lastBite = Time.time;
            }
            if (Input.GetKey(KeyCode.K) && ableToBark)
            {
                ableToBark = false;
                BarkMake(direction);
                lastBark = Time.time;
            }
            if (ableToBark == false && (lastBark + barkDelay) < Time.time)
            {
                ableToBark = true;
            }
            if (ableToBite == false && (lastBite + biteDelay) < Time.time)
            {
                ableToBite = true;
            }
        }
    }

	void Rotate(float angle) 
	{
		rotat.z = angle;
		playerTransform.rotation = Quaternion.Euler(rotat);
	}

	void Move(int dir)
	{
		playerMove = true;
		movement = speed * Time.deltaTime;

		if (dir == 3)
			projectedPos += Vector2.left * movement;
		if (dir == 1)
			projectedPos += Vector2.right * movement;
		if (dir == 0)
			projectedPos += Vector2.up * movement;
		if (dir == 2)
			projectedPos += Vector2.down * movement;
		
		hit = Physics2D.Linecast (currentPos, projectedPos);
		if (hit.transform == null) {
			currentPos.Set (projectedPos.x, projectedPos.y);
			rigidBody.MovePosition (currentPos);
		} 
		projectedPos.Set(currentPos.x,currentPos.y);
	}

	void BiteMake(int dir)
	{
        GameObject bite;
        if (dir < 4)
        {
            bite = (GameObject)Instantiate(Bite, new Vector3(currentPos.x + distanceFromAtk * Mathf.Sin(dir * Mathf.PI / 2f), currentPos.y + distanceFromAtk * Mathf.Cos(dir * Mathf.PI / 2f), -5f), new Quaternion(0f, 0f, 0f ,0f));
			bite.transform.parent = gameObject.transform;
            bite.GetComponent<Transform>().Rotate(new Vector3(0f, 0f, dir * -90f));
        }
        else
        {
            bite = (GameObject)Instantiate(Bite, new Vector3(currentPos.x + distanceFromAtk * Mathf.Sin(((dir - 4) * (Mathf.PI / 2f)) + (Mathf.PI / 4)), currentPos.y + distanceFromAtk * Mathf.Cos(((dir - 4) * (Mathf.PI / 2f)) + (Mathf.PI / 4f)), -5f), new Quaternion(0f, 0f, 0f, 0f));
			bite.transform.parent = gameObject.transform;
            bite.GetComponent<Transform>().Rotate(new Vector3(0f,0f,(dir-4)*-90f - 45f ));
		}
    }

    void BarkMake(int dir)
    {
        GameObject bark;
        if (dir < 4)
        {
            bark = (GameObject)Instantiate(Bark, new Vector3(currentPos.x + 2 * Mathf.Sin(dir * Mathf.PI / 2f), currentPos.y + 2 * Mathf.Cos(dir * Mathf.PI / 2f), -5f), new Quaternion(0f, 0f, (dir * -Mathf.PI / 6), 0f));
            bark.GetComponent<Rigidbody2D>().AddForce(new Vector2 (barkSpd * Mathf.Sin(dir * Mathf.PI / 2f), barkSpd * Mathf.Cos(dir * Mathf.PI / 2f)));
        }
        else
        {
            bark = (GameObject)Instantiate(Bark, new Vector3(currentPos.x + 2 * Mathf.Sin((dir - 4) * Mathf.PI / 2f + Mathf.PI / 4), currentPos.y + 2 * Mathf.Cos((dir - 4) * Mathf.PI / 2f + Mathf.PI / 4f), -5f), new Quaternion(0f, 0f, (dir * -Mathf.PI / 6), 0f));
            bark.GetComponent<Rigidbody2D>().AddForce(new Vector2(barkSpd * Mathf.Sin((dir - 4) * Mathf.PI / 2f + Mathf.PI / 4), barkSpd * Mathf.Cos((dir - 4) * Mathf.PI / 2f + Mathf.PI / 4)));
        }
        
    }

    public void HurtPlayer(int amt)
	{
		animator.SetTrigger ("playerHit");

		StartCoroutine (ChangeColor());

		health -= amt;
		healthText.text = "x " + health;
		IsGameOver ();
	}

	IEnumerator ChangeColor() 
	{
		renderer.color = new Color (237/255.0f, 95/255.0f, 85/255.0f);
		yield return new WaitForSecondsRealtime(0.2f);
		renderer.color = normalColor;
	}

	private void IsGameOver()
	{
		if (health <= 0) 
		{
			GameManager.instance.GameOver ();
		}
	}

	public Vector2 getPosition ()
	{
		return transform.position;
	}
}