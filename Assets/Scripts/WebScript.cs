using UnityEngine;
using System.Runtime.InteropServices;

public class WebScript : MonoBehaviour
{
    // Start is called before the first frame update
    [DllImport("__Internal")]
    private static extern void SendBoxID(int boxid);
    public void IDMethod() {
        #if UNITY_WEBGL == true && UNITY_EDITOR == false
            SendBoxID(1);
        #endif
    }

}
