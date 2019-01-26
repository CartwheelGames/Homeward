using UnityEngine;

public class Player : MonoBehaviour
{
	public State state { get; private set;  }
	public KeyCode leftInput, rightInput, upInput, downInput;
	public float flapPower = 10;
	public float swoopPower = 10;
	public float forwardPower = 1;
	public float flapCoolDown = 0.1f;
	public float stunnedDuration = 1; 
	public Transform beakObject;
	private Animator localAnimator;
	public bool isFacingLeft;
	public SpriteRenderer localRenderer;
	private Rigidbody2D localRigidBody;
	private float stunEndTime;
	private NestPiece currentNestPiece;

	public enum State
	{
		IDLE, MOVING, STUNNED
	};

	private void Start () 
	{
		localRigidBody = GetComponent<Rigidbody2D>();
	}
	
	private void Update()
	{
		switch (state)
		{
			case State.IDLE:
				break;
			case State.MOVING:
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
		if (Input.GetKey(leftInput))
		{
			MoveForward();

			if (!isFacingLeft)
			{
				// FLIP ANIMATION LEFT

				isFacingLeft = true;
			}
		}
		else if (Input.GetKey(rightInput))
		{
			MoveForward();

			if (isFacingLeft)
			{
				// FLIP ANIMATION RIGHT

				isFacingLeft = false;
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

		SetState(State.MOVING);
	}

	private void MoveForward ()
	{
		Vector2 force = (isFacingLeft ? Vector2.left : Vector2.right) * forwardPower;

		localRigidBody.AddForce(force, ForceMode2D.Force);
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (state == State.MOVING)
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

	private void HandlePlayerCollision (Collision collision)
	{
		Player otherPlayer = collision.collider.gameObject.GetComponent<Player>();

		if (otherPlayer.transform.position.y > transform.position.y)
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

	private void PickupNestPiece (Collision collision)
	{
		NestPiece targetPiece = collision.collider.gameObject.GetComponent<NestPiece>();

		if (targetPiece != null && currentNestPiece == null)
		{
			targetPiece.SetHeld(beakObject);

			currentNestPiece = targetPiece;
		}
	}
}
