using UnityEngine;
using System.Collections;

public class CarAI : MonoBehaviour {
	public Transform[] waypoints;
	float waypointRadius = 20.0f;
	float damping = 0.1f;
	float speed = 1.0f;
	float initSpeed = 0.0f;
	bool faceHeading = true;
	private Vector3 targetHeading;
	private Vector3 currentHeading;
	private int targetwaypoint;
	private Transform xform;
	private Rigidbody rigidmember;
	

	// Use this for initialization
	void Start () {
		xform = transform;
		currentHeading = xform.forward;
		if(waypoints.Length<=0) {
			Debug.Log("No waypoints on "+name);
			enabled = false;
		}
		targetwaypoint = 0;
		rigidmember = rigidbody;
	}
	
	void FixedUpdate() {
		targetHeading = waypoints[targetwaypoint].position - xform.position;
		currentHeading = Vector3.Slerp (currentHeading, targetHeading, damping);
		//   	 float changeX = (Mathf.Abs (waypoints [targetwaypoint].position.x - xform.position.x)) / xform.position.x;
		//   	 float changeZ = (Mathf.Abs (waypoints [targetwaypoint].position.z - xform.position.z)) / xform.position.z;
		//   	 if (changeX / changeZ > 0.5 && changeX / changeZ < 2)
		//   		 speed = 2.0f;
		//   	 else {
		//   		 speed = 1.5f;
		//   	 }
	}
	
	// Update is called once per frame
	void Update () {
		if (initSpeed < speed)
			initSpeed += 0.1f;
		if (initSpeed > speed)
			initSpeed = speed;
		xform.position +=currentHeading * Time.deltaTime * initSpeed;
		if(faceHeading)
			xform.LookAt(xform.position+currentHeading);
		//   	 Debug.Log (Vector3.Distance(xform.position,waypoints[targetwaypoint].position));
		//   	 Debug.Log (waypointRadius);
		//   	 Debug.Log (targetwaypoint);
		//   	 Debug.Log (rigidmember.velocity);
		//   	 Debug.Log (currentHeading.magnitude);
		//   	 Debug.Log (initSpeed);
		if(Vector3.Distance(xform.position,waypoints[targetwaypoint].position)<=waypointRadius)
		{
			targetwaypoint++;
			if(targetwaypoint>=waypoints.Length)
			{
				targetwaypoint = 0;
			}
		}
	}
	
	void OnDrawGizmos(){
		
		Gizmos.color = Color.red;
		for(int i = 0; i< waypoints.Length;i++)
		{
			Vector3 pos = waypoints[i].position;
			if(i>0)
			{
				Vector3 prev = waypoints[i-1].position;
				Gizmos.DrawLine(prev,pos);
			}
		}
	}
}
