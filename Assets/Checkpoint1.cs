using UnityEngine;
using System.Collections;

//Andy
public class Checkpoint1 : MonoBehaviour {

	public static int lapNum = 0;

	// Use this for initialization
	void Start () {
		//print ("Lap: " + lapNum + "\n");
	
		print ("start\n");
	}
	
	// Update is called once per frame
	void Update () {

		GameObject obj = this.gameObject;
		Collider col = obj.collider;


	
	}

	void OnTriggerEnter()
	{
		lapNum++;
		print ("Lap: " + lapNum + "\n");
		GUIText display = new GUIText();
		display.text = lapNum.ToString();

	}

	void OnTriggerStay()
	{
		print("Still touching\n");
	}

	/* void OnGUI()
	{
		GUI.Label(Rect(0,0,100,100),lapNum);
	}
	*/
}
