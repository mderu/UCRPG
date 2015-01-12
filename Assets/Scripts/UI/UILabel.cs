//
// UILabel.cs
//
// Author:
//       Will Matterer <will.matterer@gmail.com>
//
using UnityEngine;
using System.Collections;

public class UILabel : UIElement
{
	[Tooltip("Label's text.")]
	public string 
		Text;

	void OnGUI( )
	{
		uiConfig();

		if ( useCustomStyle ) {
			GUI.Label( elementRect, 
				           Text,
				           elementStyle );
		} else {
			GUI.Label( elementRect, Text );
		}
		
	}
}