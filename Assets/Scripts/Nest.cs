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
		int spriteCount = progressSprites.Length;
		if (spriteCount > 0)
		{
			int index = progress < Mathf.Epsilon ? 0 : Mathf.FloorToInt(spriteCount * progress);
			if (index < spriteCount)
			{
				spriteRenderer.sprite = progressSprites[index];
			}
			else
			{
				spriteRenderer.sprite = progressSprites[spriteCount - 1];
			}
		}
	}
}
