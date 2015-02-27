using UnityEngine;
using System.Collections;

public class LevelController : MonoBehaviour 
{
	public void SceneSwitch (int scene) 
	{
		Application.LoadLevel (scene);
	}
}
