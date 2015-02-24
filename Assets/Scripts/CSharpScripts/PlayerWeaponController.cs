using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerWeaponController : MonoBehaviour {
	RawImage ammoPic;
	RawImage rocketPic;
	RawImage shieldPic;
	RawImage laserPic;
	RawImage boosterPic;
	
	public GameObject rocket;
	GameObject booster1;
	GameObject booster2;	
	
	Text weaponCounter;
	Text defenseCounter;
	public int enemyCollisionDamage = 30;
	public int playerCollisionDamage = 15;
	
	
	//Using int for efficiency, much faster than string comparison every frame
	//itemHeld[0] -> Weaponheld      itemHeld[1] -> Defenseheld
	//0 for no item, 1->machinegun/healthpack, 2->laser/shield, 3->rocketlauncher/gravityfield
	int[] itemsHeld = { 2, 0 };
	
	int numWeapon = 500;
	int numDefense = 0;
	float range = 50f;
	float effectsDisplayTime = 0.2f;
	public int damagePerShot = 4;
	public float timeBetweenBullets = 0.15f;
	public float timeBetweenLasers = 0.05f;
	float timeBetweenBoosts = .05f;
	
	
	
	float timer;
	float timer2;	//For the defense timing not to interfere with weapons
	Ray shootRay;
	RaycastHit shootHit;
	int shootableMask;
	//ParticleSystem gunParticles;
	LineRenderer MgunLine;
	AudioSource MgunAudio;
	Light MgunLight;
	

	void Awake ()
	{
	
		Random.seed = (int)(Time.deltaTime*10000)%50;
		
		//rocket = GameObject.Find("RocketMissile");
		weaponCounter = GameObject.Find("WeaponCounter").GetComponent<Text>();
		defenseCounter = GameObject.Find("DefenseCounter").GetComponent<Text>();
		ammoPic = GameObject.Find("AmmoPicture").GetComponent<RawImage>();
		shieldPic = GameObject.Find("HealthPicture").GetComponent<RawImage>();
		laserPic = GameObject.Find("LaserPicture").GetComponent<RawImage>();
		rocketPic = GameObject.Find("RocketPicture").GetComponent<RawImage>();
		boosterPic = GameObject.Find("BoosterPicture").GetComponent<RawImage>();
		
		booster1 = GameObject.Find("RocketFire03");
		booster2 = GameObject.Find("RocketFire02");
		//print(booster1.ToString());
		//print(booster2.ToString());
		
		
		booster1.SetActive(false);
		booster2.SetActive(false);
		
		ammoPic.gameObject.SetActive(false);
		shieldPic.gameObject.SetActive(false);
		boosterPic.gameObject.SetActive(false);
		laserPic.gameObject.SetActive(false);
		rocketPic.gameObject.SetActive(false);
		weaponCounter.gameObject.SetActive(false);
		defenseCounter.gameObject.SetActive(false);
		
		shootableMask = LayerMask.GetMask ("Cars");
		//gunParticles = GetComponent<ParticleSystem> ();
		MgunLine = GetComponent <LineRenderer> ();
		MgunAudio = GetComponent<AudioSource> ();
		MgunLight = GetComponent<Light> ();
		
		
		SetUpLaser();
		
		
	}
	
	void SetUpMgun()
	{
		MgunLine.SetVertexCount(2);
		MgunLine.material = new Material(Shader.Find("Particles/Additive"));
		MgunLine.SetWidth(.1f,.1f);
		MgunLine.SetColors( new Color( 1.0f, 1.0f, 1.0f ), new Color( 1.0f, 1.0f, 1.0f) );
	}
	
	
	void Update ()
	{
		timer += Time.deltaTime;
		timer2 += Time.deltaTime;
		if( itemsHeld[0] != 0 || itemsHeld[1] != 0 )
		{
			if( Input.GetButton ("Fire1") && timer >= timeBetweenBullets*3 && itemsHeld[0] == 3 )
				ShootLauncher ();
			if( Input.GetButton ("Fire1") && timer >= timeBetweenLasers && itemsHeld[0] == 2 )
				ShootLaser ();
			if(Input.GetButton ("Fire1") && timer >= timeBetweenBullets 
			   && Time.timeScale != 0 && itemsHeld[0] == 1)
				ShootMgun ();
			if( Input.GetButton ("Fire2") && itemsHeld[1] == 1 )
				UseHPItem();	
			if( Input.GetButton ("Fire2") && itemsHeld[1] == 2 && timer2 >= timeBetweenBoosts )
				UseBooster();	
			
		}
		if(timer >= timeBetweenBullets * effectsDisplayTime && ( itemsHeld[0] == 1 || itemsHeld[0] == 0) ){	//Disable to toggle line to look like gunfire
			MgunLine.enabled = false;	
			MgunLight.enabled = false;
		}
		if(timer >= timeBetweenLasers * effectsDisplayTime && ( itemsHeld[0] == 2 || itemsHeld[0] == 0) ) { 	//Disable to toggle line to look like gunfire {
			MgunLine.enabled = false;	
			MgunLight.enabled = false;
		}
		if(timer2 >= timeBetweenBoosts*5 ) {
			booster1.SetActive(false);
			booster2.SetActive(false);
		}
		
	}
	
	void UseHPItem ()
	{
		numDefense = 0;
		defenseCounter.text = numDefense.ToString();
		
		PlayerHealth playerHealth = gameObject.GetComponentInParent<PlayerHealth>();
		playerHealth.HealCar(20);
		itemsHeld[1] = 0;
		shieldPic.gameObject.SetActive(false);
		defenseCounter.gameObject.SetActive(false);
	}
	
	Vector3 getFireDir() 
	{	
		LayerMask levelMask = LayerMask.GetMask("Targetable");
		LayerMask carMask = LayerMask.GetMask("Cars");
		  
		Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit levelHit, carHit;
		Vector3 fireDir;
		if(Physics.Raycast (camRay, out carHit, 100f, carMask))
		{
			return carHit.point;
		}
		
		if(Physics.Raycast (camRay, out levelHit, 100f, levelMask))
		{
			return levelHit.point;
		}
		Ray mouseXY = Camera.main.ScreenPointToRay(Input.mousePosition);
		return GetGunOrigin() + mouseXY.direction*50;
	}
	
	void UseBooster ()
	{
		timer2 = 0;
		numDefense--;
		if( numDefense <= 0 ) {
			itemsHeld[1] = 0;
			boosterPic.gameObject.SetActive(false);
			defenseCounter.gameObject.SetActive(false);
		}
		defenseCounter.text = numDefense.ToString();
		booster1.SetActive(true);
		booster2.SetActive(true);
		//CAR SRIPT INCREASE MAX SPEED
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
		shootRay.direction = getFireDir()-shootRay.origin;//transform.forward;		//Away from the user car in Z direction
		
		MgunLine.SetPosition (0, GetGunOrigin()); 
		
		if(Physics.Raycast (shootRay, out shootHit, range, shootableMask))
		{
			EnemyHealth enemyHealth = shootHit.collider.GetComponentInParent <EnemyHealth> ();
			if(enemyHealth != null)
			{
				enemyHealth.TakeDamage (damagePerShot, shootHit.point);
			}
			MgunLine.SetPosition (1, getFireDir());
		}
		else
		{
			MgunLine.SetPosition (1, getFireDir());
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
		//Quaternion rotate = transform.rotation;
		Vector3 fire = getFireDir();
		Vector3 firedir = fire - GetGunOrigin();	
		Quaternion rotate = transform.rotation;
		rotate.SetLookRotation(firedir,Vector3.up);
		GameObject tempRocket = (GameObject)(Instantiate (rocket, GetGunOrigin()+Vector3.up*2, rotate));
	}
	
	Vector3 GetGunOrigin() 
	{
		return transform.position + Vector3.up;
	}
	
	void SetUpLaser () 
	{
		MgunLine.material = new Material(Shader.Find("Particles/Additive"));
		MgunLine.SetColors(new Color(1.0f,0.0f,1.0f), new Color(0.0f,1.0f,1.0f));
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
		Ray firedir = new Ray(pos, getFireDir()-pos);
		float nx, ny, nz;
		MgunLine.SetPosition(0, pos);
		
		for( int i = 1; i < 15; i++ )
		{
//			float deg = (acuteTriangleAngle * i) % 360;
//			float rad = deg * Mathf.Deg2Rad;
			nx = firedir.GetPoint(i).x + 2 * Random.insideUnitCircle.x;
			ny = firedir.GetPoint(i).y + 1 * Random.insideUnitCircle.y;
			nz = firedir.GetPoint(i).z;
			
			//nx += firedir.GetPoint(i).x;
			//ny += firedir.GetPoint(i).y;
//			nx = pos.x + 2*(Mathf.Cos(rad));
//			ny = pos.y + 2*(Mathf.Sin(rad));
			//nz = pos.z + i*10;
			//Vector3 refFrame = pos + RotateVectorY( new Vector3(nx,ny,nz)-pos, transform.rotation.eulerAngles.y);
			//print ( " x " + refFrame.x + ", y " + refFrame.y + ", z " + refFrame.z );
			Vector3 refFrame = new Vector3(nx,ny,nz);
			//refFrame.x += firedir.GetPoint(i).x;
			//refFrame.y += firedir.GetPoint(i).y;
			//print (refFrame );
//			print ("#" + i + firedir.GetPoint(i) );
			
			MgunLine.SetPosition(i, refFrame);
			Collider[] urDone4 = Physics.OverlapSphere( refFrame, 0.2f );
			foreach (Collider enemy in urDone4)
			{
				if( enemy && enemy.rigidbody ) 
				{
					EnemyHealth enemyHealth = enemy.GetComponentInParent <EnemyHealth> ();
					if(enemyHealth != null)
					{
						enemyHealth.TakeDamage (5, Vector3.zero);	
					}
				}
			}
			
		}
//		for( int i = 0; i < 15; i++ ) {
//			nx += firedir.GetPoint(i).x;
//			ny += firedir.GetPoint(i).y;
//		}
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
			
			MgunLine.enabled = false;	
			MgunLight.enabled = false;
			int randWeapon = Mathf.Abs(((int)(Random.insideUnitCircle.x*1000)))%3;//Mathf.Abs((int)(Random.insideUnitCircle.x*100)) % 3;
			if( randWeapon == 0 )
			{
				itemsHeld[0] = 1;
				numWeapon = 50;
				SetUpMgun();
				
				ammoPic.gameObject.SetActive(true);
				laserPic.gameObject.SetActive(false);
				rocketPic.gameObject.SetActive(false);
				
			}
			else if ( randWeapon == 1 )
			{
				itemsHeld[0] = 2;
				numWeapon = 100;
				SetUpLaser();
				
				laserPic.gameObject.SetActive(true);
				ammoPic.gameObject.SetActive(false);
				rocketPic.gameObject.SetActive(false);
			}			
			else if ( randWeapon == 2 )
			{
				itemsHeld[0] = 3;
				numWeapon = 10;
				rocketPic.gameObject.SetActive(true);
				ammoPic.gameObject.SetActive(false);
				laserPic.gameObject.SetActive(false);
			}
			weaponCounter.text = numWeapon.ToString();
			weaponCounter.gameObject.SetActive(true);
			collider.gameObject.SetActive(false);
			Destroy(collider.gameObject);
		}
		if( collider.gameObject.tag == "Defense" )
		{
			int randWeapon = Mathf.Abs(((int)(Random.insideUnitCircle.x*1000)))%2;//Mathf.Abs((int)(Random.insideUnitCircle.x*100)) % 3;
			
			if( randWeapon == 0 ){
				itemsHeld[1] = 1;
				numDefense = 1;
				shieldPic.gameObject.SetActive(true);
				boosterPic.gameObject.SetActive(false);
			}
			else if ( randWeapon == 1 ) {
				itemsHeld[1] = 2;
				numDefense = 50;
				boosterPic.gameObject.SetActive(true);
				shieldPic.gameObject.SetActive(false);
			}
			
			defenseCounter.text = numDefense.ToString();
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