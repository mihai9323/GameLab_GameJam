using UnityEngine;
using System.Collections;

public class TileMover : MonoBehaviour {
	public int north,east,south,west;
	// Use this for initialization
	void Start () {
		Performance.EnterFrame += moveOnExit ;
	}
	void OnDestroy(){
		Performance.EnterFrame -= moveOnExit;
	}
	public void moveOnExit(){
		if (transform.position.x > east) {
			transform.position = new Vector3(west+(transform.position.x - east),transform.position.y,transform.position.z);
		}
		if (transform.position.x < west) {
			transform.position = new Vector3(east-(west-transform.position.x),transform.position.y,transform.position.z);
		}
		if (transform.position.z > north) {
			transform.position = new Vector3(transform.position.x,transform.position.y,south+(transform.position.z - north));
		}
		if (transform.position.z < south) {
			transform.position = new Vector3(transform.position.x,transform.position.y,north-(south- transform.position.z));
		}
	}



}
