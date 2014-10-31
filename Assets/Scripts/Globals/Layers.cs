using UnityEngine;
using System.Collections;

public static class Layers {

	//Use bitwise funtions to ignore or include layers.
	//Ex: 
	//~Player & ~Ground will ignore both the player and the floor

	public const int Default       = 1 << 0;
	public const int TransparentFX = 1 << 1;
	public const int IgnoreRaycast = 1 << 2;

	public const int Water         = 1 << 4;
	public const int UI            = 1 << 5;

	
	public const int Vehicles      = 1 << 8;
	public const int Characters    = 1 << 9;
	public const int Environment   = 1 << 10;
	public const int Player        = 1 << 11;
}
