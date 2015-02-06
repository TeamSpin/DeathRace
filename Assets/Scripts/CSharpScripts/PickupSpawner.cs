using UnityEngine;
using System.Collections;

public class PickupSpawner : MonoBehaviour {
	
	
	public float spawnInterval = 3f;
	public Transform[] spawnPoints;
	public GameObject[] spawnItem; 
	
	bool drawItem1 = false;
	// Use this for initialization
	void Start () {
		InvokeRepeating("SpawnPickups", spawnInterval, spawnInterval);
	}
	
	void Update () 
	{
		
	}
	
	void SpawnPickups () 
	{
		for( int i = 0; i < spawnPoints.Length; i++ ) 
		{
			if( ! Physics.CheckSphere (spawnPoints[i].position, 1.0f ) ) 
			{
				if( drawItem1 )
				{
					Instantiate (spawnItem[0], spawnPoints[i].position, spawnPoints[i].rotation);
					drawItem1 = false;
				}
				else 
				{
					Instantiate (spawnItem[1], spawnPoints[i].position, spawnPoints[i].rotation);
					drawItem1 = true;
				}
			}
		}
	}
}
