using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : ScriptableObject
{
	public string pointName;
	public Coord coord;
	public Color thisColor = Color.white;
	public PointConnection[] connections;
}

[System.Serializable]
public class Coord
{
	public int x;
	public int y;
}

[System.Serializable]
public class PointConnection
{
	public string name;
	public float distance;
}

