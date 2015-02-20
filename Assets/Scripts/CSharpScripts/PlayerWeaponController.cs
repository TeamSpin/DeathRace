using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerWeaponController : MonoBehaviour {
	public int damagePerShot = 4;
	public float timeBetweenBullets = 0.15f;
	public RawImage ammoPic;
	public RawImage rocketPic;
	public RawImage shieldPic;
	public RawImage laserPic;
	public GameObject rocket;
	
	Text weaponCounter;
	Text defenseCounter;
	public int enemyCollisionDamage = 30;
	public int playerCollisionDamage = 15;
	
	
	//Using int for efficiency, much faster than string comparison every frame
	//itemHeld[0] -> Weaponheld      itemHeld[1] -> Defenseheld
	//0 for no item, 1->machinegun/healthpack, 2->laser/shield, 3->rocketlauncher/gravityfield
	int[] itemsHeld = { 0, 0 };
	
	int numWeapon = 50;
	int numDefense = 0;
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
		//rocket = GameObject.Find("RocketMissile");
		weaponCounter = GameObject.Find("WeaponCounter").GetComponent<Text>();
		defenseCounter = GameObject.Find("DefenseCounter").GetComponent<Text>();
		
		ammoPic.gameObject.SetActive(false);
		shieldPic.gameObject.SetActive(false);
		laserPic.gameObject.SetActive(false);
		rocketPic.gameObject.SetActive(false);
		weaponCounter.gameObject.SetActive(false);
		defenseCounter.gameObject.SetActive(false);
		
		shootableMask = LayerMask.GetMask ("Cars");
		//gunParticles = GetComponent<ParticleSystem> ();
		MgunLine = GetComponent <LineRenderer> ();
		MgunAudio = GetComponent<AudioSource> ();
		MgunLight = GetComponent<Light> ();
		
		SetUpWeapons();
	}
	
	void SetUpWeapons()
	{
		Random.seed = (int)(Time.deltaTime*10000)%50;
		MgunLine.material = new Material(Shader.Find("Particles/Additive"));
		MgunLine.SetWidth(.1f,.1f);
		MgunLine.SetColors( new Color( 1.0f, 1.0f, 1.0f ), new Color( 1.0f, 1.0f, 1.0f) );
	}
	
	
	void Update ()
	{
		timer += Time.deltaTime;
		if( itemsHeld[0] != 0 || itemsHeld[1] != 0 )
		{
			if( Input.GetButton ("Fire1") && timer >= timeBetweenBullets && itemsHeld[0] == 3 )
			{
				ShootLauncher ();
			}
			if( Input.GetButton ("Fire1") && timer >= timeBetweenBullets && itemsHeld[0] == 2 )
			{
				ShootLaser ();
			}
			if(Input.GetButton ("Fire1") && timer >= timeBetweenBullets 
			   && Time.timeScale != 0 && itemsHeld[0] == 1)
			{
				ShootMgun ();
			}
			
			if( Input.GetButton ("Fire2") && itemsHeld[1] == 1 )
			{
				UseHPItem();	
			}
			

		}
		if(timer >= timeBetweenBullets * effectsDisplayTime)	//Disable to toggle line to look like gunfire
		{
			MgunLine.enabled = false;	
			MgunLight.enabled = false;
		}
	}
	
	void UseHPItem ()
	{
		numDefense = 0;
		weaponCounter.text = numDefense.ToString();
		
		PlayerHealth playerHealth = gameObject.GetComponentInParent<PlayerHealth>();
		playerHealth.HealCar(20);
		itemsHeld[1] = 0;
		shieldPic.gameObject.SetActive(false);
		defenseCounter.gameObject.SetActive(false);
	}
	
	
	void ShootMgun ()
	{
		numWeapon--;
		weaponCounter.text = numWeapon.ToString();
		if( numWeapon <= 0 ) {
			itemsHeld[0] = 0;
			
			ammoPic.gameObject.SetActive(false);
			weaponCounter.gameObject.SetActive(false);
		}
		
		timer = 0f;  //Reset the timer to not fire insanley fast
		MgunLight.enabled = true; //Enable line rendered, looks like bullet fire path
		MgunAudio.Play ();    //Play random sound bit
		
		//gunParticles.Stop ();
		//gunParticles.Play ();
		
		MgunLine.enabled = true;
		
		shootRay.origin = transform.position + Vector3.up/2;		//Set origin to be from where gun fires
		shootRay.direction = transform.forward;		//Away from the user car in Z direction
		
		MgunLine.SetPosition (0, GetGunOrigin()); 
		
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
	
	void ShootLauncher () 
	{
		numWeapon--;
		weaponCounter.text = numWeapon.ToString();
		if( numWeapon <= 0 ) {
			itemsHeld[0] = 0;
			rocketPic.gameObject.SetActive(false);
			weaponCounter.gameObject.SetActive(false);
		}
		timer = 0.0f;
		Quaternion rotate = transform.rotation;
		float angle = transform.rotation.eulerAngles.y;
		Vector3 axis = Vector3.up;
		rotate.ToAngleAxis(out angle,out axis);
		Instantiate (rocket, GetGunOrigin()+Vector3.up*2, rotate);
	}
	
	Vector3 GetGunOrigin() 
	{
		return transform.position + Vector3.up;
	}
	
	void SetLaserProperies () 
	{
		MgunLine.material = new Material(Shader.Find("Particles/Additive"));
		MgunLine.SetColors(new Color(1.0f,0.0f,0.0f), new Color(0.0f,1.0f,0.0f));
		MgunLine.SetWidth(0.4F, 0.2F);
		MgunLine.SetVertexCount(15);
	}
	
	void ShootLaser ()
	{
		numWeapon--;
		weaponCounter.text = numWeapon.ToString();
		if( numWeapon <= 0 ) {
			itemsHeld[0] = 0;
			laserPic.gameObject.SetActive(false);
			weaponCounter.gameObject.SetActive(false);
			return;
		}
		timer = 0.0f;

//		float acuteTriangleAngle = 60.0f;
		
		Vector3 pos = GetGunOrigin();
		float nx, ny, nz;
		MgunLine.SetPosition(0, pos);
		
		for( int i = 1; i < 15; i++ )
		{
//			float deg = (acuteTriangleAngle * i) % 360;
//			float rad = deg * Mathf.Deg2Rad;
			nx = pos.x + 3 * Random.insideUnitCircle.x;
			ny = pos.y + 3 * Random.insideUnitCircle.y;
//			nx = pos.x + 2*(Mathf.Cos(rad));
//			ny = pos.y + 2*(Mathf.Sin(rad));
			nz = pos.z + i*10;
			Vector3 refFrame = pos + RotateVectorY( new Vector3(nx,ny,nz)-pos, transform.rotation.eulerAngles.y);
			//print ( " x " + refFrame.x + ", y " + refFrame.y + ", z " + refFrame.z );
						MgunLine.SetPosition(i, refFrame);
		}

		MgunLine.enabled = true;
		MgunLight.enabled = true;
		
	}
	
	Vector3 RotateVectorY(Vector3 oldDirection, float angle)   
	{
		float newX = Mathf.Sin(angle*Mathf.Deg2Rad) * (oldDirection.z) + Mathf.Sin(angle*Mathf.Deg2Rad) * (oldDirection.x);   
		float newY = oldDirection.y;    		
		float newZ = Mathf.Cos(angle*Mathf.Deg2Rad) * (oldDirection.z) - Mathf.Cos(angle*Mathf.Deg2Rad) * (oldDirection.y);        
		return new Vector3(newX, newY, newZ);   
	}
	
	
	void OnTriggerEnter(Collider collider)
	{
		if( collider.gameObject.tag == "Weapon" )
		{
			ammoPic.gameObject.SetActive(false);
			laserPic.gameObject.SetActive(false);
			rocketPic.gameObject.SetActive(false);
			
			
			int randWeapon = ((int)(Random.insideUnitCircle.x*1000))%3;//Mathf.Abs((int)(Random.insideUnitCircle.x*100)) % 3;
						
			if( randWeapon == 0 )
			{
				SetUpWeapons();
				itemsHeld[0] = 1;
				numWeapon = 50;
				ammoPic.gameObject.SetActive(true);
				
			}
			else if ( randWeapon == 1 )
			{
				SetLaserProperies();
				itemsHeld[0] = 2;
				numWeapon = 20;
				laserPic.gameObject.SetActive(true);
			}			
			else if ( randWeapon == 2 )
			{
				itemsHeld[0] = 3;
				numWeapon = 3;
				rocketPic.gameObject.SetActive(true);
			}
			weaponCounter.text = numWeapon.ToString();
			weaponCounter.gameObject.SetActive(true);
			collider.gameObject.SetActive(false);
			Destroy(collider.gameObject);
		}
		if( collider.gameObject.tag == "Defense" )
		{
			itemsHeld[1] = 1;
			numDefense = 1;
			defenseCounter.text = numDefense.ToString();
			
			shieldPic.gameObject.SetActive(true);
			defenseCounter.gameObject.SetActive(true);
			collider.gameObject.SetActive(false);
			Destroy(collider.gameObject);
		}
	}
	
	void OnCollisionEnter(Collision collision) 
	{
		if( collision.gameObject.tag == "AICar" ) 
		{
			EnemyHealth enemyHealth = collision.collider.GetComponentInParent <EnemyHealth> ();
			enemyHealth.TakeDamage( enemyCollisionDamage, collision.collider.transform.position );
			
			PlayerHealth playerHealth = gameObject.GetComponentInParent<PlayerHealth>();
			playerHealth.TakeDamage(playerCollisionDamage, collision.transform.position );
		}

	}
	
	
	
	
	
}