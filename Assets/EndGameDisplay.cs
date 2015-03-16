using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EndGameDisplay : MonoBehaviour 
{
	EndGameController endscript;
	public Text gameover;
	public Image bg;
	
	void Awake()
	{
		gameover.gameObject.SetActive(false);
		bg.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);
	}
	
	void Start()
	{
		GameObject endcontrol = GameObject.Find("GameOver Manager");
		endscript = endcontrol.GetComponent<EndGameController>();	
	}
	
	// Update is called once per frame
	void Update () 
	{
		bool display_pause = endscript.is_over;
		if(display_pause)
		{
			gameover.gameObject.SetActive(true);
			bg.gameObject.SetActive(true);
		}
		else 
		{
			gameover.gameObject.SetActive(false);		
			bg.gameObject.SetActive(false);
		}
	}

}
