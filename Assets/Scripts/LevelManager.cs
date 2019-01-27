using UnityEngine;

public class LevelManager : MonoBehaviour 
{
	
	[System.Serializable]
	public class PlayerInfo 
	{
		public int score;
		public Player player;
		public Nest nest;
	}

	public PlayerInfo[] players;

	private void Start () 
	{
		//for (int i = 0; i < players.Length; i++)
		//{
		//	if (players[i].nest != null)
		//	{
		//		players[i].nest.onScore += () => OnScore(i);
		//	}
		//}
	}
	
	private void Update () {
		
	}

	private void OnScore (int playerIndex)
	{
		//PlayerInfo player = players[playerIndex];
		//if (player != null && player.nest != null)
		//{
		//	player.score++;
		//}
	}
}
