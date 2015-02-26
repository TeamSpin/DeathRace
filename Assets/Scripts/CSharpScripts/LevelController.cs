using UnityEngine;
using System.Collections;

public class LevelController : MonoBehaviour 
{
	public void SceneSwitch (int scene) 
	{
		if(gameObject.name == "ReturnToMenuButton") Destroy(GameObject.Find("VolumeManager"));
		Application.LoadLevel (scene);
	}
}
