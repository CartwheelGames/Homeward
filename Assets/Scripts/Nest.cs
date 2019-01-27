using UnityEngine;

public class Nest : MonoBehaviour
{
	private SpriteRenderer spriteRenderer;
	[SerializeField] private Sprite[] progressSprites;

	private void Start()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();

		SetProgress(0f);
	}

	public void SetProgress (float progress) 
	{
		// SET IMAGE OF PROGRESS BASED ON PROGRESS

		if (progressSprites.Length > 0)
		{
			Debug.Log(Mathf.Epsilon);
			Debug.Log(progressSprites.Length);
			Debug.Log(progress);

			int index = progress <= Mathf.Epsilon ? 0 : Mathf.FloorToInt(progressSprites.Length * progress);

			Debug.Log(progress);
			spriteRenderer.sprite = progressSprites[index];
		}
	}
}
