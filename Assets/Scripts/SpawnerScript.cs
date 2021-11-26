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
        
        generateBoxes(dimensions, coordinates);
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
                float newx = (coordinates[i][0] + (dimensions[i][0] / 2f));
                float newy = (coordinates[i][1] + (dimensions[i][1] / 2f));
                float newz = coordinates[i][2] + 0.5f;
                newobj.Find("Canvas").localPosition = transform.InverseTransformPoint(new Vector3(newx, newy, newz));
                
                //Sets the text for the box's label
                TMPro.TextMeshProUGUI labeltext = newobj.GetComponentInChildren<TMPro.TextMeshProUGUI>();
                labeltext.text = "Box #" + i.ToString();
                
                //Provides a collider so that boxes do not clip into each other
                BoxCollider boxcoll = newobj.GetComponent<BoxCollider>();
                boxcoll.size = new Vector3(dimensions[i][0], dimensions[i][1], dimensions[i][2]);
                boxcoll.center = new Vector3((coordinates[i][0] + (dimensions[i][0] / 2f)), (coordinates[i][1] + (dimensions[i][1] / 2f)), coordinates[i][2] + (dimensions[i][2] / 2f));

                //Runs the prefab script so the box is generated
                var newobjscript = newobj.GetComponent<BoxGenerator>();
                newobjscript.testdimensions = dimensions[i];
                newobjscript.testcoordinates = coordinates[i];
                newobjscript.boxid = i;
            }
        }
    }


}