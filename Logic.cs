﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Logic : MonoBehaviour {

	private const int INT_MAX = (1<<30);

	private TileInfo tileInfo;
	public Pawn pawn;

	public static int[,] DIR = new int[4 , 2]{{0,1} , {0 , -1} , {-1, 0} , {1,0}}; //Down, Up, Left, Right

	public static ArrayList pawnList;
	public static Pawn selectedPawn;

	public static void SelectPawn(Pawn pawn){
		Logic.selectedPawn = pawn;
	}
	
	public static void DeselectPawn(Pawn pawn){
		Logic.selectedPawn = null;
	}



	void Awake(){
		tileInfo = GameObject.Find ("Map").GetComponent<TileInfo>();
		pawnList = new ArrayList();
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	}

	public Vector3 GetPositionByGrid( int x , int y , int z){
		Vector3 position = new Vector3();
		position.x = x * tileInfo.tileWidth + tileInfo.xOffset  + 0.5f * tileInfo.tileWidth;
		position.y = tileInfo.yOffset - y * tileInfo.tileHeight - 0.5f * tileInfo.tileHeight;
		position.z = z;
		return position;
	}

	public TileGrid GetGridByPosition( float x , float y){
		TileGrid ret = new TileGrid(
			(int) ( (x - tileInfo.xOffset)/ tileInfo.tileWidth + 0.0000001),
			(int) ( (tileInfo.yOffset - y )/ tileInfo.tileHeight + 0.0000001 )
			);

		return ret;
	}

	public int[,] CalculateMobilityReduce()
	{
		int w = tileInfo.numTileWidth;
		int h = tileInfo.numTileHeight;

		int[,] result = new int[w,h];
		for (int i = 0 ; i < w ; i ++)
			for (int j = 0 ; j < h ; j ++)
				result[i,j] = 1;
		for (int i = 0 ; i < Logic.pawnList.Count ; i ++)
		{
			Pawn p = (Pawn)pawnList[i];
			if (pawn.team != p.team)
			{
				TileGrid pos = p.gridPosition;
				for (int d = 0 ; d < 4 ; d ++)
				{
					int nx = pos.x + DIR[d,0];
					int ny = pos.y+ DIR[d,1];
					if (nx >= 0 && nx < w && ny >= 0 && ny < h &&
					    tileInfo.collisionInfo[nx,ny] == TileInfo.CollisionType.Empty
					    )
						result[nx,ny] = INT_MAX;
				}
			}
		}
		return result;
	}

	public ArrayList GetValidMove(){

		ArrayList result = new ArrayList();

		var mobility_required = CalculateMobilityReduce();
		mobility_required[pawn.gridPosition.x , pawn.gridPosition.y] = 1;

		Queue q = new Queue();
		Hashtable visited = new Hashtable();
		Tuple<int , TileGrid> ele = new Tuple<int,TileGrid>(0 , pawn.gridPosition);

		visited.Add( ele.Second.GetString() ,  true);

		q.Enqueue( ele );

		int mobility = 6;

		while (q.Count > 0)
		{
			Tuple<int,TileGrid> top = (Tuple<int,TileGrid>)q.Dequeue();
			int step = top.First;
			TileGrid pos = top.Second;
			if (step > mobility)
				continue;
			result.Add( pos );

			if ( mobility_required[ pos.x , pos.y ] == INT_MAX)
				continue;

			for (int d = 0 ; d < 4 ; d ++){
				int nx = pos.x + DIR[d,0];
				int ny = pos.y + DIR[d,1];

				TileGrid np = new TileGrid(nx , ny);
				if (nx >= 0 && nx < tileInfo.numTileWidth && ny >= 0 && ny < tileInfo.numTileHeight &&
				    visited.ContainsKey( np.GetString() ) == false &&
				    tileInfo.collisionInfo[nx,ny] == TileInfo.CollisionType.Empty &&
				    (tileInfo.blockInfo[ pos.x , pos.y ] & (1<<d)) == 0
				    )
				{
					visited.Add(np.GetString() , true);
					q.Enqueue( new Tuple<int, TileGrid>( step + mobility_required[ pos.x , pos.y ] , np) );
				}
			}
		}

		return result;

	}

	public void GridSetEmpty(TileGrid grid){
		tileInfo.collisionInfo[ grid.x , grid.y ] = TileInfo.CollisionType.Empty;
	}
	public void GridSetOccupied(TileGrid grid){
		tileInfo.collisionInfo[ grid.x , grid.y ] = TileInfo.CollisionType.Occupied;
	}

	public static CustomAnimation.Direction GetDirection( TileGrid start , TileGrid target ){
		if (start.x == target.x && start.y <  target.y)
			return CustomAnimation.Direction.Down;
		if (start.x == target.x && start.y > target.y)
			return CustomAnimation.Direction.Up;
		if (start.y == target.y && start.x < target.x)
			return CustomAnimation.Direction.Right;
		if (start.y == target.y && start.x > target.x)
			return CustomAnimation.Direction.Left;
		return CustomAnimation.Direction.Down;
	}
	
	public ArrayList FindPath(TileGrid start, TileGrid destination, int stepLimit){

		ArrayList path = new ArrayList();

		if (start.GetString() == destination.GetString()){
			return path;
		}
		
		var mobility_required = CalculateMobilityReduce();
		mobility_required[start.x , start.y] = 1;
		
		Queue q = new Queue();
		Hashtable visited = new Hashtable();
		Hashtable prev = new Hashtable();
		Tuple<int , TileGrid> ele = new Tuple<int,TileGrid>(0 , pawn.gridPosition);
		
		visited.Add( ele.Second.GetString() ,  true);
		
		q.Enqueue( ele );
		bool find = false;
		while (q.Count > 0)
		{
			Tuple<int,TileGrid> top = (Tuple<int,TileGrid>)q.Dequeue();
			int step = top.First;
			TileGrid pos = top.Second;
			if (step > stepLimit)
				break;
			if (pos.GetString() == destination.GetString()){
				find = true;
				break;
			}
			if ( mobility_required[ pos.x , pos.y ] == INT_MAX)
				continue;
			for (int d = 0 ; d < 4 ; d ++){
				int nx = pos.x + DIR[d,0];
				int ny = pos.y + DIR[d,1];
				TileGrid np = new TileGrid(nx , ny);
				if (nx >= 0 && nx < tileInfo.numTileWidth && ny >= 0 && ny < tileInfo.numTileHeight &&
				    visited.ContainsKey( np.GetString() ) == false &&
				    tileInfo.collisionInfo[nx,ny] == TileInfo.CollisionType.Empty &&
				    (tileInfo.blockInfo[ pos.x , pos.y ] & (1<<d)) == 0
				    )
				{
					prev.Add( np.GetString() , pos );
					visited.Add(np.GetString() , true);
					q.Enqueue( new Tuple<int, TileGrid>( step + mobility_required[ pos.x , pos.y ] , np) );
				}
			}
		}

		if (find){
			TileGrid cur = destination;
			while ( prev.ContainsKey( cur.GetString() ) ){
				path.Add( cur );
				cur = (TileGrid)prev[ cur.GetString() ];
			}
			path.Reverse();
			return path;
		}

		return path;
		
	}
}