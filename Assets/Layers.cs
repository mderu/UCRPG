using UnityEngine;
using System.Collections;

public static class Layers {

	public static int Default       = 1 << 0;
	public static int TransparentFX = 1 << 1;
	public static int IgnoreRaycast = 1 << 2;

	public static int Water         = 1 << 4;
	public static int UI            = 1 << 5;

	
	public static int Vehicles      = 1 << 8;
	public static int Characters    = 1 << 9;
	public static int Floor         = 1 << 10;
	public static int Player        = 1 << 11;

	public static int IgnorePlayer(){
		return ~Player;
	}
}
