using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PauseDisplay : MonoBehaviour 
{	
	PauseController pausescript;
	public Text pause;

	void Awake()
	{
		pause.gameObject.SetActive(false);
	}

	void Start()
	{
		GameObject pausecontrol = GameObject.Find("PauseManager");
		pausescript = pausecontrol.GetComponent<PauseController>();	
	}

	// Update is called once per frame
	void Update () 
	{
		bool display_pause = pausescript.is_paused;
		if(display_pause)
		{
			pause.gameObject.SetActive(true);
		}
		else 
		{
			pause.gameObject.SetActive(false);		
		}
	}
}
