using UnityEngine;
using System.Runtime.InteropServices;
/// <summary>
/// @author Kibum Park
/// @version 2.0
/// This script assigns the coordinates and dimensions of each box generated by the 3DBinPacking algorithm.
/// This is where the Mesh can create the appropriate faces of the box using triangle generated from the vertices and coordinates.
/// </summary>
[RequireComponent(typeof (MeshFilter))]
[RequireComponent(typeof (MeshRenderer))]
[RequireComponent(typeof (MeshCollider))]
public class BoxGenerator : MonoBehaviour
{
    public int[] coordinates;
    public int[] dimensions;
    public int boxid;
    public string webboxid;
    private float dist;
    private Vector3 v3Offset;
    private Plane plane;

    /// <summary>
    /// Create cube with coords and dimensions.
    /// </summary>
    void Start()
    {
        createCube(testcoordinates, testdimensions);
    }

    //To create a box, there needs to be eight points of the box set.
    //This is done by using a variety of combinations between coodinates/dimensions points and their sums.
    public void createCube(int[] coor, int[] dims) {
        Vector3[] vertices = {
            transform.InverseTransformPoint(new Vector3 (coor[0], coor[1], coor[2])),
            transform.InverseTransformPoint(new Vector3 (coor[0] + dims[0], coor[1], coor[2])),
            transform.InverseTransformPoint(new Vector3 (coor[0] + dims[0], coor[1] + dims[1], coor[2])),
            transform.InverseTransformPoint(new Vector3 (coor[0], coor[1] + dims[1], coor[2])),
            transform.InverseTransformPoint(new Vector3 (coor[0], coor[1] + dims[1], coor[2] + dims[2])),
            transform.InverseTransformPoint(new Vector3 (coor[0] + dims[0], coor[1] + dims[1], coor[2]+ dims[2])),            
            transform.InverseTransformPoint(new Vector3 (coor[0] + dims[0], coor[1], coor[2] + dims[2])),
            transform.InverseTransformPoint(new Vector3 (coor[0], coor[1], coor[2] + dims[2])),     
        };
        // Debug.Log(vertices);
        // for(int i = 0; i < vertices.Length; i++) {
        //     vertices[i] = transform.TransformPoint(vertices[i]);
        // }

        int[] triangles = {
            0, 2, 1, //face front
            0, 3, 2,
            2, 3, 4, //face top
            2, 4, 5,
            1, 2, 5, //face right
            1, 5, 6,
            0, 7, 4, //face left
            0, 4, 3,
            5, 4, 7, //face back
            5, 7, 6,
            0, 6, 7, //face bottom
            0, 1, 6
        };

        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.vertices = vertices;
        mesh.triangles = triangles;  
        MeshCollider meshcol = GetComponent<MeshCollider>();
        meshcol.sharedMesh = mesh;    
    }

    [DllImport("__Internal")]
    private static extern void SendBoxID(int boxid);
     
    //Allows the mouse cursor to hold the box in place while the left mouse button is held down and immobile
     void OnMouseDown() {
         plane.SetNormalAndPosition(Camera.main.transform.forward, transform.position);
         Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
         float dist;
         plane.Raycast (ray, out dist);
         v3Offset = transform.position - ray.GetPoint (dist);                
         //SendBoxID(boxid);  
         Debug.Log(boxid);
     }
     
    //Allows the box to move and follow the cursor while th mouse button is held down and mobile
     void OnMouseDrag() {
          Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
          float dist;
          plane.Raycast (ray, out dist);
          Vector3 v3Pos = ray.GetPoint (dist);
          transform.position = v3Pos + v3Offset;    
     }
}
