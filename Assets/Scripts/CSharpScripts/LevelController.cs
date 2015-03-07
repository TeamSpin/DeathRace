using UnityEngine;
using System.Collections;

public class LevelController : MonoBehaviour 
{
	public void SceneSwitch (int scene) 
	{
		if(scene == 0)
		{
			GameObject old_audio = GameObject.Find("VolumeManager");
			Destroy(old_audio);
		}
		Application.LoadLevel (scene);
	}
}
