using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayUI : MonoBehaviour
{
	public MatchManager matchManager;
	public Text winnerLabel;
	public Text scoreLabel1;
	public Text scoreLabel2;
	public CanvasGroup tutorialGroup;
	public float tutorialFadeDelay = 3f;

	private void Awake ()
	{
		matchManager.OnNewScore += RefreshScore;
		matchManager.OnPlayerWin += OnPlayerWin;
	}

	private void Start ()
	{
		if (tutorialGroup != null)
		{
			StartCoroutine(FadeTutorial());
		}
	}

	private IEnumerator FadeTutorial ()
	{
		yield return new WaitForSeconds(tutorialFadeDelay);

		while (tutorialGroup.alpha > 0)
		{
			tutorialGroup.alpha -= Time.deltaTime;

			yield return null;
		}

		Destroy(tutorialGroup.gameObject);

	}

	private void OnPlayerWin(int playerIndex)
	{
		winnerLabel.enabled = true;
		winnerLabel.text = "Player " + playerIndex + " Wins";
	}

	private void RefreshScore(int playerOneScore, int playerTwoScore)
	{
		scoreLabel1.text = playerOneScore.ToString();
		scoreLabel2.text = playerTwoScore.ToString();
	}
}
