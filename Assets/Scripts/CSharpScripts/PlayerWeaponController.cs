using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerWeaponController : MonoBehaviour {
	public int damagePerShot = 4;
	public float timeBetweenBullets = 0.15f;
	public RawImage ammoPic;
	public RawImage shieldPic;
	public int enemyCollisionDamage = 30;
	public int playerCollisionDamage = 15;
	
	
	
	bool gunEnabled = false;
	int numBullets = 0;
	float range = 50f;
	float effectsDisplayTime = 0.2f;
	
	float timer;
	Ray shootRay;
	RaycastHit shootHit;
	int shootableMask;
	//ParticleSystem gunParticles;
	LineRenderer MgunLine;
	AudioSource MgunAudio;
	Light MgunLight;
	
	
	void Awake ()
	{
		ammoPic.gameObject.SetActive(false);
		shieldPic.gameObject.SetActive(false);
		shootableMask = LayerMask.GetMask ("EnemyCars");
		//gunParticles = GetComponent<ParticleSystem> ();
		MgunLine = GetComponent <LineRenderer> ();
		MgunAudio = GetComponent<AudioSource> ();
		MgunLight = GetComponent<Light> ();
	}
	
	
	void Update ()
	{
		timer += Time.deltaTime;
		
		if(Input.GetButton ("Fire1") && timer >= timeBetweenBullets 
		   && Time.timeScale != 0 && gunEnabled )
		{
			ShootMgun ();
		}
		
		if(timer >= timeBetweenBullets * effectsDisplayTime)	//Disable to toggle line to look like gunfire
		{
			MgunLine.enabled = false;	
			MgunLight.enabled = false;
		}
	}
	
	
	void ShootMgun ()
	{
		numBullets--;
		if( numBullets <= 0 ) {
			gunEnabled = false;
			ammoPic.gameObject.SetActive(false);
		}
		
		timer = 0f;  //Reset the timer to not fire insanley fast
		MgunLight.enabled = true; //Enable line rendered, looks like bullet fire path
		MgunAudio.Play ();    //Play random sound bit
		
		//gunParticles.Stop ();
		//gunParticles.Play ();
		
		MgunLine.enabled = true;
		
		shootRay.origin = transform.position + Vector3.up/2;		//Set origin to be from where gun fires
		shootRay.direction = transform.forward;		//Away from the user car in Z direction
		
		MgunLine.SetPosition (0, shootRay.origin); 
		
		if(Physics.Raycast (shootRay, out shootHit, range, shootableMask))
		{
			EnemyHealth enemyHealth = shootHit.collider.GetComponentInParent <EnemyHealth> ();
			if(enemyHealth != null)
			{
				enemyHealth.TakeDamage (damagePerShot, shootHit.point);
			}
			
			MgunLine.SetPosition (1, shootHit.point);
		}
		else
		{
			MgunLine.SetPosition (1, shootRay.origin + shootRay.direction * range);
		}
	}
	
	void OnTriggerEnter(Collider collider)
	{
		if( collider.gameObject.tag == "Weapon" )
		{
			numBullets = 50;
			gunEnabled = true;
			ammoPic.gameObject.SetActive(true);
			shieldPic.gameObject.SetActive(false);
			collider.gameObject.SetActive(false);
		}
		if( collider.gameObject.tag == "Defense" )
		{
			numBullets = 0;
			gunEnabled = false;
			ammoPic.gameObject.SetActive(false);
			shieldPic.gameObject.SetActive(true);
			collider.gameObject.SetActive(false);
			
			
			PlayerHealth playerHealth = gameObject.GetComponentInParent<PlayerHealth>();
			playerHealth.HealCar(20);
		}
	}
	
	void OnCollisionEnter(Collision collision) 
	{
		if( collision.gameObject.layer == 10 ) 
		{
			EnemyHealth enemyHealth = collision.collider.GetComponentInParent <EnemyHealth> ();
			enemyHealth.TakeDamage( enemyCollisionDamage, collision.collider.transform.position );
			
			PlayerHealth playerHealth = gameObject.GetComponentInParent<PlayerHealth>();
			playerHealth.TakeDamage(playerCollisionDamage, collision.transform.position );
		}
		
		
		
		
	}
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
}