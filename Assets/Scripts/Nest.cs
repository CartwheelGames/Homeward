using UnityEngine;
using System;

public class Nest : MonoBehaviour
{
	public event Action onScore;

	private void OnTriggerEnter(Collider other)
	{
		//if (other.gameObject.tag == "player")
		//{
		//	if (onScore != null)
		//	{
		//		onScore();

		//		Destroy(other.gameObject);
		//	}
		//}
	}
}
