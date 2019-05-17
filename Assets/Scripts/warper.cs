//**************************************************//
// Class Name: warper
// Class Description: Instantiable object in the RoboBUG game. This controls the warp objects and
//                    corresponds with the warper tool in that game.
// Methods:
// 		void Start()
//		void Update()
//		void OnTriggerEnter2D(Collider2D collidingObj)
// Author: Michael Miljanovic
// Date Last Modified: 6/1/2016
//**************************************************//

using UnityEngine;
using UnityEngine.UI; 
using System.Collections;
using System.IO;

public class warper : Tools
{
	public string Filename { get; set; }
	public string WarpToLine { get; set; }

	void OnTriggerEnter2D(Collider2D collidingObj) {
		if (collidingObj.name == stringLib.PROJECTILE_WARP) {
			string sMessage = stringLib.LOG_WARPED + Filename;
			Destroy(collidingObj.gameObject);
            if (!toolgiven)
            {
                toolgiven = true;
                for (int i = 0; i < tools.Length; i++)
                {
                    if (tools[i] > 0)
                    {
                        lg.floatingTextOnPlayer(GlobalState.StringLib.COLORS[i]);
                    }
                    selectedTool.toolCounts[i] += tools[i];                    Debug.Log(i + ": " + tools[i]);
                }
            }
            string filepath = Application.streamingAssetsPath + "/" + GlobalState.GameMode + "leveldata/" + Filename;
            
            // Path.Combine(Application.streamingAssetsPath, GlobalState.GameMode + "leveldata");
            // filepath = Path.Combine(filepath,  GlobalState.FilePath + Filename);
            //factory = new LevelFactory(filepath);
            GameObject.Find("Main Camera").GetComponent<GameController>().WarpLevel(filepath, WarpToLine);
           
		}
	}
}
