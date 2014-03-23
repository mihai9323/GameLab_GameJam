using UnityEngine;
using System.Collections;

public class GenerateLevel : MonoBehaviour {













	public AlertController ac;



	public enum Status
		{
			CreateMap,WaitCreation,SuggestPath,WaitForSuggestion,ChooseMyPath,WaitForPath,MoveMe,WaitForMove
		}
	public enum ChosenArrow
		{
		none,left,right,forward
		}
	public SuggestionPannel sp;
	public Status GameState;
	public Status OpGameState;
	public ChosenArrow chosenArrow;
	public byte[] myArrows;

	private bool madeCombo;
	public float blockSize;
	public int mapX,mapZ;
	public float height;

	public GameObject wallContainer;
	public GameObject destructableWallContainer;
	public GameObject pathContainer;

	public GameObject[] Wall;
	public GameObject[] Path;
	public GameObject[] DestructableWall;

	private int turn;
	private int lastCombo;
	public GameObject camera;
	public float CameraOffSet;
	public float CameraHeight = 1.0f;
	public float CameraAngle = 7;
	public Tile[,] map;
	public Vector2 playerPosition=new Vector2(6,6);
	public byte face = 0;

	public Vector2 opPlayerPosition=new Vector2(6,6);
	public byte opFace = 0;

	public bool playTurn;
	public byte[] opArrows; // 0-empty 1-monster 2-treasure 3-wall

	public GameObject[] mazeObjects;

	public int gold =0;
	public int combo = 1;

	private NetworkInterface NI;
	private Vector3 destination;
	private Vector3 wallPosition;
	private bool delayOver;

	private Vector2 LastPosition;

