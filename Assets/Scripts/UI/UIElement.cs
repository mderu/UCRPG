//
// UIElement.cs
//
// Author:
//       Will Matterer <will.matterer@gmail.com>
//
// This is a base class for UI elements, not to be used directly as a component.

using UnityEngine;
using System.Collections;

public class UIElement : MonoBehaviour 
{	
	public enum ScreenLocation {
		UPPER_LEFT,
		UPPER_CENTER,
		UPPER_RIGHT,
		MIDDLE_LEFT,
		MIDDLE_CENTER,
		MIDDLE_RIGHT,
		LOWER_LEFT,
		LOWER_CENTER,
		LOWER_RIGHT
	}

	[Tooltip("GUISkin to be used by this button")]
	public GUISkin 
		gameSkin;

	[Tooltip("Pixel width of the element on screen")]
	public float 
		width = 170.0f;

	[Tooltip("Pixel height of the element on screen")]
	public float 
		height = 70.0f;

	[Tooltip("GUI depth")]
	public int 
		depth = 1;

	[Tooltip("Location on the screen")]
	public ScreenLocation 
		screenLocation;
	
	[Tooltip("An offset from current location on the screen")]
	public Vector2 
		offset;

	[Tooltip("If true, use a custom style in the GUISkin")]
	public bool 
		useCustomStyle = false;

	[Tooltip("The name of the custom style to use")]
	public string 
		style;
	
	protected Rect elementRect;
	protected GUIStyle elementStyle;
	protected Vector2 location;
	
	protected void uiConfig() 
	{
		GUI.skin = gameSkin;
		GUI.depth = depth;
		if( useCustomStyle ) {
			elementStyle = GUI.skin.GetStyle(style);
		}
		switch( screenLocation ) {
			case ScreenLocation.UPPER_LEFT:
				location = new Vector2( offset.x, 
				                        offset.y );
				break;
			case ScreenLocation.UPPER_CENTER:
				location = new Vector2( (Screen.width / 2) - (width/2) + offset.x, 
				                        offset.y );
				break;
			case ScreenLocation.UPPER_RIGHT:
				location = new Vector2( (Screen.width - width) + offset.x, 
				                        offset.y );
				break;
			case ScreenLocation.MIDDLE_LEFT:
				location = new Vector2( offset.x, 
				                        (Screen.height / 2) - (height/2) + offset.y );
				break;
			case ScreenLocation.MIDDLE_CENTER:
				location = new Vector2( (Screen.width / 2)  - (width/2)  + offset.x, 
				                        (Screen.height / 2) - (height/2) + offset.y );
				break;
			case ScreenLocation.MIDDLE_RIGHT:
				location = new Vector2( (Screen.width - width) + offset.x, 
				                        (Screen.height / 2) - (height/2) + offset.y );
				break;
			case ScreenLocation.LOWER_LEFT:
				location = new Vector2( offset.x, 
				                        (Screen.height - height) + offset.y );
				break;
			case ScreenLocation.LOWER_CENTER:
				location = new Vector2( (Screen.width / 2) - (width/2) + offset.x, 
				                       (Screen.height - height) + offset.y );
				break;
			case ScreenLocation.LOWER_RIGHT:
				location = new Vector2((Screen.width - width) + offset.x, 
				                       (Screen.height - height) + offset.y );
				break;
		}

		elementRect = new Rect( location.x, location.y, width, height );
	}
}