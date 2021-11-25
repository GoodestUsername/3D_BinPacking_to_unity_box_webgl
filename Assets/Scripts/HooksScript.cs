using System.Collections;
using System.Collections.Generic;
using UnityEngine;
///<summary>
///@author Kibum Park
///This script contains Hooks that allows the React Webpage that hosts the WebGL to send and recieve data.
///<summary>
public class HooksScript : MonoBehaviour
{    
    [SerializeField] private string roomJSON;
    //Reference to the target box
    public GameObject spawn;
    //Sends the ID of the selected box
    public void idHook(int boxid) {
        Debug.Log(boxid);
    }
    //Sends as an array, the contents of the selected box.
    public void contentsHook(string contents) {
        spawn = GameObject.Find("Spawner");
        //spawn.initialarray = contents;
    }
}
