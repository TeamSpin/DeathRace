﻿using UnityEngine;
using System.Collections;

public class PauseController : MonoBehaviour 
{
	public int current_level;
	private int button_width = 200;
	private int button_height = 50;
	public Font button_font;
	public Color button_color;
	public bool is_paused = false;
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetKey(KeyCode.Escape)) is_paused = CheckPause();
	}

	void OnGUI()
	{
		GUI.skin.font = button_font;
		GUI.color = button_color;
		if(is_paused)
		{
			GUILayout.BeginArea(new Rect((Screen.width / 2)-100, (Screen.height / 2)-75, 200, 150));
			GUILayout.FlexibleSpace();
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.BeginVertical();

			if(GUI.Button(new Rect(0, 0, button_width, button_height), "Resume"))
			{
				is_paused = CheckPause();
			}
			if(GUI.Button(new Rect(0, 60, button_width, button_height), "Restart Level"))
			{
				is_paused = CheckPause();
				Application.LoadLevel(current_level);
			}
			GUILayout.EndVertical();
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.EndArea();
		}
	}

	bool CheckPause()
	{
		if(Time.timeScale == 0.0f) //if time has stopped
		{
			Time.timeScale = 1.0f;
			return false;
		}
		else 
		{
			Time.timeScale = 0.0f;
			return true;		
		}
	}
}
