using UnityEngine;

public class Player : MonoBehaviour
{
	public State state = State.FLYING;
	public KeyCode leftInput, rightInput, upInput, downInput;
	public float flapPower = 10;
	public float swoopPower = 10;
	public float forwardPower = 1;
	public float flapCoolDown = 0.1f;
	public float stunnedDuration = 1; 
	public Transform beakObject;
	public bool isFacingLeft;
	public SpriteRenderer localRenderer;
	private Animator localAnimator;
	private Rigidbody2D localRigidBody;
	private NestPiece currentNestPiece;
	private float stunEndTime;

	public enum State
	{
		IDLE, WALKING, FLYING, STUNNED
	};

	private void Start () 
	{
		localRigidBody = GetComponent<Rigidbody2D>();

		Flip(isFacingLeft);
	}
	
	private void Update()
	{
		switch (state)
		{
			case State.IDLE:
			case State.FLYING:
				ApplyInput();
				break;
			case State.STUNNED:
				if (Time.time > stunEndTime)
				{
					StopStun();
				}
				break;
		}
	}

	private void ApplyInput ()
	{
		bool isPressingLeft = Input.GetKey(leftInput);
		bool isPressingRight = Input.GetKey(rightInput);

		if (isPressingLeft || isPressingRight)
		{
			MoveForward();

			if ((isPressingLeft && !isFacingLeft) || (isPressingRight && isFacingLeft))
			{
				isFacingLeft = !isFacingLeft;
				Flip(isFacingLeft);
			}
		}
		               
		if (Input.GetKeyDown(upInput))
		{
			Flap();
		}
		else if (Input.GetKeyDown(downInput))
		{
			Swoop();
		}
	}

	private void ApplyFlapInput()
	{
		
	}

	private void Flap ()
	{
		Vector2 force = Vector2.up * flapPower;

		localRigidBody.AddForce(force);
	}

	private void Swoop ()
	{
		Vector2 force = Vector2.down * swoopPower;

		localRigidBody.AddForce(force);
	}

	private void Idle ()
	{
		// SWITCH TO IDLE ANIMATION
	}

	private void Stun ()
	{
		// SWITCH TO STUN ANIMATION

		if (currentNestPiece != null)
		{
			currentNestPiece.IsDropped();

			currentNestPiece = null;
		}
	}

	private void StopStun()
	{
		// SWITCH TO FLAP ANIMATION

		SetState(State.FLYING);
	}

	private void MoveForward ()
	{
		Vector2 force = (isFacingLeft ? Vector2.left : Vector2.right) * forwardPower;

		localRigidBody.AddForce(force, ForceMode2D.Force);
	}

	private void Flip (bool isLeft)
	{
		float newScaleX = Mathf.Abs(localRenderer.transform.localScale.x) * (isLeft ? -1 : 1);
		localRenderer.transform.localScale = new Vector2(newScaleX, localRenderer.transform.localScale.y);
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (state == State.FLYING || state == State.IDLE)
		{
			switch (collision.collider.gameObject.tag)
			{
				case "ground":
					SetState(State.IDLE);
					break;
				case "player":
					HandlePlayerCollision(collision);
					break;
				case "nestPiece":
					PickupNestPiece(collision);
					break;
			}
		}
	}

	private void HandlePlayerCollision (Collision2D collision)
	{
		Player otherPlayer = collision.collider.gameObject.GetComponent<Player>();

		if (otherPlayer != null 
		    && otherPlayer.state == State.FLYING 
		    && state == State.FLYING
		    && otherPlayer.transform.position.y > transform.position.y)
		{
			SetState(State.STUNNED);
		}
	}

	private void SetState (State newState)
	{
		switch (newState)
		{
			case State.IDLE:
				Idle();
				break;
			case State.STUNNED:
				Stun();
				stunEndTime = Time.time + stunnedDuration;
				break;
		}

		state = newState;
	}

	private void PickupNestPiece (Collision2D collision)
	{
		NestPiece targetPiece = collision.collider.gameObject.GetComponent<NestPiece>();
		Debug.Log(targetPiece);
		if (targetPiece != null && currentNestPiece == null)
		{
			targetPiece.SetHeld(beakObject);

			currentNestPiece = targetPiece;
		}
	}
}
