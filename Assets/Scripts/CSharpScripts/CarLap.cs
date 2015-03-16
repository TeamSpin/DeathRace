using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CarLap : MonoBehaviour {

	public int lapNum = 0;
	int state = 0;

	public GameObject finishLine;
	public GameObject check2;
	public GameObject theHud;
	public int maxLaps;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnTriggerEnter(Collider other)
	{
		if( other.gameObject == finishLine && state == 0)
		{
			lapNum++;
			UnityEngine.UI.Text s = theHud.GetComponent<Text>();
			s.text = "Lap " + lapNum.ToString() + " / " + maxLaps.ToString();
			state = 1;
			print ("Hit check 1");
		}
		else if( other.gameObject == check2)
		{
			print ("Hit check 2");
			state = 0;
		}
	}
}
