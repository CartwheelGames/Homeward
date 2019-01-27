using UnityEngine;

public class NestPieceSpawner : MonoBehaviour 
{
	[Range(1, 10)] public int maxOnScreen = 1;
	public GameObject nestPiecePrefab;
	public const float spawnWidth = 5;
	public float spawnIntervalMinimum = 1;
	public float spawnIntervalRandom = 1;
	private int count = 0;
	private float spawnTimer = 0;
	  
	private void Update ()
	{
		if (spawnTimer <= 0 && count < maxOnScreen)
		{
			Spawn();
			spawnTimer = spawnIntervalMinimum + (Random.value * spawnIntervalRandom);
		}
		spawnTimer -= Time.deltaTime;
	}

	private void Spawn()
	{
		GameObject newInstance = Instantiate(nestPiecePrefab);

		NestPiece nestPiece = newInstance.GetComponent<NestPiece>();

		if (nestPiece != null)
		{
			// RANDOMIZE POSITION
			float newX = Random.value * spawnWidth  - (spawnWidth/2);
			float newY = nestPiece.transform.position.y;
			float rotation = Random.value * 360f;

			nestPiece.transform.position = new Vector2(newX, newY);
			nestPiece.transform.eulerAngles = new Vector3(0, 0, rotation);
			nestPiece.GetComponent<Rigidbody2D>().AddTorque(2);

			// INCREMENT COUNT
			count++;


			// DECREMENT COUNT ON REMOVE
			nestPiece.OnRemoved += () => count--;
		}
	}
}
