using UnityEngine;
using System.Collections;

public class MaskSet : MonoBehaviour {

	// Use this for initialization

	public Texture2D  maskJungleLow48;
	public Texture2D maskJungleLow64;
	public Texture2D maskJungleHigh48;
	public Texture2D maskJungleHigh64;

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public Texture2D GetMask(TileInfo.TileType tileType , bool attack) {
		if (attack){
			switch (tileType){
			case TileInfo.TileType.JungleLow:
				return maskJungleLow48;
			case TileInfo.TileType.JungleHigh:
				return maskJungleHigh48;
			default:
				return null;
			}
		}
		else{
			switch (tileType){
			case TileInfo.TileType.JungleLow:
				return maskJungleLow64;
			case TileInfo.TileType.JungleHigh:
				return maskJungleHigh64;
			default:
				return null;
			}
		}
	}

}
