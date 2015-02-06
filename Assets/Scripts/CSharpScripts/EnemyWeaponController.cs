using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class EnemyWeaponController : MonoBehaviour {
	public int damagePerShot = 4;
	public float timeBetweenBullets = 0.15f;
	public int enemyCollisionDamage = 30;
	public int playerCollisionDamage = 15;
	
	
	public float detectionAngle = 60.0f;
	public int detectionDist = 30;
	
	
	
	public bool gunEnabled = true;
	public int numBullets = 50;
	float range = 50f;
	float effectsDisplayTime = 0.2f;
	
	float timer;
	Ray shootRay;
	RaycastHit shootHit;
	int shootableMask;
	List<GameObject> shootableTargets;
	
	//ParticleSystem gunParticles;
	LineRenderer MgunLine;
	AudioSource MgunAudio;
	Light MgunLight;
	
	
	void Awake ()
	{
		shootableMask = LayerMask.GetMask ("Player");
		shootableTargets = grabTargets();
		//gunParticles = GetComponent<ParticleSystem> ();
		MgunLine = GetComponent <LineRenderer> ();
		MgunAudio = GetComponent<AudioSource> ();
		MgunLight = GetComponent<Light> ();
		//Physics.IgnoreLayerCollision( LayerMask.GetMask("EnemyCars"),LayerMask.GetMask("Default"),true);
	}
	
	
	void Update ()
	{
		
		timer += Time.deltaTime;
		if( gunEnabled ) 
		{
			if( numBullets > 0 && timer >= timeBetweenBullets )//player in front of
			{
//				bool go = shouldIFire();
//				print(go);
//				print (shootableTargets.Count);
//				if( go  )
//				{
//					ShootMgun();
//				}
			}
		}
		if(timer >= timeBetweenBullets * effectsDisplayTime)	//Disable to toggle line to look like gunfire
		{
			MgunLine.enabled = false;	
			MgunLight.enabled = false;
		}
	}
	
	bool shouldIFire() 
	{
		RaycastHit fireHit;
		shootableTargets = grabTargets();
		for( int i = 0; i < shootableTargets.Count; i++ ) 
		{
			Vector3 fireDirection = shootableTargets[i].transform.position + Vector3.up/2;
			print( fireDirection.ToString() );
			if ((Vector3.Angle(fireDirection, transform.forward)) < detectionAngle * .5f )
			{
				print ("got to hereaoeuhgalehglakhgeklhgealjhge");
				if (Physics.Raycast(transform.position, fireDirection, out fireHit, detectionDist))
				{
					return true;
				}
			}
		}
		return false;
		
	}
	
	List<GameObject> grabTargets() 
	{
		GameObject[] allObjects = FindObjectsOfType(typeof(GameObject)) as GameObject[]; 
		List <GameObject> fireable = new List<GameObject>();
		for (int i = 0; i < allObjects.Length; i++) 
		{ 
			if ((allObjects[i].layer == 8 || allObjects[i].layer == 9) && allObjects[i].collider != null ) 
			{ 
				fireable.Add(allObjects[i]);
				//print(LayerMask.GetMask("Player"));
				//print(LayerMask.GetMask("EnemyCars"));
				//allObjects[i].
				
			} 
		} 
		
		if (fireable.Count == 0) 
		{ 
			return null; 
		} 
		return fireable; 
	}
	
	void ShootMgun ()
	{
		numBullets--;
		if( numBullets <= 0 ) {
			gunEnabled = false;
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
			PlayerHealth playerHealth = shootHit.collider.GetComponentInParent <PlayerHealth> ();
			if(playerHealth != null)
			{
				playerHealth.TakeDamage (damagePerShot, shootHit.point);
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
			collider.gameObject.SetActive(false);
		}
		if( collider.gameObject.tag == "Defense" )
		{
			numBullets = 0;
			gunEnabled = false;
			collider.gameObject.SetActive(false);
			
			
			EnemyHealth enemyHealth = gameObject.GetComponentInParent<EnemyHealth>();
			enemyHealth.HealCar(20); 
		}
	}
	
//	void OnCollisionEnter(Collision collision) 		//Does this calculate twice! Depends on AI
//	{
//		print(shootableMask);
//		if( collision.gameObject.layer == 9 ) 
//		{
//			PlayerHealth playerHealth = collision.collider.GetComponentInParent <PlayerHealth> ();
//			playerHealth.TakeDamage( enemyCollisionDamage, collision.collider.transform.position );
//			
//			EnemyHealth enemyHealth = gameObject.GetComponentInParent<enemyHealth>();
//			enemyHealth.TakeDamage(playerCollisionDamage);
//		}
//		
//	}
	
	
	
	
	
	
	
}