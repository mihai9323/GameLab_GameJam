using UnityEngine;
using System.Collections;

public class GenerateLevel : MonoBehaviour {

	public float blockSize;
	public int mapX,mapZ;
	public float height;

	public GameObject wallContainer;
	public GameObject destructableWallContainer;
	public GameObject pathContainer;

	public GameObject[] Wall;
	public GameObject[] Path;
	public GameObject[] DestructableWall;


	public GameObject camera;
	public float CameraOffSet;
	public float CameraHeight = 1.0f;
	public float CameraAngle = 7;
	public static Tile[,] map;
	public static Vector2 playerPosition=new Vector2(6,6);
	public byte face = 0;
	private NetworkInterface NI;
	private Vector3 destination;

	private bool delayOver;
	// Use this for initialization
	void Awake () {
		delayOver = true;
	}
	void Start(){
		CreateMap ();
	}
	public void CreateMap(){ //face : 0 = north 1 = east 2=south 3=west
		Vector3 position;
		if (Network.isServer) {
			position = new Vector3 (playerPosition.x, 0, playerPosition.y);
		} else {
			bool found = false;
			while(!found){
				int x = (int)(Random.value * (mapX-8))+4;
				int z = (int)(Random.value * (mapZ-8))+4;
				if(x!=playerPosition.x && z!=playerPosition.y){
					if(x %2 == 0 && z%2 ==0){
						face = 0;
						found = true;
						playerPosition = new Vector2(x,z);

					}
				}
			}
			position = new Vector3 (playerPosition.x, 0, playerPosition.y);
		}
		StartCoroutine ("GenerateMap");
		switch (face) {
		case 0: camera.transform.position = position +new Vector3(0,CameraHeight,-CameraOffSet); camera.transform.rotation = Quaternion.Euler (CameraAngle,0,0); break;
		case 1: camera.transform.position = position +new Vector3(-CameraOffSet,CameraHeight,0); camera.transform.rotation = Quaternion.Euler (CameraAngle,90,0); break;
		case 2: camera.transform.position = position +new Vector3(0,CameraHeight,CameraOffSet); camera.transform.rotation = Quaternion.Euler (CameraAngle,180,0); break;
		case 3: camera.transform.position = position +new Vector3(CameraOffSet,CameraHeight,0); camera.transform.rotation = Quaternion.Euler (CameraAngle,270,0); break;
		}
	}
	// Update is called once per frame
	void Update () {
		Debug.Log (face);
		if (Input.GetKeyUp (KeyCode.W) && delayOver) {
			MoveForward ();
			StartCoroutine(delay (2.0f));
		}

		if (Input.GetKeyUp (KeyCode.A)&& delayOver) {
			MoveLeft ();
			StartCoroutine(delay (2.0f));
		}
		if (Input.GetKeyUp (KeyCode.D)&& delayOver) {
			MoveRight();
			StartCoroutine(delay (2.0f));
		}
	}

