using UnityEngine;
using System.Collections;

public class SuggestionPannel : MonoBehaviour {

	public GUITexture leftArrow,frontArrow,rightArrow;
	public Texture monster, treasure, empty;

	private byte[] arrows; //0 empty 1 monster 2 treasure 3 wall
	// Use this for initialization
	public void setArrows(byte[] arr){
		arrows = arr;
//	

		switch (arr [0]) {
			case 0: leftArrow.texture = empty; break;
			case 1: leftArrow.texture = monster; break;
			case 2: leftArrow.texture = treasure; break;
			case 3: leftArrow.texture = empty; break;
			//default: leftArrow.texture = empty;
		
		}
		switch (arr [1]) {
		case 0: frontArrow.texture = empty; break;
		case 1: frontArrow.texture = monster; break;
		case 2: frontArrow.texture = treasure; break;
		case 3: frontArrow.texture = empty; break;
		//default: frontArrow.texture = empty;
			
		}
		switch (arr [2]) {
		case 0: rightArrow.texture = empty; break;
		case 1: rightArrow.texture = monster; break;
		case 2: rightArrow.texture = treasure; break;
		case 3: rightArrow.texture = empty; break;
//		default: rightArrow.texture = empty;

		}

	}


}
