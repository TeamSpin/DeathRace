using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyHealth : MonoBehaviour {

	public int StartHP = 100;
	int CurrentHP;
	
	Animator animate;
	ParticleSystem hitParticles;
	//CapsuleCollider capsuleCollider;
	public Slider enemyHealth;
	public bool isDead;
	bool damaged = false; 
	public float displayDuration = .1f;
	float timer;
	
	// Use this for initialization
	void Awake () {
		CurrentHP = StartHP;
		enemyHealth.maxValue = 100;
		enemyHealth.minValue = 0;
		enemyHealth.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		
		if( damaged ) {
			if( timer > displayDuration ) {
				damaged = false;
				
				enemyHealth.gameObject.SetActive(false);
			}
		}
		else {
			
		}
	}
	
	public void TakeDamage(int amount, Vector3 pointContact) {
		//gameObject.SetActive(false);
		enemyHealth.gameObject.SetActive(false);
		if( isDead )
			return;
		damaged = true;
		timer = 0f;
		
		CurrentHP = CurrentHP - amount;
		enemyHealth.value = CurrentHP;
		enemyHealth.gameObject.SetActive(true);
		//enemyHealth.
		//hitParticles.transform.position = pointContact;
		//hitParticles.Play();
		
		if( CurrentHP <= 0 ) {
			isDead = true;
			transform.position += Vector3.up * 3;
			
		}
	}
	
	public void HealCar ( int amount ) {
		CurrentHP += 20;
		if( CurrentHP > 100 )  CurrentHP = 100;
		
	}
	
	
}
