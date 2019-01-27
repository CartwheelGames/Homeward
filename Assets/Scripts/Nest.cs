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
			int index = progress <= Mathf.Epsilon ? 0 : Mathf.FloorToInt(progressSprites.Length * progress);

			spriteRenderer.sprite = progressSprites[index];
		}
	}
}
