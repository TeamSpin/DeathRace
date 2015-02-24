using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CarLap : MonoBehaviour {

	int lapNum = 0;
	int state = 0;

	public GameObject finishLine;
	public GameObject check2;
	public GameObject check3;
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

		if( other.gameObject == finishLine)
		{
			//moving across finish line forwards
			if(state == 0)
			{
				lapNum++;
				UnityEngine.UI.Text s = theHud.GetComponent<Text>();
				s.text = "Lap " + lapNum.ToString() + " / " + maxLaps.ToString();
				state = 1;
				print ("Hit check 1");
			}
			//moving across finish line backwards
			else if(state == 1)
			{
				lapNum--;
				UnityEngine.UI.Text s = theHud.GetComponent<Text>();
				s.text = "Lap " + lapNum.ToString() + " / " + maxLaps.ToString();
				state = 0;
				print ("Hit check 1");
			}
		}

		else if( other.gameObject == check2)
		{
			if( state == 1)//moving forward
			{
				state = 2;
			}
			else if( state == 2)//backward
			{
				state = 1;
			}
		}

		else if( other.gameObject == check3)
		{
			if( state == 2)//forward
			{
				state = 0;
			}
			else if( state == 0)//backward
			{
				state = 2;
			}
		}
	}
}
