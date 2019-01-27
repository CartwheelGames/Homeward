using System.Collections;
using UnityEngine;
public class SoundManager : MonoBehaviour
{
	public AudioSource source;
	public AudioClip clashClip;
	public AudioClip flapClip;
	public AudioClip diveClip;
	public AudioClip winClip;
	public AudioClip scoreClip;
	public AudioClip spawnPiece;

	public void PlayClashClip()
	{
		if (clashClip != null)
		{
			source.PlayOneShot(clashClip);
		}
	}

	public void PlayFlapClip()
	{
		if (flapClip != null)
		{
			source.PlayOneShot(flapClip, 0.2f);
		}
	}

	public void PlayDiveClip()
	{
		if (diveClip != null)
		{
			source.PlayOneShot(diveClip);
		}
	}

	public void PlayWinClip()
	{
		if (winClip != null)
		{
			source.PlayOneShot(winClip);
		}
	}

	public void PlayScoreClip()
	{
		if (scoreClip != null)
		{
			source.PlayOneShot(scoreClip);
		}
	}
	
	public void PlaySpawnPieceClip()
	{
		if (scoreClip != null)
		{
			source.PlayOneShot(spawnPiece, 0.25f);
		}
	}
}
