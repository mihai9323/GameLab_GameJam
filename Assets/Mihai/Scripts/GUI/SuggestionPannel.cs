using UnityEngine;
using System.Collections;

public class SuggestionPannel : MonoBehaviour {

	public GameObject bg;
	public GameObject leftArrow,frontArrow,rightArrow;
	public Material monster, treasure, empty;

	private byte[] arrows; //0 empty 1 monster 2 treasure 3 wall
	// Use this for initialization
	public void setArrows(byte[] arr){
		arrows = arr;
//	

		switch (arr [0]) {
			case 0: leftArrow.renderer.material = empty; break;
		case 1: leftArrow.renderer.material = monster; break;
		case 2: leftArrow.renderer.material = treasure; break;
		case 3: leftArrow.renderer.material = empty; break;
			//default: leftArrow.texture = empty;
		
		}
		switch (arr [1]) {
		case 0: frontArrow.renderer.material = empty; break;
		case 1: frontArrow.renderer.material = monster; break;
		case 2: frontArrow.renderer.material = treasure; break;
		case 3: frontArrow.renderer.material = empty; break;
		//default: frontArrow.texture = empty;
			
		}
		switch (arr [2]) {
		case 0: rightArrow.renderer.material = empty; break;
		case 1: rightArrow.renderer.material = monster; break;
		case 2: rightArrow.renderer.material = treasure; break;
		case 3: rightArrow.renderer.material = empty; break;
//		default: rightArrow.texture = empty;

		}

	}
	void Update(){
		if (this.renderer.enabled) {
			bg.renderer.enabled = leftArrow.renderer.enabled = frontArrow.renderer.enabled = rightArrow.renderer.enabled = true;
		} else {
			bg.renderer.enabled = leftArrow.renderer.enabled = frontArrow.renderer.enabled = rightArrow.renderer.enabled = false;
		}
	}


}
