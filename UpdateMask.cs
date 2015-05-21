using UnityEngine;
using System.Collections;

public class UpdateMask : MonoBehaviour {

	public Texture2D  mask48;
	public Texture2D mask64;
	public GameObject pawn;
	// Use this for initialization
	void Start () {
		mask48 = (Texture2D )Resources.Load ("mask48");
		mask64 = (Texture2D )Resources.Load ("mask64");
	}
	
	// Update is called once per frame
	void Update () {

	
	}
}
