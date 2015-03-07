using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PauseDisplay : MonoBehaviour 
{	
	PauseController pausescript;
	public Text pause;
	public Image bg;

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
			bg.gameObject.SetActive(true);
		}
		else 
		{
			pause.gameObject.SetActive(false);		
			bg.gameObject.SetActive(false);
		}
	}
}
