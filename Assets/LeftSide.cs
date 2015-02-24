using UnityEngine;
using System.Collections;

public class LeftSide : MonoBehaviour {

	bool is_colliding = false;

	// Use this for initialization
	void Start () {
		is_colliding = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	bool Is_Colliding()
	{
		return is_colliding;
	}

	void OnCollisionEnter( Collider other)
	{
		if(other.gameObject.layer == 8)
		{
		is_colliding = true;
		//get parent
		YellowCar yc = gameObject.GetComponentInParent<YellowCar>();
		yc.collided_on_left();
		}
	}

	void OnColliseionExit( Collider other)
	{
		is_colliding = false;
	}
}
