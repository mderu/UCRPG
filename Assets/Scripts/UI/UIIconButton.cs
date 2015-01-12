//
// UIIconButton.cs
//
// Author:
//       Will Matterer <will.matterer@gmail.com>
//

using UnityEngine;
using System.Collections;

public class UIIconButton : UIElement
{
	[Tooltip("Link to script to be called on button press")]
	public MonoBehaviour 
		script;

	[Tooltip("Name of function to call on button press")]
	public string 
		functionToInvoke;

	[Tooltip("Image to use instead of text.")]
	public Texture2D 
		Icon;

	void OnGUI( ) 
	{
		uiConfig();  // required to be called first
		
		if( useCustomStyle ) {
			if( Icon ) {
				if( GUI.Button (elementRect, Icon, elementStyle) )
				{
					script.Invoke( functionToInvoke, 0 );
				}

			} else {
				if( Icon ) {
					if( GUI.Button (elementRect, Icon) )
					{
						script.Invoke( functionToInvoke, 0 );
					}
				} 
			}
		}
	}

	void OnDrawGizmos() 
	{
		Gizmos.DrawGUITexture(elementRect, Icon);
	}
}
