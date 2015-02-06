using UnityEngine;
using System.Collections;

public class YellowCar : MonoBehaviour {

	private float vel = 0;
	public int maxSpeed;
	private int revSpeed = 8;
	private bool is_colliding = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKey("up") && !is_colliding)
		{
			vel++;
		}
		else if(Input.GetKey("down"))
		{
			vel--;
		}
		else
		{
			if( vel > 0)
				vel -= 0.1f;
			else if( vel < 0)
				vel += 0.1f;
		}


		if( Input.GetKey("left") && Mathf.Abs(vel) > 4)
		{
			transform.Rotate(Vector3.up, -1);
		}
		else if( Input.GetKey("right") && Mathf.Abs(vel) > 4)
		{
			transform.Rotate(Vector3.up, 1);
		}

		if( vel > maxSpeed) vel = maxSpeed;
		else if( vel < -revSpeed) vel = -revSpeed;

		if( Input.GetKey("space"))
		{
			if(vel > 0) vel = vel - 0.5f;
			else if(vel < 0) vel = vel + 0.5f;
		}



		transform.Translate(Vector3.forward * Time.deltaTime * vel);

	
	}

	void OnCollisionEnter(Collision other)
	{
		if( other.gameObject.layer == 8)
		{
			print ("hit a fence");

			   vel = -1;
			is_colliding = true;
		}
		else
		{
			is_colliding = false;
		}

	}

	void OnCollisionExit(Collision other)
	{
		is_colliding = false;
	}

}
