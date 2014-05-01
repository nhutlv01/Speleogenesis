using UnityEngine;
using System.Collections;
using Vectrosity;

public class GameManager : MonoBehaviour {

	public Board board;
	public Player player;
	public Timer timer;
	public MenuScript menu;
	public Camera mainCamera;
	public bool bPaused = false;

	public int enemyAttack = 10;

	// Use this for initialization
	void Start () {
		//board = GetComponent<Board> ();
		//player = GetComponent<Player> ();
		NotificationCenter.DefaultCenter().AddObserver(this, "MenuEnter");
		NotificationCenter.DefaultCenter().AddObserver(this, "MenuExit");
		NotificationCenter.DefaultCenter().AddObserver(this, "TimerTrigger");
		NotificationCenter.DefaultCenter().AddObserver(this, "PlayerDeath");
		NotificationCenter.DefaultCenter().AddObserver(this, "Pause");
		NotificationCenter.DefaultCenter().AddObserver(this, "Unpause");
		VectorLine.SetCamera3D();
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
		enemyAttack = player.level * 5 + 10;
	}

	void PlayerDeath()
	{
		NotificationCenter.DefaultCenter().PostNotification(this,"MenuEnter");
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

	void MenuEnter()
	{
		NotificationCenter.DefaultCenter().PostNotification(this, "Pause");
		menu.menuEnable();
	}

	void MenuExit()
	{
		menu.menuDisable();
		NotificationCenter.DefaultCenter().PostNotification(this, "Unpause");
	}
}
