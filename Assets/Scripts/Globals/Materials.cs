using UnityEngine;
using System.Collections;

//This class holds materials that NEED to be added via code.
//Any outlining or interactive shaders need to be in here.

//Shaders can be added here as long as they say shader in the name.

public static class Materials {
    public static Material OutlineEnemy;
    public static Material OutlineEnemyLock;
    public static Material OutlineTarget;
    public static Material OutlineAlly;

	public static Shader OutlineShader;

	public static void Initialize(){
        OutlineEnemy = Resources.Load<Material>("Materials/OutlineEnemy");
        OutlineEnemyLock = Resources.Load<Material>("Materials/OutlineEnemyLock");
        OutlineAlly = Resources.Load<Material>("Materials/OutlineAlly");
		OutlineTarget = Resources.Load<Material>("Materials/OutlineTarget");
		OutlineShader = OutlineTarget.shader;
	}
}
