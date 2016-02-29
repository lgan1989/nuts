using UnityEngine;
using System;
using System.Collections;



public class Tuple<T1, T2>
{
	public T1 First { get; private set; }
	public T2 Second { get; private set; }
	internal Tuple(T1 first, T2 second)
	{
		First = first;
		Second = second;
	}	

}

public class Status{
	public Hashtable next; 
}


public enum EventTargetType{
	None,
	Any,
	Tile,
	MoveTile,
	PlayerPawn,
	FriendPawn,
	EnemyPawn,
	MenuItemAttack,
	MenuItemStandBy
}

public enum EventType{
	LeftClick,
	RightClick,
	MoveFinished,
	AttackFinished
}

public class GameEvent
{
	public EventTargetType target;
	public GameObject obj;
	public EventType type;

	public GameEvent(EventTargetType _o , EventType _t){
		target = _o;
		type = _t;
	}
	public GameEvent(EventTargetType _o ,GameObject _obj, EventType _t){
		target = _o;
		type = _t;
		obj = _obj;
	}
	public override int GetHashCode ()
	{
		return (int)(target + 1) * 10+ (int)type;
	}

}