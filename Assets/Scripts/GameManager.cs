using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	private bool isInMatch;
	public event Action OnMatchBeginEvent;
	public event Action OnMatchEndEvent;

	public void BeginMatch()
	{
		if (OnMatchBeginEvent != null)
		{
			OnMatchBeginEvent();
		}
		SceneManager.LoadScene(1, LoadSceneMode.Additive);
		isInMatch = true;
	}

	public void EndMatch()
	{
		if (OnMatchEndEvent != null)
		{
			OnMatchEndEvent();
		}
		SceneManager.UnloadSceneAsync(1);
		isInMatch = false;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (isInMatch)
			{
				EndMatch();
			}
			else
			{
				Application.Quit();
			}
		}
	}
}
