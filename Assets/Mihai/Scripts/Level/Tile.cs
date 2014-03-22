using UnityEngine;
using System.Collections;

public class Tile  {

	public enum TileType
		{
			wall, destructable, path
		}
	public TileType tile;
	public GameObject tileObj;

	public Tile(TileType tile,GameObject go){
		this.tile = tile;
		this.tileObj = go;
	}

}
