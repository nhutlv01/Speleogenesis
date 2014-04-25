using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour {

	public float timerTimeLeft;
	public float timerMax;
	public float barScale;
	public GameObject barObject;
	public TextMesh textMesh;
	public bool bPaused = false;
	public float timerDecreaseLevel;
	// Use this for initialization
	void Start () {
		NotificationCenter.DefaultCenter().AddObserver(this, "Pause");
		NotificationCenter.DefaultCenter().AddObserver(this, "Unpause");
		NotificationCenter.DefaultCenter().AddObserver(this, "AddTime");

		textMesh = this.GetComponent<TextMesh> ();
		textMesh.text = timerMax.ToString();
		/*textMesh.font = Resources.Load ("Fonts/Lemiesz") as Font;
		textMesh.renderer.material = textMesh.font.material;
		textMesh.characterSize = 0.02f;
		textMesh.alignment = TextAlignment.Center;
		textMesh.anchor = TextAnchor.MiddleCenter;
		textMesh.fontSize = 100;*/
	}
	
	// Update is called once per frame
	void Update () {
		if(!bPaused)
		{
			if (timerTimeLeft <= 0.0f)
			{
				ResetTimer();
			}
			else{
				Color colorLerp;
				timerTimeLeft = timerTimeLeft - Time.deltaTime;
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
		}
	}

	void Countdown()
	{
	}

	void ResetTimer()
	{
		NotificationCenter.DefaultCenter().PostNotification (this, "TimerTrigger");
		timerTimeLeft = timerMax;
	}

	void AddTime(Notification notification)
	{
		timerTimeLeft += (float)notification.data;
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
