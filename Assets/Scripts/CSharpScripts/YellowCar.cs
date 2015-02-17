using UnityEngine;
using System.Collections;

public class YellowCar : MonoBehaviour {

	Transform waypoints;
	int vel;
	int i;
	int maxSpeed;
	int revSpeed;

	// Use this for initialization
	void Start () {
		maxSpeed = 100;
		vel = 0;
		revSpeed = 8;
		waypoints = (GameObject.FindWithTag ("waypoints")).transform;
		i = 0;
	}

	// Update is called once per frame
	void LateUpdate () {
		if(vel < maxSpeed) vel++;
		Transform waypoint = waypoints.GetChild (i);
		Vector3 targetDir = new Vector3(waypoint.position.x - transform.position.x, 0,
		                                waypoint.position.z - transform.position.z);
		float step = vel * Time.deltaTime;
		var q = Quaternion.LookRotation(waypoint.position - transform.position);
		transform.rotation = Quaternion.RotateTowards(transform.rotation, q, step);
		transform.position = Vector3.MoveTowards(transform.position, waypoint.position, step);
		if(targetDir == Vector3.zero && i < waypoints.childCount) i++;
		if (i == waypoints.childCount) i = 0;

		if( vel > maxSpeed) vel = maxSpeed;
		else if( vel < -revSpeed) vel = -revSpeed;

		if( Input.GetKey("space"))
		{
			if(vel > 0) vel--;
			else if(vel < 0) vel++;
		}
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
