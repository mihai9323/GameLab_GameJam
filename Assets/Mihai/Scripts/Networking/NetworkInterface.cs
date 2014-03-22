using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class NetworkInterface : MonoBehaviour {
	static GameObject _instance;
	bool hasWon = false;

	string gameName = "GLGameJam14.TrickTheMage";
	string gameTypeName = "GLGameJam14.TrickTheMage";
	bool isRefreshingServerList = false;
	HostData[] hostdata;

	void Awake()
	{
		//Util.Settings.DrawLog = false;
		if(_instance != null)
		{
			_instance.GetComponent<NetworkInterface>().CleanUp ();
			GameObject.Destroy(_instance);
		}
		_instance = gameObject;
	}

	void Start()
	{
		gameName += SystemInfo.deviceName.ToString();
		InvokeRepeating("RefreshHelper", 0f, 1f);
		DontDestroyOnLoad(this.gameObject);
	}

	void RefreshHelper()
	{
		StartCoroutine("RefreshServerList");
	}

	public void CreateServer()
	{
		Network.InitializeServer(1, 25002, true);
		MasterServer.RegisterHost(gameTypeName, gameName);
	}

	public void CreateServerWithName(string namerzorz)
	{
		Network.InitializeServer(1, 25002, true);
		MasterServer.RegisterHost(gameTypeName, namerzorz);
	}

	public void Connect(string guid)
	{
		Network.Connect(guid);

	}
	public void Connect(HostData hd)
	{
		Network.Connect (hd);
		
	}

	public HostData[] GetServerList()
	{
		return hostdata;
	}

	private IEnumerator RefreshServerList()
	{
//		if(Network.isServer || Network.isClient)
//			yield return null;
		MasterServer.RequestHostList(gameTypeName);
		yield return new WaitForSeconds(2f);
		hostdata = MasterServer.PollHostList();
	}

	void OnServerInitialized()
	{
		Debug.Log("Server has been initialized");
	}

	void OnApplicationQuit()
	{
		Network.Disconnect(0);
		MasterServer.UnregisterHost();
	}

	void OnConnectedToServer()
	{
		Debug.Log("Connected to server");
	}

	void OnDisconnectedFromServer()
	{
		Debug.Log("Disconnected from server");
	}

	void OnFailedToConnect()
	{
		Debug.Log("Failed to connect");
	}

	void OnPlayerConnected()
	{
		Debug.Log("Player connected");
		//oStartGame("TubeOne");
	}

	void OnPlayerDisconnected()
	{
		Debug.Log("Player disconnected");
	}
	
	void OnMasterServerEvent(MasterServerEvent mse)
	{
		if(mse == MasterServerEvent.RegistrationSucceeded)
			Debug.Log("OnMasterServerEvent: Registration of server successful");
		else if(mse == MasterServerEvent.RegistrationFailedGameName)
			Debug.Log("OnMasterServerEvent: Registration of server failed: game name");
		else if(mse == MasterServerEvent.RegistrationFailedNoServer)
			Debug.Log("OnMasterServerEvent: Registration of server failed: no server");
		else if(mse == MasterServerEvent.RegistrationFailedGameType)
			Debug.Log("OnMasterServerEvent: Registration of server failed: game type");
		else if(mse == MasterServerEvent.HostListReceived)
			Debug.Log("OnMasterServerEvent: Host list received");
	}

//	void OnGUI()
//	{
//		if(Application.loadedLevelName == "Lobby")
//		{
//			if(GUI.Button(new Rect(0,Screen.height-30, 100, 30), "Ping"))
//			{
//				networkView.RPC("Ping", RPCMode.Others, null);
//			}
//
//			if(GUI.Button(new Rect(25, 65, 150, 30), "Refresh Server List"))
//			{
//				StartCoroutine("RefreshServerList");
//			}
//
//			if(hostdata != null)
//			{
//				for(int i = 0; i < hostdata.Length; i++)
//				{
//					if(GUI.Button(new Rect(200, 25 + 40 * i, 300, 30), hostdata[i].gameName.ToString()))
//						ConnectToServer(hostdata[i]);
//				}
//			}
//
//			if(Network.isServer)
//				return;
//			
//			if(GUI.Button(new Rect(25, 25, 150, 30), "Start New Server"))
//			{
//				CreateServer();
//			}
//		}
//	}

	void ConnectToServer(HostData hd)
	{
		Debug.Log("Connecting to server...");
		Network.Connect(hd.guid);
	}

	#region rpc's
//	[RPC]
//	void Ping()
//	{
//		GameObject.Instantiate(networkCube, Vector3.zero, Quaternion.identity);
//	}






	


	[RPC]	
	void iStartGame(string levelName)
	{
        //LoadingScene.nextLevel = levelName;
		Debug.Log (levelName);
		Application.LoadLevel(levelName);
		//Time.timeScale = 0f;
	}

	void OnLevelWasLoaded(int level)
	{
		if(level == 2)
			oIsReady();
		else if(level == 0)
			CleanUp();
	}

	void CleanUp()
	{
		Network.Disconnect();
		MasterServer.UnregisterHost();
	}
	
	public void oStartGame(string levelName)
	{
		networkView.RPC("iStartGame", RPCMode.All, new object[]{levelName});
	}

	int isReady = 0;
	[RPC]
    void iIsReady()
	{
		isReady++;
	}
	
	public void oIsReady()
	{
		networkView.RPC("iIsReady", RPCMode.All, null);
	}


	[RPC]
	void iNewWall(int location){
		GameObject.Find ("Map").GetComponent<GenerateLevel>().buildWall (location);
	}

	public void oNewWall(int location)
	{
		networkView.RPC("iNewWall", RPCMode.Others,new object[]{location});
	}





	#endregion
}
