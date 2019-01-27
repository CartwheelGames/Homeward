using UnityEngine;
using System;
using System.Collections;

public class MatchManager : MonoBehaviour
{
	[Range(1, 10)]
	public int maxScore = 1;
	public Player playerOne;
	public Player playerTwo;
	public float timeOnEndScreen = 5f;
	public event Action<int, int> OnNewScore;
	public event Action<int> OnPlayerWin;
	private bool isWinnerDeclared;
	private GameManager gameManager;

	private void Awake()
	{
		gameManager = FindObjectOfType<GameManager>();
		if (playerOne != null)
		{
			playerOne.OnScoreChange += OnPlayerOneScored;
		}
		if (playerTwo != null)
		{
			playerTwo.OnScoreChange += OnPlayerTwoScored;
		}
	}

	private void OnPlayerOneScored()
	{
		OnPlayerScored(playerOne);
	}

	private void OnPlayerTwoScored()
	{
		OnPlayerScored(playerTwo);
	}

	private void OnPlayerScored(Player player)
	{
		// UPDATE NEST VISUALS
		float progress = (float)player.score / maxScore;
		player.nest.GetComponent<Nest>().SetProgress(progress);

		int playerOneScore = playerOne != null ? playerOne.score : 0;
		int playerTwoScore = playerTwo != null ? playerTwo.score : 0;
		if (OnNewScore != null)
		{
			OnNewScore(playerOneScore, playerTwoScore);
		}
		if (!isWinnerDeclared && playerOneScore >= maxScore || playerTwoScore >= maxScore)
		{
			if (OnPlayerWin != null)
			{
				int winnerIndex = playerOneScore > playerTwoScore ? 1 : 2;
				OnPlayerWin(winnerIndex);
			}
			StartCoroutine(TransitionToEndMatch());
			isWinnerDeclared = true;
		}
	}

	private IEnumerator TransitionToEndMatch()
	{
		yield return new WaitForSeconds(timeOnEndScreen);
		if (gameManager != null)
		{
			gameManager.EndMatch();
		}
	}
}
