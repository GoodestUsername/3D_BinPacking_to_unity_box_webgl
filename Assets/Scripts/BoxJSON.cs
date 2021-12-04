using System;
using UnityEngine;

/// <summary>
/// Class to serialize input json from db for individual boxes
/// </summary>
[Serializable]
public class BoxJSON 
{
  public string id;
  public string x;
  public string y;
  public string z;
  public string r;
  public string g;
  public string b;

}
