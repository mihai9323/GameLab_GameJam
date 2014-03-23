using UnityEngine;
using System.Collections;

public class AlertController : MonoBehaviour {

	private bool notification = false;
	public Transform howToPlay, howToMyPath,wallAlert,waitingForPlayer,exchangeGold,results,pathForOpp;

	public void displayHowToPlay(){
		howToMyPath.renderer.enabled = true;
		howToPlay.renderer.enabled = false;
		wallAlert.renderer.enabled = false;
		waitingForPlayer.renderer.enabled = false;
		exchangeGold.renderer.enabled = false;
		results.renderer.enabled = false;

		notification = true;
	}
	public void displayPathForOp(){
		pathForOpp.renderer.enabled = true;
		}
	public void displayHowToPath(){
		howToMyPath.renderer.enabled = false;
		howToPlay.renderer.enabled = true;
		wallAlert.renderer.enabled = false;
		waitingForPlayer.renderer.enabled = false;
		exchangeGold.renderer.enabled = false;
		results.renderer.enabled = false;
		
		notification = true;
	}
	public void displayWall(){
		howToMyPath.renderer.enabled = false;
		howToPlay.renderer.enabled = false;
		wallAlert.renderer.enabled = true;
		waitingForPlayer.renderer.enabled = false;
		exchangeGold.renderer.enabled = false;
		results.renderer.enabled = false;
		
		notification = true;
	}
	public void displayWait(){
		howToMyPath.renderer.enabled = false;
		howToPlay.renderer.enabled = false;
		wallAlert.renderer.enabled = false;
		waitingForPlayer.renderer.enabled = true;
		exchangeGold.renderer.enabled = false;
		results.renderer.enabled = false;
		
		notification = true;
	}
	public void displayExchangeGold(){
		howToMyPath.renderer.enabled = true;
		howToPlay.renderer.enabled = false;
		wallAlert.renderer.enabled = false;
		waitingForPlayer.renderer.enabled = false;
		exchangeGold.renderer.enabled = true;
		results.renderer.enabled = false;
		
		notification = true;
	}
	public void displayResults(){
		howToMyPath.renderer.enabled = true;
		howToPlay.renderer.enabled = false;
		wallAlert.renderer.enabled = false;
		waitingForPlayer.renderer.enabled = false;
		exchangeGold.renderer.enabled = true;
		results.renderer.enabled = false;
		
		notification = true;
	}

	void Update(){
		if (Input.GetKey (KeyCode.S)) {
			if(howToPlay.renderer.enabled) howToPlay.renderer.enabled = false;
			if(howToMyPath.renderer.enabled) howToMyPath.renderer.enabled = false;

			if(results.renderer.enabled){
				results.renderer.enabled = false;
				Application.LoadLevel (0);
			}
		}
	}


}
