using UnityEngine;

public class NestPieceSpawner : MonoBehaviour 
{
	[Range(1, 10)] public int maxOnScreen = 1;
	public GameObject nestPiecePrefab;
	public const float spawnWidth = 5;
	public float spawnInterval = 1;
	private int numOnScreen = 0;
	private float spawnTimer = 0;
	  
	private void Update ()
	{
		if (spawnTimer <= 0 && numOnScreen < maxOnScreen)
		{
			Spawn();
			spawnTimer = spawnInterval;
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
			nestPiece.transform.position = new Vector2(newX, newY);

			// INCREMENT COUNT
			numOnScreen++;


			// DECREMENT COUNT ON REMOVE
			nestPiece.OnRemoved += () => numOnScreen--;
		}
	}
}
