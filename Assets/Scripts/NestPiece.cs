using UnityEngine;
using System;

public class NestPiece : MonoBehaviour
{

	public bool isHeld { get; private set; }
	public Collider2D localTriggerCollider;
	public event Action OnRemoved;
	private Collider2D localCollider;
	private Rigidbody2D localRigidBody;

	private void Start()
	{
		localCollider = GetComponent<Collider2D>();
		localRigidBody = GetComponent<Rigidbody2D>();
	}

	private void OnDestroy()
	{
		if (OnRemoved != null)
		{
			OnRemoved();
		}
	}

	public void SetHeld(Transform holder)
	{
		isHeld = true;

		// PARENT TO BEAK
		transform.SetParent(holder);
		transform.localPosition = Vector2.zero;

		// REMOVE COLLIDER
		localCollider.enabled = false;
		localTriggerCollider.enabled = false;

		// REMOVE PHYSICS
		localRigidBody.isKinematic = true;
		localRigidBody.velocity = Vector2.zero;
	}

	public void IsDropped()
	{
		isHeld = false;

		// REMOVE FROM BEAK
		transform.SetParent(null);

		// ADD COLLIDER
		localCollider.enabled = true;
		localTriggerCollider.enabled = true;

		// ADD PHYSICS
		localRigidBody.isKinematic = false;
	}
}
