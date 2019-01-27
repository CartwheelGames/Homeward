using System.Collections;
using UnityEngine;
public class SoundManager : MonoBehaviour
{
	private static SoundManager instance;
	public AudioSource source;
	public AudioClip clashClip;
	public AudioClip flapClip;
	public AudioClip diveClip;
	public AudioClip winClip;
	public void Awake()
	{
		instance = this;
	}
	public static void PlayClashClip()
	{
		instance.source.PlayOneShot(instance.clashClip);
	}
	public static void PlayFlapClip()
	{
		instance.source.PlayOneShot(instance.flapClip);
	}
	public static void PlayDiveClip()
	{
		instance.source.PlayOneShot(instance.diveClip);
	}
	public static void PlayWinClip()
	{
		instance.source.PlayOneShot(instance.winClip);
	}
}
