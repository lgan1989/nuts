using UnityEngine;
using System.Collections;
using System.IO;
using Tiled2Unity;

public class TileGrid{
	public int x;
	public int y;
	public TileGrid(int _x , int _y){
		x = _x;
		y = _y;
	}
	public string GetString()
	{
		return x.ToString() + "," + y.ToString();
	}
}

public class TileInfo : MonoBehaviour {

	public string mapID;
	public CollisionType[,] collisionInfo;
	public int[,] tileInfo;
	public int[,] blockInfo;
	public float xOffset = 0;
	public float yOffset = 0;
	public float tileWidth = 0;
	public float tileHeight = 0;
	public int numTileWidth;
	public int numTileHeight;


	public enum TileType{
		Ground = 0,
		JungleLow = 1,
		JungleHight = 2
	}

	public enum CollisionType{
		Empty = 0,
		Occupied = 1
	}

	public enum BlockType{
		Up = 1,
		Down = 2,
		Left = 4,
		Right = 8
	}

	void Awake(){
		LoadMapData();
	}

	// Use this for initialization
	void Start () {

	}

	void LoadMapData(){

		TiledMap map = this.GetComponentInParent<TiledMap>();

		int w = map.NumTilesWide;
		int h = map.NumTilesHigh;

		numTileWidth = w;
		numTileHeight = h;

		tileWidth = map.TileWidth * map.ExportScale;
		tileHeight = map.TileHeight * map.ExportScale;

		xOffset = map.transform.position.x;
		yOffset = map.transform.position.y;
		
		collisionInfo = new CollisionType[w,h];
		tileInfo = new int[w,h];
		blockInfo = new int[w,h];
	
		
		var lines = File.ReadAllLines(Application.dataPath + "/Data/" + mapID + ".map");
		
		for (int i = 0 ; i < lines.Length ; i ++){
			for (int j = 0 ; j < lines[i].Length ; j += 2){
				string tmp = lines[i].Substring(j,2);
				int type = int.Parse(tmp);
				if (i < lines.Length/2){
					collisionInfo[j/2,i] = CollisionType.Empty;
					tileInfo[j/2,i] = type;
				}
				else{
					blockInfo[j/2,i - lines.Length/2] = type;
				}
			}
		}
		int x = 1;
	}

}
