﻿//Andy Hollist

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
	private bool off_road = true;
	private float curr_max_speed;
	private float curr_wheel_angle_left;
	private float curr_wheel_angle_right;
	private float max_wheel_angle;
	private float boost_time;
	private bool is_boosting;
	private float off_track_speed_boost;

	// Use this for initialization
	void Start () {
		off_track_speed = 8;
		off_track_speed_boost = 16;
		curr_max_speed = off_track_speed;
		revSpeed = curr_max_speed / 3;
		curr_wheel_angle_left = 0;
		curr_wheel_angle_right = 0;
		max_wheel_angle = 30;

	}

	public float get_speed()
	{
		return Mathf.Abs (vel);
	}


	public void speed_boost()
	{
		boost_time = (0.0f);
	}

	// Update is called once per frame
	void Update () {

		boost_time += Time.deltaTime;

		if(boost_time < 0.1f && !off_road) curr_max_speed += 0.1f;
		else if(boost_time >= 0.1f && curr_max_speed > maxSpeed) curr_max_speed -= 1f;

		//update velocity
		if(Input.GetKey("up") && on_ground)
		{
			vel += 0.3f;
		}
		else if(Input.GetKey("down") && on_ground)
		{
			vel -= 0.3f;
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
		if(Mathf.Abs(vel) > 15) rotation_angle = 0.7f;
		else if(Mathf.Abs(vel) > 25) rotation_angle = 0.2f;
		else if(Mathf.Abs(vel) > 35) rotation_angle = 0.05f;
		else if(Mathf.Abs(vel) > 45) rotation_angle = 0.01f;
		//steering input
		if( Input.GetKey("left") && Mathf.Abs(vel) > 2 && on_ground)
		{

			if( vel > 0)
				transform.Rotate(Vector3.up, -1  * rotation_angle);
			else if(vel < 0)//other direction if reversing
				transform.Rotate(Vector3.up, 1  * rotation_angle);
		}
		else if( Input.GetKey("right") && Mathf.Abs(vel) > 2 && on_ground)
		{
			if( vel > 0)
				transform.Rotate(Vector3.up , 1 * rotation_angle);
			else if(vel < 0)//other direction if reversing
				transform.Rotate(Vector3.up , -1 * rotation_angle);
		}

		if( vel > curr_max_speed) vel = curr_max_speed;
		else if( vel < -revSpeed) vel = -revSpeed;

		//brakes
		if( Input.GetKey("space") && on_ground )
		{
			if( vel > -1 && vel < 1) vel = 0;
			else if(vel > 0) vel = vel - 0.5f;
			else if(vel < 0) vel = vel + 0.5f;
		}


		if(on_ground)
		{
			transform.Translate(Vector3.forward * Time.deltaTime * vel);
		}

		//output speed
		UnityEngine.UI.Text s = myHUD.GetComponentInChildren<Text>();
		//s.text = "Lap " + lapNum.ToString() + " / " + maxLaps.ToString();
		int speed = (int)Mathf.Abs(vel);
		print (speed.ToString());
		s.text = speed.ToString() + " mph";

		//animate the wheels
		var wheel = transform.Find("CC_ME_Wheel_BR");
		wheel.Rotate(Vector3.right, vel);
		wheel = transform.Find("CC_ME_Wheel_FR");
		wheel.Rotate(Vector3.right, vel);

		//front wheel steering
		Vector3 pos = wheel.position;
		wheel.position = Vector3.zero;
		if( Input.GetKey("left") ){
			//rotate to the left all at once
			print ("left key, cur angle = " + curr_wheel_angle_right);
			if( curr_wheel_angle_right == 0)
				wheel.RotateAround( wheel.renderer.bounds.center, Vector3.up,- max_wheel_angle);
			else if( curr_wheel_angle_right == max_wheel_angle)
				wheel.RotateAround( wheel.renderer.bounds.center, Vector3.up,-2 * max_wheel_angle);

			curr_wheel_angle_right = -max_wheel_angle;

			//sanity check for the steering angle
		}
		else if( Input.GetKey ("right" ) ){
			if( curr_wheel_angle_right == 0)
				wheel.RotateAround( wheel.renderer.bounds.center, transform.up, max_wheel_angle);
			else if( curr_wheel_angle_right == -max_wheel_angle)
				wheel.RotateAround( wheel.renderer.bounds.center, transform.up,2 * max_wheel_angle);
			
			curr_wheel_angle_right = max_wheel_angle;
		}
		else {
			if( curr_wheel_angle_right == -max_wheel_angle)
				wheel.RotateAround( wheel.renderer.bounds.center, transform.up, max_wheel_angle);
			else if( curr_wheel_angle_right == max_wheel_angle)
				wheel.RotateAround( wheel.renderer.bounds.center, transform.up, -max_wheel_angle);
			curr_wheel_angle_right = 0;
		}
		wheel.position = pos;
		


		wheel = transform.Find("CC_ME_Wheel_BL");
		wheel.Rotate(Vector3.right, vel);
		wheel = transform.Find("CC_ME_Wheel_FL");
		wheel.Rotate(Vector3.right, vel);
		//front wheel steering
		pos = wheel.position;
		wheel.position = Vector3.zero;
		if( Input.GetKey("left") ){
			//rotate to the left all at once

			if( curr_wheel_angle_left == 0)
				wheel.RotateAround( wheel.renderer.bounds.center, Vector3.up,- max_wheel_angle);
			else if( curr_wheel_angle_left == max_wheel_angle)
				wheel.RotateAround( wheel.renderer.bounds.center, Vector3.up,-2 * max_wheel_angle);
			
			curr_wheel_angle_left = -max_wheel_angle;

			//sanity check for the steering angle
		}
		else if( Input.GetKey ("right" ) ){
			if( curr_wheel_angle_left == 0)
				wheel.RotateAround( wheel.renderer.bounds.center, transform.up, max_wheel_angle);
			else if( curr_wheel_angle_left == -max_wheel_angle)
				wheel.RotateAround( wheel.renderer.bounds.center, transform.up,2 * max_wheel_angle);
			
			curr_wheel_angle_left = max_wheel_angle;
		}
		else {
			if( curr_wheel_angle_left == -max_wheel_angle)
				wheel.RotateAround( wheel.renderer.bounds.center, transform.up, max_wheel_angle);
			else if( curr_wheel_angle_left == max_wheel_angle)
				wheel.RotateAround( wheel.renderer.bounds.center, transform.up, -max_wheel_angle);
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
		if( other.gameObject.layer == 12)
		{
			print ("collided with road");
			curr_max_speed = maxSpeed;
			revSpeed = curr_max_speed / 3;
			off_road = false;
		}
	}

	void OnCollisionExit(Collision other)
	{
//		if(other == ground.collider)
//			on_ground = false;
		if( other.gameObject.layer == 12 && boost_time >= 0.1f)
		{
			print ("off road");
			curr_max_speed = off_track_speed;
			revSpeed = curr_max_speed / 3;
			off_road = true;
		}
		else if( other.gameObject.layer == 12 && boost_time < 0.1f)
		{
			curr_max_speed = off_track_speed_boost;
			revSpeed = curr_max_speed / 3;
			off_road = true;
		}

	}


}
