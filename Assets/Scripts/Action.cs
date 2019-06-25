using System.Collections; 
using System.Collections.Generic; 
using UnityEngine; 

public enum ActionType{Dialog, Throw, SwitchTool}
public class Action{
    Vector3 position;  
    public string text; 
    public int lineNumber, Column;
    CodeProperties properties; 
    public Vector3 Position {
        get{
            return new Vector3(Column*((float)(Screen.width)/12800f) - ((float)(Screen.width)/192f), properties.initialLineY- properties.linespacing*lineNumber + stateLib.TOOLBOX_Y_OFFSET, 1); 
        }
    }
    int projectileCode {get;set;} 
    public ActionType Category {get;set;}
    public Action(CodeProperties props, ActionType type, int row, int col, string dialog = "", int code = -1){
        Category = type; 
        projectileCode = code; 
        text = dialog; 
        lineNumber = row; 
        properties = props; 
        Column = col; 
    }
    
    public void setPosition(Vector3 pos){
        this.position = pos; 
    }
}