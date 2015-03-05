//Andy Hollist

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class YellowCar : MonoBehaviour {


	public int maxSpeed;
	public GameObject myHUD;


	private int off_track_speed;
	private float revSpeed;
	private float vel = 0;
	private bool on_ground = true;
	private bool off_road = false;
	private float curr_max_speed;
	private float curr_wheel_angle_left;
	private float curr_wheel_angle_right;
	private float max_wheel_angle;
	private float boost_time;
	private bool is_boosting;
	private float off_track_speed_boost;

	private float curr_accel;
	private float normal_accel;
	private float boost_accel;

	private bool cntrl_left;
	private bool cntrl_right;
	private bool cntrl_accel;
	private bool cntrl_brake;

	private bool on_ramp = false;

	private int num_collisions = 0;

	// Use this for initialization
	void Start () {
		off_track_speed = 8;
		off_track_speed_boost = 16;
		curr_max_speed = maxSpeed;
		revSpeed = curr_max_speed / 3;
		curr_wheel_angle_left = 0;
		curr_wheel_angle_right = 0;
		max_wheel_angle = 30;

		normal_accel = 0.1f;
		boost_accel = 0.3f;
		curr_accel = normal_accel;

		off_road = false;

	}

	public float get_speed()
	{
		return Mathf.Abs (vel);
	}

	public void set_controller_speed( bool accel, bool brake)
	{
		cntrl_accel = accel;
		cntrl_brake = brake;
	}

	public void set_controller_turn( bool left, bool right)
	{
		cntrl_left = left;
		cntrl_right = right;
	}

	public void speed_boost()
	{
		boost_time = (0.0f);
	}

	// Update is called once per frame
	void Update () {

		if( num_collisions == 0) //to avoid the fence collision problem, dont include fences in the count
		{
			on_ground = false;
		}
		else on_ground = true;

		boost_time += Time.deltaTime;

		if(boost_time < 0.1f && !off_road)
		{
			curr_max_speed += 0.1f;
			curr_accel = boost_accel;
		}
		else if(boost_time >= 0.1f )
		{
			if( curr_max_speed > maxSpeed)
				curr_max_speed -= 1f;
			curr_accel = normal_accel;
		}

		//update velocity
		     //you are pushing the gas           you are on the ground    you are not pushing the brakes
		if( (cntrl_accel ||  Input.GetKey("w")) && on_ground && !(cntrl_brake ||Input.GetKey("s")) )
		{
			vel += curr_accel;
		}
		//brake or reverse
		else if((cntrl_brake ||  Input.GetKey("s")) && on_ground)
		{
			if( vel > 0)
				vel -= curr_accel*2;
			else if( vel <= 0)
				vel -= curr_accel;
		}
		else if( on_ground)//drag
		{
			if( vel > 0)
				vel -= 0.05f;
			else if( vel < 0)
				vel += 0.05f;
		}

		//adjust slower turning rate at high speeds
		float rotation_angle = 1.0f;
		if(Mathf.Abs(vel) > 15) rotation_angle = 0.8f;
		if(Mathf.Abs(vel) > 25) rotation_angle = 0.7f;
		if(Mathf.Abs(vel) > 35) rotation_angle = 0.5f;
		if(Mathf.Abs(vel) > 45) rotation_angle = 0.3f;
		//steering input
		if( ( cntrl_left || Input.GetKey("a") ) && Mathf.Abs(vel) > 2 && on_ground)
		{

			if( vel > 0)
				transform.Rotate(Vector3.up, -1  * rotation_angle );
			else if(vel < 0)//other direction if reversing
				transform.Rotate(Vector3.up, 1  * rotation_angle );
		}
		else if( (cntrl_brake||Input.GetKey("d")) && Mathf.Abs(vel) > 2 && on_ground)
		{
			if( vel > 0)
				transform.Rotate(Vector3.up , 1 * rotation_angle );
			else if(vel < 0)//other direction if reversing
				transform.Rotate(Vector3.up , -1 * rotation_angle );
		}

		if( vel > curr_max_speed) vel = curr_max_speed;
		else if( vel < -revSpeed) vel = -revSpeed;

		//brakes
		if( Input.GetKey("space") && on_ground )
		{
			if( vel > -1 && vel < 1) vel = 0;
			else if(vel > 0) vel = vel - 0.2f;
			else if(vel < 0) vel = vel + 0.2f;
		}


		if(on_ground)
		{
			transform.Translate(Vector3.forward * Time.deltaTime * vel);
		}
		else
		{
			//rigidbody.velocity = transform.forward * vel;
			transform.Translate(Vector3.forward * Time.deltaTime * vel);
			/*if(vel > 0)
				vel -= 0.1f;
			transform.Translate(new Vector3(0,-1,0) *Time.deltaTime);*/

		}

		//output speed
		UnityEngine.UI.Text s = myHUD.GetComponentInChildren<Text>();
		//s.text = "Lap " + lapNum.ToString() + " / " + maxLaps.ToString();
		int speed = (int)Mathf.Abs(vel);
		//print (speed.ToString());
		s.text = speed.ToString() + " mph";

		//animate the wheels
		var wheel = transform.Find("CC_ME_Wheel_BR");
		wheel.Rotate(Vector3.right, vel);
		wheel = transform.Find("CC_ME_Wheel_FR");
		wheel.Rotate(Vector3.right, vel);

		//front wheel steering

		//front right wheel only

		Vector3 pos = wheel.position;
		wheel.position = Vector3.zero;
		if( cntrl_left || Input.GetKey("a") ){
			//rotate to the left all at once
			//print ("left key, cur angle = " + curr_wheel_angle_right);
			if( curr_wheel_angle_right == 0)
				wheel.RotateAround( wheel.renderer.bounds.center, this.transform.up,- max_wheel_angle);
			else if( curr_wheel_angle_right == max_wheel_angle)
				wheel.RotateAround( wheel.renderer.bounds.center, this.transform.up,-2 * max_wheel_angle);

			curr_wheel_angle_right = -max_wheel_angle;

			//sanity check for the steering angle



		}
		else if( cntrl_right||Input.GetKey ("d" ) ){
			if( curr_wheel_angle_right == 0)
				wheel.RotateAround( wheel.renderer.bounds.center, this.transform.up, max_wheel_angle);
			else if( curr_wheel_angle_right == -max_wheel_angle)
				wheel.RotateAround( wheel.renderer.bounds.center, this.transform.up,2 * max_wheel_angle);
			
			curr_wheel_angle_right = max_wheel_angle;
		}
		else {

			if( curr_wheel_angle_right == -max_wheel_angle)
				wheel.RotateAround( wheel.renderer.bounds.center, this.transform.up, max_wheel_angle);
			else if( curr_wheel_angle_right == max_wheel_angle)
				wheel.RotateAround( wheel.renderer.bounds.center, this.transform.up, -max_wheel_angle);
			curr_wheel_angle_right = 0;

			//sanity check for the steering angle
			//print ("Wheel right vector : " + wheel.right.x * 10 + " " + wheel.right.y * 10 + " " + wheel.right.z * 10);

			//if( wheel.right != Vector3.right)
			{
			//	wheel.right = Vector3.right;
			}


		}
		wheel.position = pos;
		


		wheel = transform.Find("CC_ME_Wheel_BL");
		wheel.Rotate(Vector3.right, vel);
		wheel = transform.Find("CC_ME_Wheel_FL");
		wheel.Rotate(Vector3.right, vel);
		//front wheel steering

		//front left wheel only

		pos = wheel.position;
		wheel.position = Vector3.zero;
		if( cntrl_left || Input.GetKey("a") ){
			//rotate to the left all at once

			if( curr_wheel_angle_left == 0)
				wheel.RotateAround( wheel.renderer.bounds.center,this.transform.up,- max_wheel_angle);
			else if( curr_wheel_angle_left == max_wheel_angle)
				wheel.RotateAround( wheel.renderer.bounds.center, this.transform.up,-2 * max_wheel_angle);
			
			curr_wheel_angle_left = -max_wheel_angle;

			//sanity check for the steering angle
		}
		else if( cntrl_right||Input.GetKey ("d") ){
			if( curr_wheel_angle_left == 0)
				wheel.RotateAround( wheel.renderer.bounds.center, this.transform.up, max_wheel_angle);
			else if( curr_wheel_angle_left == -max_wheel_angle)
				wheel.RotateAround( wheel.renderer.bounds.center, this.transform.up,2 * max_wheel_angle);
			
			curr_wheel_angle_left = max_wheel_angle;
		}
		else {
			if( curr_wheel_angle_left == -max_wheel_angle)
				wheel.RotateAround( wheel.renderer.bounds.center, this.transform.up, max_wheel_angle);
			else if( curr_wheel_angle_left == max_wheel_angle)
				wheel.RotateAround( wheel.renderer.bounds.center, this.transform.up, -max_wheel_angle);
			curr_wheel_angle_left = 0;
		}
		wheel.position = pos;
	
	}

	public void collided_on_left()
	{
		transform.Translate( Vector3.right * Time.deltaTime);
		vel--;
	}

	public void collided_on_right()
	{
		transform.Translate( Vector3.left * Time.deltaTime);
		vel--;
	}

	public void collided_on_front()
	{
		transform.Translate( Vector3.back * Time.deltaTime * 5);
		vel = -5;
	}
	public void collided_on_back()
	{
		transform.Translate( Vector3.forward * Time.deltaTime);
		vel = 5;
	}

	void OnCollisionStay(Collision other)
	{
//		if(other == ground.collider)
//			on_ground = true;
		/*if( other.gameObject.layer == 12)
		{
			print ("collided with road");
			curr_max_speed = maxSpeed;
			revSpeed = curr_max_speed / 3;
			off_road = false;
		}*/

	}

	void OnCollisionExit(Collision other)
	{
		num_collisions--;

//		if(other == ground.collider)
//			on_ground = false;
		//layer 12 == road
		//exiting the road
		if( other.gameObject.layer == 12  && !on_ramp && boost_time >= 0.1f)
		{
			print ("exiting the road");
			curr_max_speed = off_track_speed;
			revSpeed = curr_max_speed / 3;
			off_road = true;
		}
		else if( other.gameObject.layer == 12  && !on_ramp && boost_time < 0.1f)
		{
			print ("exiting the road");
			curr_max_speed = off_track_speed_boost;
			revSpeed = curr_max_speed / 3;
			off_road = true;
		}

		//layer 11 is grass
		//exiting the grass
		if( other.gameObject.layer == 11)
		{
			print ("exiting the grass");
			curr_max_speed = maxSpeed;
			revSpeed = curr_max_speed / 3;
			off_road = false;
		}

		//ramp
		if(other.gameObject.layer == 13)
		{
			on_ramp = false;
		}

	}

	void OnCollisionEnter(Collision other)
	{
		num_collisions++;
		//entering the road
		if( other.gameObject.layer == 12)
		{
			print ("entered the road, max speed = " + maxSpeed);
			off_road = false;
			curr_max_speed = maxSpeed;
			revSpeed = curr_max_speed / 3;
		}

		//ramp
		else if( other.gameObject.layer == 13)
		{
			off_road = false;
			on_ramp = true;
		}
		//layer 11 is grass
		//heading off road
//		else if(other.gameObject.layer == 11 && boost_time >= 0.1f)
//		{
//			print ("entered the grass");
//			curr_max_speed = off_track_speed;
//			revSpeed = curr_max_speed / 3;
//			off_road = true;
//		}
//
//		//you are currently boosting
//		else if( other.gameObject.layer == 11 && boost_time < 0.1f)
//		{
//			print ("entered the grass");
//			curr_max_speed = off_track_speed_boost;
//			revSpeed = curr_max_speed / 3;
//			off_road = true;
//		}


	}


}
