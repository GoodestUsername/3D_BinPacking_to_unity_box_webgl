using UnityEngine;
using Sharp3DBinPacking;
using System.IO;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;
using Newtonsoft.Json;
/// <summary>
/// @Author: Eric Dong, Kibum
/// @version 2.0
/// Class that spawns the objects in unity.
/// </summary>
public class SpawnerScript : MonoBehaviour
{
    //The BoxGen prefab is what will be initialized and instantiated
    public Transform prefab;
    //Holds the array of integers that gives the dimensions of each box
    public int[][] dimensions;
    //Holds the starting positions of each box after they were generated with the 3DBinPacking
    public int[][] coordinates;
    static string WRITE_FILE_PATH = "Assets/Scripts/text_files/write.txt";
    public  TextAsset mockjson;
    // Start is called before the first frame update
    private static List<Cuboid> getCuboidsFromFile(string path)
    {
        List<Cuboid> cubes = new List<Cuboid>();
        string line;
        try
        {
            using (StreamReader streamReader = new StreamReader(path))
            {
                while ((line = streamReader.ReadLine()) != null)
                {
                    cubes = JsonConvert.DeserializeObject<List<Cuboid>>(line);
                }
            }
        }
        catch (IOException e)
        {
            Console.Write(e.ToString());
        }

        return cubes;
    }
    /// <summary>
    /// @Author: Eric Dong, Kibum
    /// @version 2.0
    /// Gets boxes and locations from algorithm output text file, and start the box generation with it.
    /// </summary>
    void Start()
    {
        List<Cuboid> cubes = getCuboidsFromFile(WRITE_FILE_PATH);
        dimensions = new int[cubes.Count][];
        coordinates = new int[cubes.Count][];

        for (int i = 0; i != cubes.Count; i++)
        {
            dimensions[i] = new int[3] { (int)cubes[i].Width, (int)cubes[i].Height, (int)cubes[i].Depth };
            coordinates[i] = new int[3] { (int)cubes[i].X, (int)cubes[i].Y, (int)cubes[i].Z };
        }
        //generateWebBoxes(mockjson.text);
        generateBoxes(dimensions, coordinates);
    }
    
    BinPackResult binPackWebBoxes(string[][] boxes, string[] room) {
        List<Cuboid> cubes = new List<Cuboid>();
        foreach(string[] box in boxes) {
            Cuboid cube = new Cuboid(Convert.ToDecimal(box[1]), Convert.ToDecimal(box[2]), Convert.ToDecimal(box[3]), 0, box[0]);
            Debug.Log(cube);
            cubes.Add(cube);
        }
        var binPacker = BinPacker.GetDefault(BinPackerVerifyOption.BestOnly);
        BinPackParameter parameter = new BinPackParameter(Convert.ToDecimal(room[0]), Convert.ToDecimal(room[1]), Convert.ToDecimal(room[2]), cubes);
        return binPacker.Pack(parameter);               
    }

    string[][] jsonDeserializeBox(BoxListJSON boxesjson) {
        string[][] boxarray = new string[boxesjson.boxes.Length][];
        for(int i = 0; i < boxesjson.boxes.Length; i++) {
            string[] temp = new string[4];
            temp[0] = boxesjson.boxes[i].id;
            temp[1] = boxesjson.boxes[i].x;
            temp[2] = boxesjson.boxes[i].y;
            temp[3] = boxesjson.boxes[i].z;
            boxarray[i] = temp;           
         
        }
        return boxarray;
    }

    string[] jsonDeserializeRoom(RoomJSON room) {
        string[] roomarray = new string[3];        
            roomarray[0] = room.x;
            roomarray[1] = room.y;
            roomarray[2] = room.z;        
        return roomarray;

    }
    //Preliminary Method for Working With Database
    void generateWebBoxes(string json) {
        BoxListJSON boxes = JsonUtility.FromJson<BoxListJSON>(json);
        RoomJSON room = boxes.room;
        BinPackResult packedboxes = binPackWebBoxes(jsonDeserializeBox(boxes), jsonDeserializeRoom(room));
        Debug.Log(packedboxes.BestResult[0].Count);
        for(int i = 0; i < packedboxes.BestResult[0].Count; i++) {
            Cuboid box = packedboxes.BestResult[0][i];
            var newobj = Instantiate(prefab, new Vector3((float)box.Width, (float)box.Height, (float)box.Depth), Quaternion.identity);

            //MeshRenderer creates the meshes needed to visualize each box
            MeshRenderer meshrend = newobj.GetComponent<MeshRenderer>();
            //Assigns a random color for each box to allow differentiation
            meshrend.material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);

            //Sets the label for each box
            float newx = ((float)box.X + (float)(box.Width / 2));
            float newy = ((float)box.Y + (float)(box.Height / 2));
            float newz = ((float)box.Z + (float)(box.Depth / 2));

            //Runs the prefab script so the box is generated
            int[] dimensions = new int[3] {(int)box.Width, (int)box.Height, (int)box.Depth};
            int[] coordinates = new int[3] {(int)box.X, (int)box.Y, (int)box.Z};
            var newobjscript = newobj.GetComponent<BoxGenerator>();
            newobjscript.testdimensions = dimensions;
            newobjscript.testcoordinates = coordinates;
            newobjscript.webboxid = box.Tag.ToString();
        }
    }
    //Creates several BoxGen Objects based on how many elements are in the coordinates/dimensions array
    void generateBoxes(int[][] dimensions, int[][] coordinates) {
        if(dimensions.Length == coordinates.Length) {
            for(int i = 0; i < dimensions.Length; i++) {
                var newobj = Instantiate(prefab, new Vector3(coordinates[i][0], coordinates[i][1], coordinates[i][2]), Quaternion.identity);

                //MeshRenderer creates the meshes needed to visualize each box
                MeshRenderer meshrend = newobj.GetComponent<MeshRenderer>();
                //Assigns a random color for each box to allow differentiation
                meshrend.material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);

                //Sets the label for each box
                float newx = (coordinates[i][0] / 1f+ (dimensions[i][0] / 2f));
                float newy = (coordinates[i][1] / 1f + (dimensions[i][1] / 2f));
                float newz = coordinates[i][2] / 1f + 0.5f;
                //newobj.Find("Canvas").localPosition = transform.InverseTransformPoint(new Vector3(newx, newy, newz));
                
                //Sets the text for the box's label
                //TMPro.TextMeshProUGUI labeltext = newobj.GetComponentInChildren<TMPro.TextMeshProUGUI>();
                //labeltext.text = "Box #" + i.ToString();
                
                //Provides a collider so that boxes do not clip into each other
                //boxcoll.size = transform.TransformVector(new Vector3(dimensions[i][0], dimensions[i][1], dimensions[i][2]));
                //boxcoll.center = transform.InverseTransformPoint(new Vector3(coordinates[i][0], coordinates[i][1] + (dimensions[i][2] / 2f), coordinates[i][2]));
                

                //Runs the prefab script so the box is generated
                var newobjscript = newobj.GetComponent<BoxGenerator>();
                newobjscript.testdimensions = dimensions[i];
                newobjscript.testcoordinates = coordinates[i];
                newobjscript.boxid = i;
            }
        }
    }


}
