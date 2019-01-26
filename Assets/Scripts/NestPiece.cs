using UnityEngine;

public class NestPiece : MonoBehaviour
{

	public bool isHeld { get; private set; }
	private Collider2D localCollider;
	private Rigidbody2D localRigidBody;

	private void Start()
	{
		localCollider = GetComponent<Collider2D>();
		localRigidBody = GetComponent<Rigidbody2D>();
	}

	public void SetHeld (Transform holder)
	{
		isHeld = true;

		// PARENT TO BEAK
		transform.SetParent(holder);
		transform.localPosition = Vector2.zero;

		// REMOVE COLLIDER
		localCollider.enabled = false;

		// REMOVE PHYSICS
		localRigidBody.isKinematic = true;
		localRigidBody.velocity = Vector2.zero;
	}

	public void IsDropped ()
	{
		isHeld = false;

		// REMOVE FROM BEAK
		transform.SetParent(null);

		// ADD COLLIDER
		localCollider.enabled = true;

		// ADD PHYSICS
		localRigidBody.isKinematic = false;
	}
}
