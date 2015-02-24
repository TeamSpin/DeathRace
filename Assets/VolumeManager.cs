using UnityEngine;
using System.Collections;

public class VolumeManager : MonoBehaviour 
{
	public float fade;
	bool music_status = true;

	void Start() 
	{
		StartCoroutine(FadeInAudio (fade));
	}

	IEnumerator FadeInAudio(float fade_timer)
	{
		float i = 0.0f;
		float volume_add = 1.0f / fade_timer;
		while(i <= 1.0f) 
		{
			i += volume_add * Time.deltaTime;
			audio.volume = Mathf.Lerp(0.0f, 1.0f, i);	
			yield return new WaitForSeconds(volume_add * Time.deltaTime);
		}
	}

	public void Audio_Toggle()
	{
		music_status = !music_status;
		if(music_status == true) audio.Play();
		else audio.Pause();
	}
}
