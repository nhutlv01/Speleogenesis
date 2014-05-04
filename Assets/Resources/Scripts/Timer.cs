using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour {

	public float timerTimeLeft = 3.0f;
	public float timerMax = 3.0f;
	public float barScale;
	public GameObject barObject;
	public TextMesh textMesh;
	public bool bPaused = false;
	public float timerDecreaseLevel;
	bool bPreAttack = false;
	// Use this for initialization
	void Start () {
		//Add observers
		NotificationCenter.DefaultCenter().AddObserver(this, "Pause");
		NotificationCenter.DefaultCenter().AddObserver(this, "Unpause");
		NotificationCenter.DefaultCenter().AddObserver(this, "AddTime");

		//Assign textmesh and fill
		textMesh = this.GetComponent<TextMesh> ();
		textMesh.text = timerMax.ToString();
		 
		//Set up textMesh with correct style
		/*textMesh.font = Resources.Load ("Fonts/Lemiesz") as Font;
		textMesh.renderer.material = textMesh.font.material;
		textMesh.characterSize = 0.02f;
		textMesh.alignment = TextAlignment.Center;
		textMesh.anchor = TextAnchor.MiddleCenter;
		textMesh.fontSize = 100;*/
		//InvokeRepeating("Countdown", 0.0f, 0.01f);
	}
	
	// Update is called once per frame
	void Update () {
		//If the game is not paused
		if(!bPaused)
		{
			//Timer needs reset
			if (timerTimeLeft <= 0.0f)
			{
				ResetTimer();
			}
			//Trigger enemy pre-attack
			else if(timerTimeLeft <= 2.60f)
			{
				//if a preattack animation has not been triggered
				if(!bPreAttack)
				{
					//Post notification
					bPreAttack = true;
					NotificationCenter.DefaultCenter().PostNotification (this, "PreEnemyAttack");
				}
			}
			//Update time
			timerTimeLeft = Mathf.Round((timerTimeLeft - Time.deltaTime) * 1000f) / 1000f;
			//Update the timer bar
			updateTimerBar();
		}
	}
	
	/////////////////////////////////
	//ResetTimer()
	//Resets timer back to timerMax and sends notification
	/////////////////////////////////
	void ResetTimer()
	{
		NotificationCenter.DefaultCenter().PostNotification (this, "TimerTrigger");
		bPreAttack = false;
		timerTimeLeft = timerMax;
	}

	/////////////////////////////////
	//AddTime()
	//Other objects can send message to timer to increase time.
	/////////////////////////////////
	void AddTime(Notification notification)
	{
		bPreAttack = false;
		timerTimeLeft += (float)notification.data;
	}

	/////////////////////////////////
	//updateTimerBar()
	//Changes x scale of timer bar and colorLerps to change it's color.
	//Text is updated to match the time left.
	/////////////////////////////////
	void updateTimerBar()
	{
		Color colorLerp;
		if(timerTimeLeft < timerMax)
		{
			barObject.transform.localScale = new Vector3 (timerTimeLeft / timerMax * barScale + .001f, barObject.transform.localScale.y, barObject.transform.localScale.z);
			colorLerp = Color.Lerp(Color.red, Color.green, timerTimeLeft % timerMax / timerMax);
		}
		else
		{
			barObject.transform.localScale = new Vector3 (timerTimeLeft % timerMax / timerMax * barScale + .001f, barObject.transform.localScale.y, barObject.transform.localScale.z);
			colorLerp = Color.Lerp(Color.green, Color.cyan, timerTimeLeft % timerMax / timerMax);
		}
		barObject.GetComponent<SpriteRenderer>().color = colorLerp;
		textMesh.text = timerTimeLeft.ToString ("F2");
		textMesh.color = colorLerp;
	}
	/////////////////////////////////
	//Pause
	//When the game is paused
	/////////////////////////////////
	void Pause()
	{
		bPaused = true;
	}
	
	/////////////////////////////////
	//Unpause
	//When the game resumes
	/////////////////////////////////
	void Unpause()
	{
		bPaused = false;
	}
}
