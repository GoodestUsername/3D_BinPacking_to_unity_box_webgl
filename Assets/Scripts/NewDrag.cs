using System.Collections;
using System.Collections.Generic;
using UnityEngine;

  /// <summary>
  /// @author Kibum Park
  /// @version 2.0
  /// The script that allows the user to move boxes around using their mouse.
  /// </summary>
 public class NewDrag: MonoBehaviour {
     private float dist;
     private Vector3 v3Offset;
     private Plane plane;
     
    //Allows the mouse cursor to hold the box in place while the left mouse button is held down and immobile
     void OnMouseDown() {
         plane.SetNormalAndPosition(Camera.main.transform.forward, transform.position);
         Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
         float dist;
         plane.Raycast (ray, out dist);
         v3Offset = transform.position - ray.GetPoint (dist);         
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
