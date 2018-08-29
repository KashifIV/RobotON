//**************************************************//
// Class Name: rename
// Class Description: Instantiable object for the Robot ON! game. This is used with the Renamer tool.
// Methods:
// 		void Start()
//		void Update()
//		void OnTriggerEnter2D(Collider2D collidingObj)
// Author: Michael Miljanovic
// Date Last Modified: 6/1/2016
//**************************************************//

using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;


public class rename : MonoBehaviour {

	public int index = -1;
	public int groupid = -1;
	public string correct;
	public string displaytext = "";
	public string innertext;
	public string language;
	public List<string> options;
	public GameObject SidebarObject;
	public GameObject CodescreenObject;
	public GameObject ToolSelectorObject;
  public AudioSource audioPrompt;
  public AudioSource audioCorrect;
	public bool answered = false;

	private bool answering = false;
	private bool decolorOnce = false;

	private int selection = 0;
	private LevelGenerator lg;

	//.................................>8.......................................
	// Use this for initialization
	void Start() {
		lg = CodescreenObject.GetComponent<LevelGenerator>();
	}

	//.................................>8.......................................
	// Update is called once per frame
	void Update() {
		if (answering) {
			// Handle left and right arrows --[
			if (selection == 0) {
				SidebarObject.GetComponent<GUIText>().text = displaytext + "   " + options[selection] + " →";
			}
			else if (selection == options.Count-1) {
				SidebarObject.GetComponent<GUIText>().text = displaytext + "← " + options[selection];
			}
			else {
				SidebarObject.GetComponent<GUIText>().text = displaytext + "← " + options[selection] + " →";
			}
			// ]-- End of handling arrows

			// Handle input --[
			if ((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))) {
				answered = true;
				answering = false;
				lg.isAnswering = false;
				if (selection != options.IndexOf(correct)) {
					// lg.isLosing = true;
					answered = false;
					ToolSelectorObject.GetComponent<SelectedTool>().outputtext.GetComponent<GUIText>().text = "The name you chose isn't the best option for\nthis variable's purpose.\nWhat is this variable used for?";
				}
				else {
					// Change this object to the correct text
					lg.taskscompleted[2]++;
					// Award 1 extra use of the tool.
					ToolSelectorObject.GetComponent<SelectedTool>().bonusTools[stateLib.TOOL_WARPER_OR_RENAMER]++;
					audioCorrect.Play();
					lg.innerXmlLines[index] = lg.innerXmlLines[index].Replace(innertext, correct);
					lg.DrawInnerXmlLinesToScreen();
					SidebarObject.GetComponent<GUIText>().text= "";
					// Change the next groupid objects to the new colors
					foreach(GameObject renames in lg.robotONrenamers) {
						if (renames.GetComponent<rename>().groupid == (groupid+1)) {
							int lineNum = renames.GetComponent<rename>().index;
							string sReplace = lg.outerXmlLines[lineNum];
							sReplace = lg.OuterToInnerXml(sReplace, language);
							lg.innerXmlLines[lineNum] = sReplace;
							lg.DrawInnerXmlLinesToScreen();
						}
					}
					lg.renamegroupidCounter++;
				}
			}
			else if (Input.GetKeyDown(KeyCode.RightArrow)) {
				selection = (selection + 1 <= options.Count - 1) ? selection + 1 : options.Count - 1;
			}
			else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
				selection = (selection - 1 >= 0) ? selection - 1 : 0;
			}
			// ]-- End of input handling
		}
		else if (lg.renamegroupidCounter != groupid && decolorOnce != true) {
			// Change the next groupid objects to the new colors
			decolorOnce = true;
			lg.innerXmlLines[index] = lg.innerXmlLines[index].Replace(innertext, lg.DecolorizeText(innertext));
			lg.DrawInnerXmlLinesToScreen();
		}

	}

	//.................................>8.......................................
	void OnTriggerEnter2D(Collider2D collidingObj) {
		if (collidingObj.name == stringLib.PROJECTILE_WARP && !answered && lg.renamegroupidCounter == groupid) {
			Destroy(collidingObj.gameObject);
			SidebarObject.GetComponent<GUIText>().text = displaytext;
			audioPrompt.Play();
			answering = true;
			lg.isAnswering = true;
			lg.toolsAirborne--;
		}
	}

	//.................................>8.......................................
}