using UnityEngine;
using System.Collections;

public class UITextButton : UIElement
{
	[Tooltip("Link to script to be called on button press")]
	public MonoBehaviour 
		script;

	[Tooltip("Name of function to call on button press")]
	public string 
		functionToInvoke;

	[Tooltip("Button's text.")]
	public string 
		Text;
	
	void OnGUI( ) 
	{
		uiConfig();  // required to be called first
		
		if( useCustomStyle ) {
			
			if( GUI.Button ( elementRect, Text, elementStyle) )
			{
				script.Invoke( functionToInvoke, 0 );
			}
			
		} else {
			if( GUI.Button ( elementRect, Text) )
			{
				script.Invoke( functionToInvoke, 0 );
			}
			
		}
	}
}
