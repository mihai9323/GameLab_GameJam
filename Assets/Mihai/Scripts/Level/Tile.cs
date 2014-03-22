using UnityEngine;
using System.Collections;

public class Tile  {

	public enum TileType
		{
			wall, destructable, path
		}
	public TileType tile;

	public Tile(TileType tile){
		this.tile = tile;
	}

}