	// Use this for initialization
	void Awake () {
		haveInfo = false;
		delayOver = false;
		GameState = Status.CreateMap;
		OpGameState = Status.CreateMap;
	}
	void Start(){
		CreateMap ();
	}
	public void CreateMap(){ //face : 0 = north 1 = east 2=south 3=west
		madeCombo = false;
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

			LastPosition = playerPosition;
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

	public void OpponentSetGround(byte[] arr){
		myArrows = arr;
		OpGameState = Status.WaitForSuggestion;
		if (GameState == Status.WaitForSuggestion) {
			GameState = Status.ChooseMyPath;

				
		}
	}
	public void OpponentSetPath(){
		OpGameState = Status.WaitForPath;
		if (GameState == Status.WaitForPath) {
			GameState = Status.MoveMe;
			
			
		}
	}


	void Update () {
		Debug.Log (GameState+" "+OpGameState);
		if (turn - lastCombo > 1)		combo = 1;
		if (GameState == Status.SuggestPath && haveInfo) {

						if (chosenArrow == ChosenArrow.none) {
								if (Input.GetKeyUp (KeyCode.W) && delayOver) {
										haveInfo = false;
										chosenArrow = ChosenArrow.forward;
										byte[] arr = new byte[4];
										arr[0] = opArrows[0];
										arr[1] = opArrows[1];
										arr[2] = opArrows[2];
										arr[3] = opArrows[1];
										GameObject.Find("NetworkInterface").GetComponent<NetworkInterface>().oSendGroundArrow(arr);
										GameState = Status.WaitForSuggestion;
										if(OpGameState == Status.WaitForSuggestion) GameState=Status.ChooseMyPath;
										chosenArrow = ChosenArrow.none;
								}
				
								if (Input.GetKeyUp (KeyCode.A) && delayOver) {
										haveInfo = false;
										chosenArrow = ChosenArrow.left;
										byte[] arr = new byte[4];
										arr[0] = opArrows[0];
										arr[1] = opArrows[1];
										arr[2] = opArrows[2];
										arr[3] = opArrows[0];
										GameObject.Find("NetworkInterface").GetComponent<NetworkInterface>().oSendGroundArrow(arr);
										GameState = Status.WaitForSuggestion;
										if(OpGameState == Status.WaitForSuggestion) GameState=Status.ChooseMyPath;
										chosenArrow = ChosenArrow.none;
								}
								if (Input.GetKeyUp (KeyCode.D) && delayOver) {
										haveInfo = false;
										chosenArrow = ChosenArrow.right;
										byte[] arr = new byte[4];
										arr[0] = opArrows[0];
										arr[1] = opArrows[1];
										arr[2] = opArrows[2];
										arr[3] = opArrows[2];
										GameObject.Find("NetworkInterface").GetComponent<NetworkInterface>().oSendGroundArrow(arr);
										GameState = Status.WaitForSuggestion;
										if(OpGameState == Status.WaitForSuggestion) GameState=Status.ChooseMyPath;
										chosenArrow = ChosenArrow.none;
								}
						}
				} else if (GameState == Status.ChooseMyPath) {
						if (chosenArrow == ChosenArrow.none) {
								if (Input.GetKeyUp (KeyCode.W) && delayOver) {
										chosenArrow = ChosenArrow.forward;
										GameState = Status.WaitForPath;
										if(OpGameState == Status.WaitForPath) GameState=Status.MoveMe;
										GameObject.Find("NetworkInterface").GetComponent<NetworkInterface>().oSendPath();
								}
				
								if (Input.GetKeyUp (KeyCode.A) && delayOver) {
										chosenArrow = ChosenArrow.left;
										GameState = Status.WaitForPath;
										if(OpGameState == Status.WaitForPath) GameState=Status.MoveMe;
										GameObject.Find("NetworkInterface").GetComponent<NetworkInterface>().oSendPath();
								}
								if (Input.GetKeyUp (KeyCode.D) && delayOver) {
										chosenArrow = ChosenArrow.right;
								    	GameState = Status.WaitForPath;
										if(OpGameState == Status.WaitForPath) GameState=Status.MoveMe;
										GameObject.Find("NetworkInterface").GetComponent<NetworkInterface>().oSendPath();
									}
						}
		
				} else if (GameState == Status.MoveMe) {
						
						if (chosenArrow == ChosenArrow.forward) {
							addMazeObjects();
							MoveForward ();
							StartCoroutine(delay (3.0f));
							turn++;
							
						}
						
						if (chosenArrow == ChosenArrow.left) {
							addMazeObjects();
							MoveLeft ();
							StartCoroutine(delay (3.0f));
							turn++;
							
						}
						if (chosenArrow == ChosenArrow.right) {
							addMazeObjects();
							MoveRight();
							StartCoroutine(delay (3.0f));
							turn++;
							
						}
					chosenArrow = ChosenArrow.none;
				}

	}

	IEnumerator GenerateMap(){
		map = new Tile[mapX,mapZ];


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

			}
			yield return new WaitForSeconds(0.01f);
		}

