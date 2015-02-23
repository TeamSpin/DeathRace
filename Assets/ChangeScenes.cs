using UnityEngine;
using System.Collections;

public class ChangeScenes : MonoBehaviour 
{
	public void SceneSwitch (int scene) 
	{
		Application.LoadLevel (scene);
	}
}
