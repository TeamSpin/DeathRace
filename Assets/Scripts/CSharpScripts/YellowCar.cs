using UnityEngine;
using System.Collections;

public class YellowCar : MonoBehaviour {

	public Transform waypoint;
	public Transform waypoint2;
	int vel = 0;
	public int maxSpeed;
	int revSpeed = 8;
	bool way1;
	bool way2;

	// Use this for initialization
	void Start () {
		way1 = true;
		way2 = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(way1){
			Vector3 targetDir = waypoint.position - transform.position;
			float step = maxSpeed * Time.deltaTime;
			Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
			transform.rotation = Quaternion.LookRotation(newDir);
			transform.position = Vector3.MoveTowards(transform.position, waypoint.position, step);
		}
		if(way2){
		}

		if( vel > maxSpeed) vel = maxSpeed;
		else if( vel < -revSpeed) vel = -revSpeed;

		if( Input.GetKey("space"))
		{
			if(vel > 0) vel--;
			else if(vel < 0) vel++;
		}



		transform.Translate(Vector3.forward * Time.deltaTime * vel);

	
	}

	void OnCollision(Collider other)
	{
		if( other.gameObject.tag == "Fence")
		{
			if( vel > 0) vel = -1;
			else if( vel < 0) vel = 1;
		}
	}

}