		delayOver = true;
		if (OpGameState == Status.WaitCreation) {
			getDirectionData();
			haveInfo = true;
			GameState = Status.SuggestPath;
		}
		mapBuilt ();
	}
	void mapBuilt(){
		//my map is built i will send this to the oponent
		byte[] mapData = new byte[3];
		mapData [0] = (byte)playerPosition.x;
		mapData [1] = (byte)playerPosition.y;
		mapData [2] = (byte)face;
		GameState = Status.WaitCreation;
		if(OpGameState == Status.WaitCreation){
			GameState = Status.SuggestPath;
			//sp.setArrows(opArrows);
		}
		GameObject.Find ("NetworkInterface").GetComponent<NetworkInterface> ().oGameBuilt (mapData);
	}
	bool haveInfo;
	public void getMapData(byte[] mD){
		//gets the position and orientation of the oponent
		opPlayerPosition.x = mD [0];
		opPlayerPosition.y = mD [1];
		opFace = mD [2];
		OpGameState = Status.WaitCreation;

		if (GameState == Status.WaitCreation) {
				GameState = Status.SuggestPath;
				getDirectionData ();
				haveInfo = true;
				//sp.setArrows (opArrows);
			} else {
				
				
		}
	}


	void getDirectionData(){
		/// important creates the arrows for the opponent
		Vector2 frontPos, leftPos, rightPos;

		if (opFace == 0) {
			frontPos = opPlayerPosition + new Vector2(0,3);
			leftPos = opPlayerPosition + new Vector2(-1,2);
			rightPos = opPlayerPosition + new Vector2(1,2);

			frontPos = new Vector2((byte)frontPos.x%mapX,(byte)frontPos.y%mapZ);
			leftPos = new Vector2((byte)leftPos.x%mapX,(byte)leftPos.y%mapZ);
			rightPos = new Vector2((byte)rightPos.x%mapX,(byte)rightPos.y%mapZ);

		}else
		if (opFace == 1) {
			frontPos = opPlayerPosition + new Vector2(3,0);
			leftPos = opPlayerPosition + new Vector2(2,1);
			rightPos = opPlayerPosition + new Vector2(2,-1);
			
			frontPos = new Vector2((byte)frontPos.x%mapX,(byte)frontPos.y%mapZ);
			leftPos = new Vector2((byte)leftPos.x%mapX,(byte)leftPos.y%mapZ);
			rightPos = new Vector2((byte)rightPos.x%mapX,(byte)rightPos.y%mapZ);
			
		}else
		if (opFace == 2) {
			frontPos = opPlayerPosition + new Vector2(0,-3);
			leftPos = opPlayerPosition + new Vector2(1,-2);
			rightPos = opPlayerPosition + new Vector2(-1,-2);
			
			frontPos = new Vector2((byte)frontPos.x%mapX,(byte)frontPos.y%mapZ);
			leftPos = new Vector2((byte)leftPos.x%mapX,(byte)leftPos.y%mapZ);
			rightPos = new Vector2((byte)rightPos.x%mapX,(byte)rightPos.y%mapZ);
			
		}else
		if (opFace == 3) {
			frontPos = opPlayerPosition + new Vector2(-3,0);
			leftPos = opPlayerPosition + new Vector2(-2,-1);
			rightPos = opPlayerPosition + new Vector2(-2,1);
			
			frontPos = new Vector2((byte)frontPos.x%mapX,(byte)frontPos.y%mapZ);
			leftPos = new Vector2((byte)leftPos.x%mapX,(byte)leftPos.y%mapZ);
			rightPos = new Vector2((byte)rightPos.x%mapX,(byte)rightPos.y%mapZ);
			
		}
		byte emptyPlaces = 0;

		if (map [(byte)frontPos.x, (byte)frontPos.y].tile != Tile.TileType.destructable) emptyPlaces ++;
		if (map [(byte)leftPos.x, (byte)leftPos.y].tile != Tile.TileType.destructable) emptyPlaces ++;
		if (map [(byte)rightPos.x, (byte)rightPos.y].tile != Tile.TileType.destructable) emptyPlaces ++;

		opArrows = new byte[3]; //empty monster treasure wall
		if (emptyPlaces == 3) {
						float tV = Random.value;
						float mV = Random.value;
						float eV = Random.value;
						float minV = Mathf.Min (Mathf.Min (tV, mV), eV);
						float maxV = Mathf.Max (Mathf.Max (tV, mV), eV);
						if (tV == minV)
								opArrows [0] = 2;
						else if (mV == minV)
								opArrows [0] = 1;
						else if (eV == minV)
								opArrows [0] = 0;
			
						if (tV == maxV)
								opArrows [2] = 2;
						else if (mV == maxV)
								opArrows [2] = 1;
						else if (eV == maxV)
								opArrows [2] = 0;
			
						if (tV != maxV && tV != minV)
								opArrows [1] = 2;
						else if (mV != maxV && mV != minV)
								opArrows [1] = 1;
						else if (eV != maxV && eV != minV)
								opArrows [1] = 0;
				} else if (emptyPlaces == 2) {
						if (map [(byte)frontPos.x, (byte)frontPos.y].tile == Tile.TileType.destructable) {
								float tV = Random.value;
								float mV = Random.value;
					
								float minV = Mathf.Min (tV, mV);
								float maxV = Mathf.Max (tV, mV);
								if (tV == minV)
										opArrows [0] = 2;
								else if (mV == minV)
										opArrows [0] = 1;
					
					
								if (tV == maxV)
										opArrows [2] = 2;
								else if (mV == maxV)
										opArrows [2] = 1;
					
								opArrows [1] = 3;

						} else if (map [(byte)leftPos.x, (byte)leftPos.y].tile == Tile.TileType.destructable) {
								float tV = Random.value;
								float mV = Random.value;
					
								float minV = Mathf.Min (tV, mV);
								float maxV = Mathf.Max (tV, mV);
								if (tV == minV)
										opArrows [1] = 2;
								else if (mV == minV)
										opArrows [1] = 1;
					
					
								if (tV == maxV)
										opArrows [2] = 2;
								else if (mV == maxV)
										opArrows [2] = 1;
					
								opArrows [0] = 3;
						} else {
								float tV = Random.value;
								float mV = Random.value;
					
								float minV = Mathf.Min (tV, mV);
								float maxV = Mathf.Max (tV, mV);
								if (tV == minV)
										opArrows [0] = 2;
								else if (mV == minV)
										opArrows [0] = 1;
					
					
								if (tV == maxV)
										opArrows [1] = 2;
								else if (mV == maxV)
										opArrows [1] = 1;

								opArrows [2] = 3;
						}
				} else if (emptyPlaces == 1) {
						if (map [(byte)frontPos.x, (byte)frontPos.y].tile != Tile.TileType.destructable) {
								opArrows [0] = 3;
								opArrows [1] = 1;
								opArrows [2] = 3;
						} else if (map [(byte)leftPos.x, (byte)leftPos.y].tile != Tile.TileType.destructable) {
								opArrows [0] = 1;
								opArrows [1] = 3;
								opArrows [2] = 3;
						} else {
								opArrows [0] = 1;
								opArrows [1] = 3;
								opArrows [2] = 1;
						}
				} else {
					opArrows [0] = 3;
					opArrows [1] = 3;
					opArrows [2] = 3;
				}



	



	}
	void addMazeObjects(){
		if (face == 0) {
			Instantiate (mazeObjects[myArrows[0]],new Vector3(transform.position.x-1,height,transform.position.z+2),Quaternion.identity);
			Instantiate (mazeObjects[myArrows[1]],new Vector3(transform.position.x,height,transform.position.z+3),Quaternion.identity);
			Instantiate (mazeObjects[myArrows[2]],new Vector3(transform.position.x+1,height,transform.position.z+2),Quaternion.identity);
		}
		if (face == 1) {
			Instantiate (mazeObjects[myArrows[0]],new Vector3(transform.position.x+2,height,transform.position.z+1),Quaternion.identity);
			Instantiate (mazeObjects[myArrows[1]],new Vector3(transform.position.x+3,height,transform.position.z),Quaternion.identity);
			Instantiate (mazeObjects[myArrows[2]],new Vector3(transform.position.x+2,height,transform.position.z-1),Quaternion.identity);
		}
		if (face == 2) {
			Instantiate (mazeObjects[myArrows[0]],new Vector3(transform.position.x+1,height,transform.position.z-2),Quaternion.identity);
			Instantiate (mazeObjects[myArrows[1]],new Vector3(transform.position.x,height,transform.position.z-3),Quaternion.identity);
			Instantiate (mazeObjects[myArrows[2]],new Vector3(transform.position.x-1,height,transform.position.z-2),Quaternion.identity);
		}
		if (face == 3) {
			Instantiate (mazeObjects[myArrows[0]],new Vector3(transform.position.x-2,height,transform.position.z-1),Quaternion.identity);
			Instantiate (mazeObjects[myArrows[1]],new Vector3(transform.position.x-3,height,transform.position.z+0),Quaternion.identity);
			Instantiate (mazeObjects[myArrows[2]],new Vector3(transform.position.x-2,height,transform.position.z+1),Quaternion.identity);
		}
	}


	void MoveForward(){
		if (face == 0) {
				iTween.MoveTo (gameObject, new Vector3 (Mathf.Round (transform.position.x), Mathf.Round (transform.position.y), Mathf.Round (transform.position.z)) + new Vector3 (0, 0, -2), 2.0f);
				LastPosition = playerPosition;
				playerPosition -= new Vector2(0,-2);	
		}
		if (face == 1) {
			iTween.MoveTo (gameObject, new Vector3 (Mathf.Round (transform.position.x), Mathf.Round (transform.position.y), Mathf.Round (transform.position.z)) + new Vector3 (-2, 0, 0), 2.0f);
			LastPosition = playerPosition;
			playerPosition -= new Vector2(-2,0);			
		}
		if (face == 2) {
			iTween.MoveTo (gameObject, new Vector3 (Mathf.Round (transform.position.x), Mathf.Round (transform.position.y), Mathf.Round (transform.position.z)) + new Vector3 (0, 0, 2), 2.0f);
			LastPosition = playerPosition;
			playerPosition -= new Vector2(0,2);			
		}
        if (face == 3) {
			iTween.MoveTo (gameObject, new Vector3 (Mathf.Round (transform.position.x), Mathf.Round (transform.position.y), Mathf.Round (transform.position.z)) + new Vector3 (2, 0, 0), 2.0f);
			LastPosition = playerPosition;
			playerPosition -= new Vector2(2,0);	
			
		}
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
			LastPosition = playerPosition;
			playerPosition -= new Vector2(0,-2);
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
			LastPosition = playerPosition;
			playerPosition -= new Vector2(-2,0);
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
			LastPosition = playerPosition;
			playerPosition -= new Vector2(0,2);
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
			LastPosition = playerPosition;
			playerPosition -= new Vector2(2,0);
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
			LastPosition = playerPosition;
			playerPosition -= new Vector2(0,-2);
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
			LastPosition = playerPosition;
			playerPosition -= new Vector2(-2,0);
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
			LastPosition = playerPosition;
			playerPosition -= new Vector2(0,2);
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
			LastPosition = playerPosition;
			playerPosition -= new Vector2(2,0);
			ht.Add ("speed",3);
			iTween.MoveTo (gameObject,ht);
			iTween.RotateTo(camera,new Vector3(CameraAngle,0,0),3);
		}
	}

	IEnumerator delay(float time){
		delayOver = false;
		yield return new WaitForSeconds (time);
		buildWall ();
		delayOver = true;
		}
	private void buildWall(){
	//	playerPosition = new Vector2 ((int)playerPosition.x % mapX,(int) playerPosition.y % mapZ);
    //	LastPosition = new Vector2 ((int)LastPosition.x % mapX, (int)LastPosition.y % mapZ);
		wallPosition = new Vector3((int)((playerPosition.x+LastPosition.x)/2)%mapX,height,(int)((playerPosition.y+LastPosition.y)/2)%mapZ);

		Destroy (map [(int)wallPosition.x, (int)wallPosition.z].tileObj);
		GameObject go = Instantiate (DestructableWall [(int)(Random.value * DestructableWall.Length)], 
		                             wallPosition + this.transform.position

		                             , Quaternion.identity) as GameObject;
		go.transform.parent = destructableWallContainer.transform;

		map [(int)wallPosition.x, (int)wallPosition.z].tileObj = go;
		map [(int)wallPosition.x, (int)wallPosition.z].tile = Tile.TileType.destructable;
		if(Network.isClient || Network.isServer)GameObject.Find("NetworkInterface").GetComponent<NetworkInterface>().oNewWall ((int)(wallPosition.x*mapZ+wallPosition.z));
		mapBuilt();

	}
	public void buildWall(int pos){
		int x = pos / mapZ;
		int z = pos % mapZ;
		wallPosition = new Vector3(x,height,z);
		Destroy (map [(int)x, (int)z].tileObj);
		GameObject go = Instantiate (DestructableWall [(int)(Random.value * DestructableWall.Length)], 
		                             wallPosition + this.transform.position
		                             
		                             , Quaternion.identity) as GameObject;
		go.transform.parent = destructableWallContainer.transform;
		
		map [(int)wallPosition.x, (int)wallPosition.z].tileObj = go;
		map [(int)wallPosition.x, (int)wallPosition.z].tile = Tile.TileType.destructable;

	}

	public void died(GameObject monster){

	}
	public void takeTreasure(GameObject treasure){
		gold += combo * gold;
		combo++;
		lastCombo = turn;
	}
	public void destroyWall(GameObject wall){
		gold -= (gold * 10 / 100);
		combo = 1;
	}


}




