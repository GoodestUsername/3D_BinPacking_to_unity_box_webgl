using System;
using UnityEngine;

/// <summary>
/// Class to serialize room and its list of boxes from json.
/// </summary>
[Serializable]
public class BoxListJSON 
{
   public RoomJSON room;
   public BoxJSON[] boxes;
}
