using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LifeBar : MonoBehaviour {

public float percent;

void Start(){
		gameObject.renderer.sortingLayerName = "GUI";
		gameObject.renderer.sortingOrder = 0;
}

void Update () { 

	renderer.material.SetFloat("_Cutoff", Mathf.Lerp(0, 100, percent)); 
}

}