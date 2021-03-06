using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour 
{
    public GameObject Bite;
    public GameObject Bark;

	public float speed = 6f;
    public float barkSpd = 700f;
    bool ableToBark = true;
    bool ableToBite = true;
    float lastBark;
    float lastBite;
	public float barkDelay = .5f;
	public float biteDelay = .5f;
	float movement;
    public float distanceFromAtk = 1.5f;
    public Vector2 biteScale = new Vector2(1f,1f);
    public float barkScale = .2f;

	Color normalColor;
	Rigidbody2D rigidBody;
	public Vector2 currentPos,projectedPos,prevPos;
	BoxCollider2D boxCollider;
	RaycastHit2D hit;

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
	}

	void Update () 
	{
        // Can't move and animations won't play if paused or upgrading
		if (!GameManager.isPaused() && !GameManager.isUpgrade() && !GameManager.isStory() && !GameManager.hasWon())
        {
            // Doesn't move until button is pressed
            playerMove = false;
            //projectedPos.Set(currentPos.x,currentPos.y);
            if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) && (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)))
            {
                direction = 7;
                Move(0,true); // Move UP
                Move(3,true); // Move LEFT
                Rotate(30f);
            }
            else if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) && (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)))
            {
                direction = 4;
                Move(0,true); // Move UP
                Move(1,true); // Move RIGHT
                Rotate(-30f);
            }
            else if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) && (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)))
            {
                direction = 5;
                Move(2,true); // Move DOWN
                Move(1,true); // Move RIGHT
                Rotate(30f);
            }
            else if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) && (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)))
            {
                direction = 6;
                Move(2,true); // Move DOWN
                Move(3,true); // Move LEFT
                Rotate(-30f);
            }
            else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                // Flips the sprite to face Left
                if (renderer.flipX)
                    renderer.flipX = false;
                // If the player is facing Left
                direction = 3;
                Move(direction,false);
                // Reset Rotation
                Rotate(0f);
            }
            else if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            {
                // If the player is facing Up
                direction = 0;
                Move(direction,false);
                // Reset Rotation
                Rotate(0f);
            }
            else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            {
                // If the player is facing Down
                direction = 2;
                Move(direction,false);
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
                Move(direction,false);
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
	public bool didPlayerMove()
	{
		if (prevPos == null)
			prevPos = currentPos;
		else if (prevPos == currentPos)
			return false;
		return true;
	}
	void Rotate(float angle) 
	{
		rotat.z = angle;
		playerTransform.rotation = Quaternion.Euler(rotat);
	}

	void Move(int dir,bool diag)
	{
        if (!diag)
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

            hit = Physics2D.Linecast(currentPos, projectedPos);
            if (hit.transform == null)
            {
                currentPos.Set(projectedPos.x, projectedPos.y);
                rigidBody.MovePosition(currentPos);
            }
            projectedPos.Set(currentPos.x, currentPos.y);
        }
        else
        {
            playerMove = true;
            movement = speed * Time.deltaTime;

            if (dir == 3)
                projectedPos += (Vector2.left * movement) * (Mathf.Sqrt(2)/2);
            if (dir == 1)
                projectedPos += (Vector2.right * movement) * (Mathf.Sqrt(2) / 2);
            if (dir == 0)
                projectedPos += (Vector2.up * movement) * (Mathf.Sqrt(2) / 2);
            if (dir == 2)
                projectedPos += (Vector2.down * movement) * (Mathf.Sqrt(2) / 2);

            hit = Physics2D.Linecast(currentPos, projectedPos);
            if (hit.transform == null)
            {
                currentPos.Set(projectedPos.x, projectedPos.y);
                rigidBody.MovePosition(currentPos);
            }
            projectedPos.Set(currentPos.x, currentPos.y);
        }
	}

	void BiteMake(int dir)
	{
        GameObject bite;
        if (dir < 4)
        {
            bite = (GameObject)Instantiate(Bite, new Vector3(currentPos.x + distanceFromAtk * Mathf.Sin(dir * Mathf.PI / 2f), currentPos.y + distanceFromAtk * Mathf.Cos(dir * Mathf.PI / 2f), -5f), new Quaternion(0f, 0f, 0f ,0f));
            bite.GetComponent<Transform>().localScale = new Vector3(biteScale.x, biteScale.y, 0f);
            bite.transform.parent = gameObject.transform;
            bite.GetComponent<Transform>().Rotate(new Vector3(0f, 0f, dir * -90f));
        }
        else
        {
            bite = (GameObject)Instantiate(Bite, new Vector3(currentPos.x + distanceFromAtk * Mathf.Sin(((dir - 4) * (Mathf.PI / 2f)) + (Mathf.PI / 4)), currentPos.y + distanceFromAtk * Mathf.Cos(((dir - 4) * (Mathf.PI / 2f)) + (Mathf.PI / 4f)), -5f), new Quaternion(0f, 0f, 0f, 0f));
            bite.GetComponent<Transform>().localScale = new Vector3(biteScale.x, biteScale.y, 0f);
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
            bark.GetComponent<Transform>().localScale = new Vector3(barkScale, .2f, 1f);
            if (dir != 2)
                bark.GetComponent<Transform>().Rotate(new Vector3(0f, 0f, dir * 90f));
            else
                bark.GetComponent<Transform>().Rotate(new Vector3(0f, 0f, 0f));
            gameObject.GetComponent<AudioSource>().Play();
        }
        else
        {
            bark = (GameObject)Instantiate(Bark, new Vector3(currentPos.x + 2 * Mathf.Sin((dir - 4) * Mathf.PI / 2f + Mathf.PI / 4), currentPos.y + 2 * Mathf.Cos((dir - 4) * Mathf.PI / 2f + Mathf.PI / 4f), -5f), new Quaternion(0f, 0f, (dir * -Mathf.PI / 6), 0f));
            bark.GetComponent<Rigidbody2D>().AddForce(new Vector2(barkSpd * Mathf.Sin((dir - 4) * Mathf.PI / 2f + Mathf.PI / 4), barkSpd * Mathf.Cos((dir - 4) * Mathf.PI / 2f + Mathf.PI / 4)));
            bark.GetComponent<Transform>().localScale = new Vector3(barkScale, .2f, 1f);
            if (dir == 4 || dir == 6)
                bark.GetComponent<Transform>().Rotate(new Vector3(0f, 0f, (dir - 5) * 90f + 225f));
            else
                bark.GetComponent<Transform>().Rotate(new Vector3(0f, 0f, (dir - 5) * 90f + 45f));
            gameObject.GetComponent<AudioSource>().Play();
        }

        
    }

	public void ResetPlayerStats()
	{
		speed = 5f;
		barkSpd = 700f;
		barkDelay = .5f;
		biteDelay = .5f;
		distanceFromAtk = 1.5f;
		biteScale = new Vector2(1f,1f);
		barkScale = .2f;
	}

    public void HurtPlayer(int amt)
	{
		animator.SetTrigger ("playerHit");

		StartCoroutine (ChangeColor());

		GameManager.instance.playerStats.HurtPlayer (amt);
	}

	IEnumerator ChangeColor() 
	{
		renderer.color = new Color (237/255.0f, 95/255.0f, 85/255.0f);
		yield return new WaitForSecondsRealtime(0.2f);
		renderer.color = normalColor;
	}

	public void SlowdownPlayer(float time) 
	{
		StartCoroutine (Slowdown(time));
	}

	IEnumerator Slowdown(float time) 
	{
		speed /= 2;
		yield return new WaitForSecondsRealtime(time);
		speed *= 2;
	}

	public Vector2 getPosition ()
	{
		return transform.position;
	}
}