//
// UITexture.cs
//
// Author:
//       Will Matterer <will.matterer@gmail.com>
//
using UnityEngine;
using System.Collections;

public class UITexture : UIElement {

	[Tooltip("Image to use instead of text.")]
	public Texture2D
		Icon;
	
	void OnGUI( )
	{
		uiConfig();

		if ( useCustomStyle ) {
			if ( Icon ) {
				GUI.Label( elementRect, 
				           Icon,
				           elementStyle );
			} 
		} else {
			if ( Icon ) {
				GUI.Label( elementRect, Icon );
			} 
		}
		
	}

	void OnDrawGizmos( )
	{
		Gizmos.DrawGUITexture( elementRect, Icon );
	}
}
