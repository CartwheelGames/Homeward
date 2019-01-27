using UnityEngine;

public class NPC : MonoBehaviour
{
	public Rigidbody2D localRigidBody;
	public float hopPower = 10f;
	public float minActionDelay = 1f;
	public float maxActionDelay = 2f;
	private float nextActionTime = 0f;
	private bool isFacingLeft { get { return transform.localScale.x < Mathf.Epsilon; } }

	private void Update()
	{
		if (Time.time > nextActionTime)
		{
			if (Random.value > 0.5f)
			{
				transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
			}
			else if (localRigidBody != null)
			{
				Vector2 force = (isFacingLeft ? new Vector2(-1, 1) : Vector2.one) * hopPower;
				localRigidBody.AddForce(force);
			}
			nextActionTime = Random.Range(minActionDelay, maxActionDelay) + Time.time;
		}
	}
}
