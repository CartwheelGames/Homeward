using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuUI : MonoBehaviour
{
	public GameManager gameManager;
	public Button startButton;
	public Button soundButton;
	public Text soundLabel;
	public Button quitButton;

	private void Awake()
	{
		gameManager.OnMatchBeginEvent += Disable;
		gameManager.OnMatchEndEvent += Enable;
	}

	private void OnDestroy()
	{
		gameManager.OnMatchBeginEvent -= Disable;
		gameManager.OnMatchEndEvent -= Enable;
	}

	private void Start()
	{
		startButton.onClick.AddListener(BeginGame);
		soundButton.onClick.AddListener(ChangeSound);
		quitButton.onClick.AddListener(Application.Quit);
		RefreshSoundLabel();
	}

	private void BeginGame()
	{
		gameManager.BeginMatch();
	}

	private void ChangeSound()
	{
		AudioListener.pause = !AudioListener.pause;
		RefreshSoundLabel();
	}

	private void RefreshSoundLabel()
	{
		soundLabel.text = AudioListener.pause ? "SOUND OFF" : "SOUND ON";
	}

	private void Enable()
	{
		gameObject.SetActive(true);
	}

	private void Disable()
	{
		gameObject.SetActive(false);
	}
}
