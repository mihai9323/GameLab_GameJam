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

	public static Tile[,] map;
	public static Vector2 playerPosition=new Vector2(2,3);
	public byte face = 0;
	private NetworkInterface NI;
	// Use this for initialization
	void Awake () {

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
				int x = (int)(Random.value * (mapX-3))+2;
				int z = (int)(Random.value * (mapZ-3))+2;
				if(x!=playerPosition.x && z!=playerPosition.y){
					if(x %2 == 0 && z%2 ==1){
						face = 0;
						found = true;
						playerPosition = new Vector2(x,z);

					}
					if(x %2 == 1 && z%2 ==0){
						face = 3;
						found = true;
						playerPosition = new Vector2(x,z);

					}

				}
			}
			position = new Vector3 (playerPosition.x, 0, playerPosition.y);
		}
		StartCoroutine ("GenerateMap");
		switch (face) {
		case 0: camera.transform.position = position +new Vector3(0,CameraHeight,-CameraOffSet); camera.transform.rotation = Quaternion.Euler (0,0,0); break;
		case 1: camera.transform.position = position +new Vector3(-CameraOffSet,CameraHeight,0); camera.transform.rotation = Quaternion.Euler (0,90,0); break;
		case 2: camera.transform.position = position +new Vector3(0,CameraHeight,CameraOffSet); camera.transform.rotation = Quaternion.Euler (0,180,0); break;
		case 3: camera.transform.position = position +new Vector3(CameraOffSet,CameraHeight,0); camera.transform.rotation = Quaternion.Euler (0,270,0); break;
		}
	}
	// Update is called once per frame
	void Update () {
	
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
					map[x,z] = new Tile(Tile.TileType.path);
				}else{
					//wall
					GameObject go = Instantiate (Wall[(int)(Random.value * Wall.Length)],new Vector3(x*blockSize,height,z*blockSize),Quaternion.identity) as GameObject;
					go.transform.parent = wallContainer.transform;
					map[x,z] = new Tile(Tile.TileType.wall);
				}
				yield return new WaitForSeconds(0.001f);
			}
		}
	}
}
