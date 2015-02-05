using UnityEngine;
using System.Collections;

public class YellowCar : MonoBehaviour {

	int vel = 0;
	public int maxSpeed;
	int revSpeed = 8;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKey("up"))
		{
			vel++;
		}
		else if(Input.GetKey("down"))
		{
			vel--;
		}

		if( Input.GetKey("left") && vel != 0)
		{
			transform.Rotate(Vector3.up, -1);
		}
		else if( Input.GetKey("right") && vel != 0)
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
		if( other.gameObject.layer == 8)
		{
			vel = 0;
		}
	}

}
