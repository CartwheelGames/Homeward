using UnityEngine;
using System;

public class Player : MonoBehaviour
{
	public State state = State.FLYING;
	public KeyCode leftInput, rightInput, upInput, downInput;
	public int score = 0;
	public float flapPower = 10;
	public float swoopPower = 10;
	public float forwardPower = 1;
	public float flapCoolDown = 0.1f;
	public float stunnedDuration = 1; 
	public bool isFacingLeft;
	public event Action OnScoreChange;
	public Transform nest;
	public Transform beakObject;
	public SpriteRenderer localRenderer;
	private Animator localAnimator;
	private Rigidbody2D localRigidBody;
	private NestPiece currentNestPiece;
	private float stunEndTime;

	public enum State
	{
		GROUNDED, FLYING, STUNNED
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
			case State.GROUNDED:
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

	private void Ground ()
	{
		// SWITCH TO GROUNDED ANIMATION
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
		float scaleX = Mathf.Abs(localRenderer.transform.localScale.x) * (isLeft ? -1 : 1);
		float scaleY = localRenderer.transform.localScale.y;

		localRenderer.transform.localScale = new Vector2(scaleX, scaleY);
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (state == State.FLYING || state == State.GROUNDED)
		{
			switch (collision.collider.gameObject.tag)
			{
				case "ground":
					SetState(State.GROUNDED);
					break;
				case "player":
					HandlePlayerCollision(collision);
					break;
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D target)
	{
		if (state == State.FLYING || state == State.GROUNDED)
		{
			switch (target.gameObject.tag)
			{
				case "nestPiece":
					PickupNestPiece(target);
					break;
				case "nest":
					DepositNestPiece(target);
					break;
			}
		}
	}

	private void HandlePlayerCollision (Collision2D target)
	{
		Player otherPlayer = target.collider.gameObject.GetComponent<Player>();

		if (otherPlayer != null)
		{
			bool neitherPlayerIsStunned = otherPlayer.state != State.STUNNED && state != State.STUNNED;
			bool otherPlayerisAbove = otherPlayer.transform.position.y > transform.position.y;


			if (neitherPlayerIsStunned && otherPlayerisAbove)
			{
				SetState(State.STUNNED);
			}
		}
	}

	private void SetState (State newState)
	{
		switch (newState)
		{
			case State.GROUNDED:
				Ground();
				break;
			case State.STUNNED:
				Stun();
				stunEndTime = Time.time + stunnedDuration;
				break;
		}

		state = newState;
	}

	private void PickupNestPiece (Collider2D target)
	{
		NestPiece targetPiece = target.gameObject.GetComponentInParent<NestPiece>();

		if (targetPiece != null && currentNestPiece == null)
		{
			targetPiece.SetHeld(beakObject);

			currentNestPiece = targetPiece;
		}
	}

	private void DepositNestPiece (Collider2D target)
	{
		Transform targetNest = target.transform;

		if (targetNest != null && nest == targetNest && currentNestPiece != null)
		{
			// INCREMENT SCORE
			score++;

			// DISPATCH EVENT
			if (OnScoreChange != null)
			{
				OnScoreChange();
			}

			// DELETE NEST PIECE
			Destroy(currentNestPiece);
			currentNestPiece = null;
		}
	}
}
