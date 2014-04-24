using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public Board board;
	public Player player;

	public int enemyAttack = 10;

	// Use this for initialization
	void Start () {
		//board = GetComponent<Board> ();
		//player = GetComponent<Player> ();
		NotificationCenter.DefaultCenter.AddObserver(this, "TimerTrigger");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void TimerTrigger()
	{
		player.applyDamage(board.numEnemies() * enemyAttack);
	}
}
