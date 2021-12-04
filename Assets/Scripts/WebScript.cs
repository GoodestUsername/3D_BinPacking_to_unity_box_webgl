using UnityEngine;
using System.Runtime.InteropServices;
/// <summary>
/// Class for interacting with webscript
/// </summary>
public class WebScript : MonoBehaviour
{
    // Start is called before the first frame update
    [DllImport("__Internal")]
    private static extern void SendBoxID(int boxid);

    /// <summary>
    /// Sends box id to web
    /// </summary>
    public void IDMethod() {
        #if UNITY_WEBGL == true && UNITY_EDITOR == false
            SendBoxID(1);
        #endif
    }

}
