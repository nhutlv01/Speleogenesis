using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public Board board;
	public Player player;
	public bool bPaused = false;

	public int enemyAttack = 10;

	// Use this for initialization
	void Start () {
		//board = GetComponent<Board> ();
		//player = GetComponent<Player> ();
		NotificationCenter.DefaultCenter.AddObserver(this, "TimerTrigger");
		NotificationCenter.DefaultCenter.AddObserver(this, "PlayerDeath");
		NotificationCenter.DefaultCenter.AddObserver(this, "Pause");
		NotificationCenter.DefaultCenter.AddObserver(this, "Unpause");
	}
	
	// Update is called once per frame
	void Update () {
		if(!bPaused)
		{

		}
		else
		{

		}
	}

	void TimerTrigger()
	{
		player.applyDamage(board.numEnemies() * enemyAttack);
	}

	void PlayerDeath()
	{
		NotificationCenter.DefaultCenter.PostNotification(this,"MenuEnter");
	}

	/////////////////////////////////
	//Pause
	/////////////////////////////////
	void Pause()
	{
		bPaused = true;
	}
	
	/////////////////////////////////
	//Unpause
	/////////////////////////////////
	void Unpause()
	{
		bPaused = false;
	}
}
