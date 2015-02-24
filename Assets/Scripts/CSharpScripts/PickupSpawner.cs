using UnityEngine;
using System.Collections;

public class PickupSpawner : MonoBehaviour {
	
	
	public float spawnInterval = 3f;
	public Transform[] spawnPoints;
	public GameObject[] spawnItem; 

	
	
	bool drawItem1 = false;
	// Use this for initialization
	void Start () {
		Random.seed = 42;
		InvokeRepeating("SpawnPickups", spawnInterval, spawnInterval);
	}
	
	
	void Update () 
	{
	}
	
	void SpawnPickups () 
	{
		for( int i = 0; i < spawnPoints.Length; i++ ) 
		{
			int anyPossiblePickup = Mathf.Abs((int)(Random.insideUnitCircle.x*100)) % spawnItem.Length;
			if( ! Physics.CheckSphere (spawnPoints[i].position, 1.0f ) ) 
			{
				Instantiate (spawnItem[anyPossiblePickup], spawnPoints[i].position, spawnPoints[i].rotation);
			}
		}
	}
}
