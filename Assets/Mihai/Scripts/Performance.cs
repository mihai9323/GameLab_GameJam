using UnityEngine;
using System.Collections;

public class Performance : MonoBehaviour {

	public delegate void ENTER_FRAME();
	public static event ENTER_FRAME EnterFrame;
	// Use this for initialization
	void Awake () {
		StartCoroutine (Enter_Frame());
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public static IEnumerator Enter_Frame(){
		while (true) {
			if(EnterFrame!=null){

				EnterFrame();
			}
			yield return new WaitForSeconds(0.003f);
		}
	}
}
