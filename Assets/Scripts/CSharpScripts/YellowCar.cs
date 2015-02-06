using UnityEngine;
using System.Collections;

public class YellowCar : MonoBehaviour {

	public GameObject waypoint;
	int vel = 0;
	public int maxSpeed;
	int revSpeed = 8;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		float temp = Mathf.Abs(waypoint.rigidbody.position.z);
		float temp2 = Mathf.Abs (rigidbody.position.z);
		vel = (int)(((Mathf.Abs (temp - temp2)) / (temp + temp2)) * maxSpeed);
		if( waypoint.rigidbody.position.x <= rigidbody.position.x && vel != 0)
		{
			transform.Rotate(Vector3.up, -1);
		}
		else if( waypoint.rigidbody.position.x >= rigidbody.position.x && vel != 0)
		{
			transform.Rotate(Vector3.up, 1);
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
