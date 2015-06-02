using UnityEngine;
using System.Collections;

public class Hero : MonoBehaviour {

	public enum HeroType{
		Strength = 0,
		Agility = 1,
		Intelligence = 2
	}

	public int cid;
	public string heroName;
	public HeroType type;
	public int[] attributes = new int[3];
	public int level;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
