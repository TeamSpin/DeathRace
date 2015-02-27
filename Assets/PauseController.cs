using UnityEngine;
using System.Collections;

public class PauseController : MonoBehaviour 
{
	bool is_paused = false;
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetKey(KeyCode.Escape)) is_paused = CheckPause();
	}

	void OnGUI()
	{
		if(is_paused)
		{
			GUILayout.Label("Paused");
			if(GUILayout.Button("Resume.")) is_paused = CheckPause();
			
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
