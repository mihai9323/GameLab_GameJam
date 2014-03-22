using UnityEngine;
using System.Collections;

public class LobbyScript : MonoBehaviour {
	HostData[] Hd;
	NetworkInterface NI;
	void Awake () 
	{
		NI = GameObject.Find ("NetworkInterface").GetComponent<NetworkInterface> ();
	}

	void CreateServer()
	{
		NI.CreateServer();
	}

	HostData[] GetServerList()
	{
		return NI.GetServerList();

	}
	
	void Update () 
	{
		
	}

	void OnGUI()
	{
		if (Hd != null) {
					for (int i = 0; i < Hd.Length; i++) {
						Debug.Log ((Hd [i].gameName + " " + Hd [i].connectedPlayers + "/" + Hd [i].playerLimit + " players").ToString ());
			
						if (GUI.Button (new Rect (255, 20 + 53 * i, 400, 50), (Hd [i].gameName + " " + Hd [i].connectedPlayers + "/" + Hd [i].playerLimit + " players").ToString ())) {
							NI.Connect(Hd[i]);
						}
					}
					Debug.Log ("hostdata length: " + Hd.Length);
				}

		if(Network.isServer || Network.isClient)
		{
			if(GUI.Button( new Rect(50, 20, 200, 50), "Get Server List"))
			{
				Hd =GetServerList();



			}
		}
		else 
		{

			if(GUI.Button( new Rect(50, 20, 200, 50), "Create Server"))
			{
				CreateServer();
			}
		}
	}
}