	IEnumerator GenerateMap(){
		map = new Tile[mapX,mapZ];
		Debug.Log (mapX+" "+mapZ);

		for(int x=0; x<mapX; x++){
			for(int z=0; z<mapZ; z++){

				if(x%2 == 0 || z%2 == 0){
					//path
					GameObject go = Instantiate (Path[(int)(Random.value * Path.Length)],new Vector3(x*blockSize,height,z*blockSize),Quaternion.identity) as GameObject;
					go.transform.parent = pathContainer.transform;
					map[x,z] = new Tile(Tile.TileType.path,go);
				}else{
					//wall
					GameObject go = Instantiate (Wall[(int)(Random.value * Wall.Length)],new Vector3(x*blockSize,height,z*blockSize),Quaternion.identity) as GameObject;
					go.transform.parent = wallContainer.transform;
					map[x,z] = new Tile(Tile.TileType.wall,go);
				}
				yield return new WaitForSeconds(0.001f);
			}
		}
	}
	void MoveForward(){
		if(face == 0) iTween.MoveTo(gameObject,new Vector3(Mathf.Round(transform.position.x),Mathf.Round(transform.position.y),Mathf.Round(transform.position.z))+new Vector3(0,0,-2),2.0f);
		if(face == 1) iTween.MoveTo(gameObject,new Vector3(Mathf.Round(transform.position.x),Mathf.Round(transform.position.y),Mathf.Round(transform.position.z))+new Vector3(-2,0,0),2.0f);
		if(face == 2) iTween.MoveTo(gameObject,new Vector3(Mathf.Round(transform.position.x),Mathf.Round(transform.position.y),Mathf.Round(transform.position.z))+new Vector3(0,0,2),2.0f);
        if(face == 3) iTween.MoveTo(gameObject,new Vector3(Mathf.Round(transform.position.x),Mathf.Round(transform.position.y),Mathf.Round(transform.position.z))+new Vector3(2,0,0),2.0f);
	}
	void MoveLeft(){
		if (face == 0) {
			face = 3;
			Vector3[] path = new Vector3[2];
			path[0] = transform.position;
			path[1] = path[0]+new Vector3(0,0,-2);
			//path[2] = path[1]+new Vector3(1,0,0);
			Hashtable ht= new Hashtable();
			ht.Add("path",path);
			ht.Add ("speed",3);
			iTween.MoveTo (gameObject,ht);
			iTween.RotateTo(camera,new Vector3(CameraAngle,270,0),3);
		}else
		if (face == 1) {
			face = 0;
			Vector3[] path = new Vector3[2];
			path[0] =  transform.position;
			path[1] = path[0]+new Vector3(-2,0,0);
			//path[2] = path[1]+new Vector3(0,0,-1);
			Hashtable ht= new Hashtable();
			ht.Add("path",path);
			ht.Add ("speed",3);
			iTween.MoveTo (gameObject,ht);

			iTween.RotateTo(camera,new Vector3(CameraAngle,0,0),3);
		}else
		if (face == 2) {
			face = 1;
			Vector3[] path = new Vector3[2];
			path[0]= transform.position;
			path[1] =path[0]+new Vector3(0,0,2);
			//path[2] =path[1]+new Vector3(-1,0,0);
			Hashtable ht= new Hashtable();
			ht.Add("path",path);
			ht.Add ("speed",3);
			iTween.MoveTo (gameObject,ht);
			iTween.RotateTo(camera,new Vector3(CameraAngle,90,0),3);
		}else
		if (face == 3) {
			face = 2;
			Vector3[] path = new Vector3[2];
			path[0]=  transform.position;
			path[1] = path[0]+new Vector3(2,0,0);
		//	path[2] = path[1]+new Vector3(0,0,1);
			Hashtable ht= new Hashtable();
			ht.Add("path",path);
			ht.Add ("speed",3);
			iTween.MoveTo (gameObject,ht);
			iTween.RotateTo(camera,new Vector3(CameraAngle,180,0),3);
		}
	}
	void MoveRight(){
		if (face == 0) {
			face = 1;
			Vector3[] path = new Vector3[2];
			path[0]= transform.position;
			path[1] = path[0]+new Vector3(0,0,-2);
			//path[2] = path[1]+new Vector3(-1,0,0);
			Hashtable ht= new Hashtable();
			ht.Add("path",path);
			ht.Add ("speed",3);
			iTween.MoveTo (gameObject,ht);
			iTween.RotateTo(camera,new Vector3(CameraAngle,90,0),3);
		}else
		if (face == 1) {
			face = 2;
			Vector3[] path = new Vector3[2];
			path[0]=  transform.position;
			path[1] = path[0]+new Vector3(-2,0,0);
			//path[2] = path[1]+new Vector3(0,0,1);
			Hashtable ht= new Hashtable();
			ht.Add("path",path);
			ht.Add ("speed",3);
			iTween.MoveTo (gameObject,ht);
			iTween.RotateTo(camera,new Vector3(CameraAngle,180,0),3);
		}else
		if (face == 2) {
			face = 3;
			Vector3[] path = new Vector3[2];
			path[0]= transform.position;
			path[1] = path[0]+new Vector3(0,0,2);
		//	path[2] = path[1]+new Vector3(1,0,0);
			Hashtable ht= new Hashtable();
			ht.Add("path",path);
			ht.Add ("speed",3);
			iTween.MoveTo (gameObject,ht);
			iTween.RotateTo(camera,new Vector3(CameraAngle,270,0),3);
		}else
		if (face == 3) {
			face = 0;
			Vector3[] path = new Vector3[2];
			path[0]=  transform.position;
			path[1] = path[0]+new Vector3(2,0,0);
		//	path[2] = path[1]+new Vector3(0,0,-1);
			Hashtable ht= new Hashtable();
			ht.Add("path",path);
			ht.Add ("speed",3);
			iTween.MoveTo (gameObject,ht);
			iTween.RotateTo(camera,new Vector3(CameraAngle,0,0),3);
		}
	}

	IEnumerator delay(float time){
		delayOver = false;
		yield return new WaitForSeconds (time);
		delayOver = true;
		}
}
