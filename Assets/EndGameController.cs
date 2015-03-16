using UnityEngine;
using System.Collections;

public class EndGameController : MonoBehaviour 
{
	private string current_level = "1";
	private int button_width = 200;
	private int button_height = 50;
	public Font button_font;
	public Color button_color;
	public bool is_over = false;
	public int winner;
	CarLap carlap_script;
	PauseController pausescript;
	private GameObject endcontrol;
	private GameObject pausecontrol;
	public string playeronecar;
	public string playertwocar;

	void Awake()
	{
		Time.timeScale = 1.0f;
		current_level = Application.loadedLevelName;
	}	

	// Use this for initialization
	void Start () 
	{
		endcontrol = GameObject.Find(playeronecar);
		carlap_script = endcontrol.GetComponent<CarLap>();
		pausecontrol = GameObject.Find("PauseController");
		pausescript = pausecontrol.GetComponent<PauseController>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(carlap_script.lapNum > carlap_script.maxLaps)
		{
			Time.timeScale = 0.3f;
			is_over = true;	
		}
		else 
		{
			if(!pausescript.is_paused)
				Time.timeScale = 1.0f;
			is_over = false;	
		}	
	}

	void OnGUI()
	{
		GUI.skin.font = button_font;
		GUI.color = button_color;
		if(is_over)
		{
			GUILayout.BeginArea(new Rect((Screen.width / 2)-100, (Screen.height / 2)-75, 200, 150));
			GUILayout.FlexibleSpace();
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.BeginVertical();
			
			if(GUI.Button(new Rect(0, 0, button_width, button_height), "Main Menu"))
			{
				Application.LoadLevel(0); //go back to the main menu
			}
			if(GUI.Button(new Rect(0, 60, button_width, button_height), "Restart Level"))
			{
				is_over = false;
				Application.LoadLevel(current_level);
			}
			GUILayout.EndVertical();
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.EndArea();
		}
	}
}
