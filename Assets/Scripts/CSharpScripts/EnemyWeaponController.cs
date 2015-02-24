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
	public int detectionDist = 50;
	
	public bool gunEnabled = true;
	public int numBullets = 500;
	float range = 50f;
	float effectsDisplayTime = 0.2f;
	
	float timer;
	Ray shootRay;
	RaycastHit shootHit;
	int shootableMask;
	List<GameObject> shootableTargets = new List<GameObject>();
	
	//ParticleSystem gunParticles;
	LineRenderer MgunLine;
	AudioSource MgunAudio;
	Light MgunLight;
	
	
	void Awake ()
	{
		shootableMask = LayerMask.GetMask ("Cars");
		//MgunLine = gameObject.AddComponent<LineRenderer>();
		MgunLine = GetComponent <LineRenderer> ();
		
		SetUpTargets();		
		SetUpWeapons();
//		print ("# tartgets = " + shootableTargets.Count );
		
		//for( int i = 0; i < shootableTargets.Count; i++ )
			//print (shootableTargets[i].ToString());
		
		//shootableTargets = grabTargets();
		//gunParticles = GetComponent<ParticleSystem> ();
		//MgunAudio = GetComponent<AudioSource> ();
		//MgunLight = GetComponent<Light> ();
		//Physics.IgnoreLayerCollision( LayerMask.GetMask("EnemyCars"),LayerMask.GetMask("Default"),true);
	}
	
	void SetUpWeapons()
	{
		MgunLine.material = new Material(Shader.Find("Particles/Additive"));
		MgunLine.SetWidth(.1f,.1f);
		MgunLine.SetColors( new Color( 1.0f, 1.0f, 1.0f ), new Color( 1.0f, 1.0f, 1.0f) );
	}
	
	void SetUpTargets() 
	{
		GameObject[] AIS = GameObject.FindGameObjectsWithTag("AICar");
		for( int i = 0; i < AIS.Length; i++ )
			if( AIS[i].gameObject.transform.position != transform.position )
				shootableTargets.Add(AIS[i]);
		GameObject me = GameObject.FindGameObjectWithTag("Player");
		shootableTargets.Add(me);
	}
	
	
	void Update ()
	{
		
		timer += Time.deltaTime;
		if( gunEnabled ) 
		{
			if( numBullets > 0 && timer >= timeBetweenBullets )//player in front of
			{
				if( shouldIFire() )
				{
					ShootMgun();
				}
			}
		}
		if(timer >= timeBetweenBullets * effectsDisplayTime)	//Disable to toggle line to look like gunfire
		{
			MgunLine.enabled = false;	
			//MgunLight.enabled = false;
		}
	}
	
	bool shouldIFire() 
	{
		RaycastHit fireHit;
		Vector3 looking = transform.forward;
		for( int i = 0; i < shootableTargets.Count; i++ ) 
		{
			Vector3 fireDirection = shootableTargets[i].transform.position - transform.position;
			float angle = Vector3.Angle( fireDirection, looking );
			
			//print( looking.ToString() + ", " + fireDirection.ToString() + " = " + angle );
			if (angle < (detectionAngle * .5f) )
			{
//				print ("THE ANGLE IS DETECTED" + Time.deltaTime.ToString());
				if( Vector3.Distance( transform.position, shootableTargets[i].transform.position ) < detectionDist )
					return true;
//				if (Physics.Raycast(transform.position, fireDirection, out fireHit, detectionDist))
//				{
//					return true;
//				}
			}
		}
		return false;
		
	}
	
	
	void ShootMgun ()
	{
		numBullets--;
		if( numBullets <= 0 ) {
			gunEnabled = false;
		}
		
		timer = 0f;  //Reset the timer to not fire insanley fast
		//MgunLight.enabled = true; //Enable line rendered, looks like bullet fire path
		//MgunAudio.Play ();    //Play random sound bit
		
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