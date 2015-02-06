using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour {
	public int startingHealth = 100;
	public Slider healthSlider;
	public int currentHealth;
	
	Image damageImage;//Blank palette, sets to red when damaged
	//public AudioClip deathClip;
	
	
	//public float flashSpeed = 5f;
	//public Color flashColour = new Color(1f, 0f, 0f, 0.1f);
	
	
	//Animator anim;
	//AudioSource playerAudio;
	//PlayerMovement playerMovement;
	//PlayerShooting playerShooting;
	bool isDead;
	bool damaged = false;
	
	void Awake ()
	{
		healthSlider.minValue = 0;
		healthSlider.maxValue = 100;
		//anim = GetComponent <Animator> ();
		//playerAudio = GetComponent <AudioSource> ();
		//playerMovement = GetComponent <PlayerMovement> ();
		//playerShooting = GetComponentInChildren <PlayerShooting> ();
		currentHealth = startingHealth;
		healthSlider.value = currentHealth;
	}
	
	
	void Update ()
	{
		if(damaged)
		{
			//damageImage.color = flashColour;
		}
		else
		{
			//damageImage.color = Color.Lerp (damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
		}
		damaged = false;
	}
	
	
	public void TakeDamage (int amount, Vector3 pointContact)
	{
		damaged = true;
		
		currentHealth -= amount;
		
		healthSlider.value = currentHealth;
		
		//playerAudio.Play ();
		
		if( currentHealth <= 0 )
		{
			Death ();
		}
	}
	
	public void HealCar ( int amount ) {
		currentHealth += 20;
		if( currentHealth > 100 )  currentHealth = 100;
		healthSlider.value = currentHealth;

	}
	
	
	void Death ()
	{
		isDead = true;
		
		transform.position += Vector3.up*2;
		currentHealth = 100;
		healthSlider.value = currentHealth;
		//playerShooting.DisableEffects ();
		
		//anim.SetTrigger ("Die");
		
		//playerAudio.clip = deathClip;
		//playerAudio.Play ();
		
		//playerMovement.enabled = false;
		//playerShooting.enabled = false;
	}
	
	
	public void RestartLevel ()
	{
		Application.LoadLevel (Application.loadedLevel);
	}
}

