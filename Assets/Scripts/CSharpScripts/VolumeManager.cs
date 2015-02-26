using UnityEngine;
using System.Collections;

public class VolumeManager : MonoBehaviour 
{
	public float fade;
	public float default_volume = 1.0f;
	public static VolumeManager menu_id {get; private set;};
 	bool music_status = true;

	void Awake()
	{
		if(menu_id != null && menu_id != this)
		{
			Destroy(this.gameObject);
			return;	
		}
		else
		{
			menu_id = this;
		}
		DontDestroyOnLoad(this.gameObject);		
	}

	void Start() 
	{
		StartCoroutine(FadeInAudio (fade));
	}

	void Update()
	{
		audio.volume = default_volume;
	}

	//Slowly fade in the audio based on fade_timer seconds
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

	//Mute and unmute audio
	public void AudioToggle()
	{
		music_status = !music_status;
		if(music_status == true) audio.Play();
		else audio.Pause();
	}
	
	//Adjust volume based on slider input
	public void AdjustVolume(float slider_volume)
	{
		default_volume = slider_volume;
	}
}
