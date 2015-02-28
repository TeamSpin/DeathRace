using UnityEngine;
using System.Collections;

public class NewCarCam : MonoBehaviour {

	public GameObject target;
	public GameObject targetLeft;
	public GameObject targetRight;
	public GameObject target_car;

	private float camera_dampening;
	private float turn_speed;

	// Use this for initialization
	void Start () {
		transform.LookAt(target_car.transform);
		camera_dampening = 5f;
		turn_speed = 14;
	}
	
	// Update is called once per frame
	void Update () {

		YellowCar yc = target_car.GetComponent("YellowCar") as YellowCar;
		float car_speed = yc.get_speed();
		print ( "vel magnitude : " + car_speed);
		GameObject curr_target = target;
		if(  Input.GetKey("left") || (Input.GetKey("a") && car_speed < turn_speed && !Input.GetKey("d")) )
			curr_target = targetLeft;
		else if( Input.GetKey("right")||(Input.GetKey("d") && car_speed < turn_speed) )
			curr_target = targetRight;
		else curr_target = target;

		Vector3 target_pos = new Vector3(curr_target.transform.position.x, 
		                       curr_target.transform.position.y + 2,curr_target.transform.position.z);
		//turn_speed = 1 / car_speed;
		transform.position = Vector3.Lerp( transform.position, target_pos, 20f * Time.deltaTime);

		//used to slow down the camera rotation when the car turns
		var curr_rotation = Quaternion.LookRotation(target_car.transform.position
		                                            - transform.position);
		transform.rotation = Quaternion.Slerp( transform.rotation, 
		                                      curr_rotation,  camera_dampening*Time.deltaTime );

	}

	void LateUpdate()
	{

	}

}
