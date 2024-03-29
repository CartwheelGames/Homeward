﻿using UnityEngine;
using System;
using System.Collections;

public class Player : MonoBehaviour
{
	public State state = State.IDLE;
	public KeyCode leftInput, rightInput, upInput, downInput;
	public SoundManager soundManager;
	public int score = 0;
	public float flapPower = 10;
	public float divePower = 10;
	public float flyingPower = 1;
	public float walkingPower = 1;
	public float stunBouncePower = 10;
	public float flapCoolDown = 0.1f;
	public float stunnedDuration = 1;
	public bool isFacingLeft;
	public Sprite flapSprite1, flapSprite2, stunnedSprite, groundedSprite;
	public int levelExtents = 7;
	public event Action OnScoreChange;
	public GameObject winElements;
	public Transform nest;
	public Transform beakObject;
	public TextMesh nameLabel;
	public float nameLabelFadeDelay = 3f;
	public SpriteRenderer localRenderer;
	private Rigidbody2D localRigidBody;
	private NestPiece currentNestPiece;
	private float stunEndTime;
	private float flapEndTime;
	private bool isGrounded;

	public enum State
	{
		IDLE, FLYING, STUNNED
	};

	public void ShowWinElements()
	{
		winElements.SetActive(true);
	}

	private void Start()
	{
		localRigidBody = GetComponent<Rigidbody2D>();

		Flip(isFacingLeft);

		if (nameLabel != null)
		{
			StartCoroutine(FadeNameLabel());
		}
	}

	private IEnumerator FadeNameLabel()
	{
		yield return new WaitForSeconds(nameLabelFadeDelay);
		Color color = nameLabel.color;
		while (nameLabel.color.a > 0)
		{
			float alpha = nameLabel.color.a - Time.deltaTime;
			nameLabel.color = new Color(color.r, color.g, color.b, alpha);
			yield return null;
		}
	}

	private void Update()
	{
		switch (state)
		{
			case State.IDLE:
				WalkUpdate();
				break;

			case State.FLYING:
				FlyUpdate();
				break;

			case State.STUNNED:
				if (Time.time > stunEndTime)
				{
					StopStun();
				}
				break;
		}

		WrapWalls();
	}

	private void WalkUpdate()
	{
		bool isPressingLeft = Input.GetKey(leftInput);
		bool isPressingRight = Input.GetKey(rightInput);

		if ((isPressingLeft && isFacingLeft) || (isPressingRight && !isFacingLeft))
		{
			MoveForward();
		}

		if (Input.GetKey(upInput))
		{
			SetState(State.FLYING);
		}
	}

	private void FlyUpdate()
	{
		if (Time.time > flapEndTime)
		{
			SetSprite(flapSprite1);
			if (Input.GetKeyDown(upInput))
			{
				Flap();
			}
			else if (Input.GetKeyDown(downInput))
			{
				Dive();
			}
		}
	}

	private void Flap()
	{
		Vector2 force = Vector2.up * flapPower;
		MoveForward();
		SetSprite(flapSprite2);
		localRigidBody.AddForce(force);
		flapEndTime = Time.time + flapCoolDown;
		soundManager.PlayFlapClip();
	}

	private void Dive()
	{
		Vector2 force = Vector2.down * divePower;

		MoveForward();

		localRigidBody.AddForce(force);

		soundManager.PlayDiveClip();
	}

	private void Idle()
	{
		SetSprite(groundedSprite);
	}

	private void SetSprite(Sprite sprite)
	{
		if (localRenderer != null && sprite != null && localRenderer.sprite != sprite)
		{
			localRenderer.sprite = sprite;
		}
	}

	private void Stun()
	{
		SetSprite(stunnedSprite);

		if (currentNestPiece != null)
		{
			currentNestPiece.IsDropped();

			currentNestPiece = null;
		}
	}

	private void StopStun()
	{
		SetSprite(flapSprite1);
		if (isGrounded)
		{
			SetState(State.IDLE);
		}
		else
		{
			SetState(State.FLYING);
		}
	}

	private void MoveForward()
	{
		float power = state == State.FLYING ? flyingPower : walkingPower;
		Vector2 force = (isFacingLeft ? Vector2.left : Vector2.right) * power;

		localRigidBody.velocity = new Vector2(0, localRigidBody.velocity.y);
		localRigidBody.AddForce(force, ForceMode2D.Force);
	}

	private void Flip(bool isLeft)
	{
		float scaleX = Mathf.Abs(localRenderer.transform.localScale.x) * (isLeft ? -1 : 1);
		float scaleY = localRenderer.transform.localScale.y;

		localRenderer.transform.localScale = new Vector3(scaleX, scaleY, 1);
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		switch (collision.collider.gameObject.tag)
		{
			case "ground":
				isGrounded = true;
				SetState(State.IDLE);
				break;
			case "Player":
				if (state != State.STUNNED)
				{
					HandlePlayerCollision(collision);
				}
				break;
		}
	}

	private void OnCollisionLeave2D(Collision2D collision)
	{
		if (isGrounded && collision.collider.gameObject.tag == "ground")
		{
			isGrounded = false;
			SetState(State.FLYING);
		}
	}

	private void OnTriggerEnter2D(Collider2D target)
	{
		if (state == State.FLYING || state == State.IDLE)
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

	private void HandlePlayerCollision(Collision2D target)
	{
		Player otherPlayer = target.collider.gameObject.GetComponent<Player>();
		if (otherPlayer != null)
		{
			Vector2 bounceVector = (localRigidBody.position - target.contacts[0].point).normalized;
			localRigidBody.AddForce(bounceVector * stunBouncePower);
			SetState(State.STUNNED);
			if (otherPlayer.state != State.STUNNED)
			{
				soundManager.PlayClashClip();
			}
		}
	}

	private void SetState(State newState)
	{
		switch (newState)
		{
			case State.IDLE:
				Idle();
				break;

			case State.FLYING:
				Flap(); // flap on state enter
				break;

			case State.STUNNED:
				Stun();
				stunEndTime = Time.time + stunnedDuration;
				break;
		}
		state = newState;
	}

	private void PickupNestPiece(Collider2D target)
	{
		NestPiece targetPiece = target.gameObject.GetComponentInParent<NestPiece>();

		if (targetPiece != null && currentNestPiece == null)
		{
			targetPiece.SetHeld(beakObject);

			currentNestPiece = targetPiece;
			if (soundManager != null)
			{
				soundManager.PlaySpawnPieceClip();
			}
		}
	}

	private void DepositNestPiece(Collider2D target)
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

			soundManager.PlayScoreClip();

			// DELETE NEST PIECE
			Destroy(currentNestPiece.gameObject);
			currentNestPiece = null;
		}
	}

	private void WrapWalls()
	{
		if (localRigidBody.position.x < -levelExtents)
		{
			localRigidBody.position = new Vector2(levelExtents - 1, localRigidBody.position.y);
		}
		else if (localRigidBody.position.x > levelExtents)
		{
			localRigidBody.position = new Vector2(-levelExtents + 1, localRigidBody.position.y);
		}
	}
}
