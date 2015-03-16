using UnityEngine;
using System.Collections;

public class VolumeManager : MonoBehaviour 
{
	public float fade;
	public float current_volume = 1.0f;
	public static VolumeManager menu_id {get; private set;}
	EndGameController endscript;
	public AudioSource main_song;
	public AudioSource end_song;
	public AudioClip theme_song;

	public AudioSource weapon_one;
	public AudioSource weapon_two;
	public AudioSource weapon_three;

 	bool music_status = true;
	private string level_name;	

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
		main_song.Play();
		StartCoroutine(FadeInAudio(fade, main_song));
	}

	void Update()
	{
		level_name = Application.loadedLevelName;
		if(music_status)
		{
			if(level_name == "MenuScene")
			{
				if(end_song.isPlaying) end_song.Stop();
				if(!end_song.isPlaying && !main_song.isPlaying) main_song.Play();
			}
	
			if(level_name != "Menu Scene" && level_name != "LevelSelectScene")
			{
				GameObject endcontrol = GameObject.Find("GameOver Manager"); //check to see if the game is over
				endscript = endcontrol.GetComponent<EndGameController>();
				if(endscript.is_over)
				{
					main_song.Pause();
					if(!main_song.isPlaying && !end_song.isPlaying) end_song.Play();
				}
				else 
				{
					if(end_song.isPlaying && !endscript.is_over) end_song.Stop();
					if(!end_song.isPlaying && !main_song.isPlaying) main_song.Play();
				}

				//check to see if a weapon is fired
			}
		}
		main_song.volume = current_volume;
		end_song.volume = current_volume;
	}

	//Slowly fade in the audio based on fade_timer seconds
	IEnumerator FadeInAudio(float fade_timer, AudioSource song)
	{
		float i = 0.0f;
		float volume_add = 1.0f / fade_timer;
		while(i <= 1.0f) 
		{
			i += volume_add * Time.deltaTime;
			song.volume = Mathf.Lerp(0.0f, 1.0f, i);	
			yield return new WaitForSeconds(volume_add * Time.deltaTime);
		}
	}

	//Mute and unmute audio
	public void AudioToggle()
	{
		music_status = !music_status;
		if(music_status == true) main_song.Play();
		else main_song.Pause();
	}
	
	//Adjust volume based on slider input
	public void AdjustVolume(float slider_volume)
	{
		current_volume = slider_volume;
	}

	public void PlayWeaponAudio(string weapon)
	{
		main_song.volume = 0.7f;
		end_song.volume = 0.7f;

		if(weapon == "rocket")
			weapon_one.Play();
		else if(weapon == "laser")
			weapon_two.Play();
		else if(weapon == "gun")
			weapon_three.Play();

		main_song.volume = 1.0f;
		end_song.volume = 1.0f;
	}
}
