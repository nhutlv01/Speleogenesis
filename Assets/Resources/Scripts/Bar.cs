using UnityEngine;
using System.Collections;

public class Bar : MonoBehaviour {
	public float percent;
	public float current;
	public float max;
	public float barScale;
	public GameObject barObject;
	public TextMesh text;
	public Vector3 Offset = Vector3.zero;
	// Use this for initialization
	void Start () {
		text = new GameObject ("Text").AddComponent ("TextMesh") as TextMesh;
		text.gameObject.transform.parent = transform;
		text.transform.localPosition = Vector3.zero + Offset;

		text.font = Resources.Load ("Fonts/Lemiesz") as Font;
		text.renderer.material = text.font.material;
		text.characterSize = 0.02f;
		text.alignment = TextAlignment.Center;
		text.anchor = TextAnchor.MiddleCenter;
		text.fontSize = 100;
	}
	
	// Update is called once per frame
	void Update () {
		barObject.transform.localScale = new Vector3 (percent * barScale *.01f +.001f, barObject.transform.localScale.y, barObject.transform.localScale.z);
		text.text = ("(" + Mathf.Round(current) + "," + max + ")");

	}
}
