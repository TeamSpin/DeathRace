using UnityEngine;
using System.Collections;

public class AI : MonoBehaviour {
	public Transform[] waypoints;
	float waypointRadius = 3.0f;
	float damping = 0.1f;
	bool loop = false;
	float speed = 2.0f;
	bool faceHeading = true;
	private Vector3 targetHeading;
	private Vector3 currentHeading;
	private int targetwaypoint;
	private Transform xform; 
	private bool useRigidbody; 
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
		if(rigidbody!=null) { 
			useRigidbody = true; 
			rigidmember = rigidbody; 
		} 
		else { useRigidbody = false; }
	}

	void FixedUpdate() { 
		targetHeading = waypoints[targetwaypoint].position - xform.position;
		currentHeading = Vector3.Slerp(currentHeading,targetHeading,damping);
	}

	// Update is called once per frame
	void Update () {
		if(useRigidbody)
			rigidmember.velocity = currentHeading * speed;
		else
			xform.position +=currentHeading * Time.deltaTime * speed;
		if(faceHeading)
			xform.LookAt(xform.position+currentHeading);
//		Debug.Log (Vector3.Distance(xform.position,waypoints[targetwaypoint].position));
//		Debug.Log (waypointRadius);
//		Debug.Log (targetwaypoint);
		if(Vector3.Distance(xform.position,waypoints[targetwaypoint].position)<=waypointRadius)
		{
			targetwaypoint++;
			if(targetwaypoint>=waypoints.Length)
			{
				targetwaypoint = 0;
//				if(!loop)
//					enabled = false;
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
