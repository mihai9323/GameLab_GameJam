using UnityEngine;
using System.Collections;

public class MazeObject : MonoBehaviour {
	public enum TypeOfObject{
		none,monster,treasure,wall
	}
	public TypeOfObject objectType;
	float destroyAfterSeconds = 5.0f;
	GameObject player;
	// Use this for initialization
	void Start () {
		if(destroyAfterSeconds>0)	StartCoroutine (kill());
	}
	
	// Update is called once per frame
	void Update () {
		transform.LookAt (player.transform.position);
	}
	IEnumerator kill(){
		yield return new WaitForSeconds (destroyAfterSeconds);
		Destroy (this.gameObject);
	}
	void OnTriggerEnter(){
		if (objectType == TypeOfObject.monster) {
			GameObject.Find ("Map").GetComponent<GenerateLevel>().died(this.gameObject);
				Debug.Log ("Lost");

		}else if(objectType == TypeOfObject.treasure){
			GameObject.Find ("Map").GetComponent<GenerateLevel>().takeTreasure(this.gameObject);
		}else if(objectType == TypeOfObject.wall){
			GameObject.Find ("Map").GetComponent<GenerateLevel>().destroyWall(this.gameObject);
		}
	}
}
