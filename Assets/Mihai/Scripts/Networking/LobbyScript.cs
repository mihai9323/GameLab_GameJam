using UnityEngine;
using System.Collections;

public class LobbyScript : MonoBehaviour {
	HostData[] Hd;
	NetworkInterface NI;
	void Awake () 
	{
		NI = GameObject.Find ("NetworkInterface").GetComponent<NetworkInterface> ();
		StartCoroutine ("RefreshList");
	}

	void CreateServer()
	{
		NI.CreateServer();
	}
	void DisconectFromServer(){
		Network.Disconnect ();
		}
	HostData[] GetServerList()
	{
		return NI.GetServerList();

	}
	IEnumerator RefreshList()
	{
		while (true) {
			Hd=GetServerList();
			if(Hd!=null){
				foreach(HostData hd in Hd){
					if(hd.connectedPlayers == 2){
						NI.oStartGame("MihaiGameScene");
						//Application.LoadLevel("CristiScene");
					}
				}
			}
			yield return new WaitForSeconds(1.0f);
			}
	}
	void Update () 
	{
		
	}

	void OnGUI()
	{
		if (Hd != null) {
					for (int i = 0; i < Hd.Length; i++) {
						
			
						if (GUI.Button (new Rect (255, 20 + 53 * i, 400, 50), (Hd [i].gameName + " " + Hd[i].connectedPlayers+ "/" + Hd [i].playerLimit + " players").ToString ())) {
							NI.Connect(Hd[i].guid);
						}
					}
					
				}


		if(GUI.Button( new Rect(50, 20, 200, 50), "Refresh List"))
			{
				Hd =GetServerList();



			}


		if (!Network.isServer && !Network.isClient) {
					if (GUI.Button (new Rect (50, 73, 200, 50), "Create Game")) {
							CreateServer ();
					}
		} else {
		if (GUI.Button (new Rect (50, 73, 200, 50), "Exit Game")) {
				DisconectFromServer ();
			}
		}
	}
}
