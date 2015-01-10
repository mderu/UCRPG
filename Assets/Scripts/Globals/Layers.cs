using UnityEngine;
using System.Collections;

public static class Layers {

	//Use Mask to properly apply a LayerMask
    //When updating the value of a layer here, make sure
    //You update it in the Unity Scene file.

	public const int Default       = 0;
	public const int TransparentFX = 1;
	public const int IgnoreRaycast = 2;

	public const int Water         = 4;
	public const int UI            = 5;
	

	public const int Vehicles      = 8;
	public const int Characters    = 9;
	public const int Environment   = 10;
	public const int Player        = 11;
	public const int Enemies       = 12;
	public const int PlayerBounds  = 13;
    public const int Allies        = 14;

    public const int TargetableMask = (1 << Enemies) | (1 << Allies);

    //Input a number of layers separated fby commas,
    //Outputs the layermask.
    //You can call ~Layers.Mask() to get the exclude
    //The layers in the mask.
    public static int Mask(params int[] layers)
    {
        int retMe = 0x000000;
        for (int i = 0; i < layers.Length; i++)
        {
            retMe |= (1 << layers[i]);
        }
        return retMe;
    }
}
