using Sharp3DBinPacking;
using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

/// <summary>
/// @Author: Eric Dong
/// @version 2.0
/// Class that reads room and box sizes from a text file, and outputs a list of cuboid objects with the 3d bin packing algorithm.
/// </summary>
class TextFileRead : MonoBehaviour
{
    static string m_Path = Application.dataPath;
    static string BIN_LABEL = "Bin: ";
    static string READ_FILE_PATH = "Assets/Scripts/text_files/read.txt";
    static string WRITE_FILE_PATH = "Assets/Scripts/text_files/write.txt";

    /// <summary>
    /// @Author: Eric Dong
    /// @version 1.0
    /// Temporary static constants for room width
    /// </summary>
    static private int roomWidth = 100;
    static private int roomHeight = 100;
    static private int roomDepth = 100;

    /// <summary>
    /// @Author: Eric Dong
    /// @version 1.0
    /// Constructor
    /// </summary>
    /// <param name="valuex"></param>
    /// <param name="valuey"></param>
    /// <param name="valuez"></param>
    public TextFileRead(int valuex, int valuey, int valuez)
    {
        roomWidth = valuex;
        roomHeight = valuey;
        roomDepth = valuez;
    }

    /// <summary>
    /// @Author: Eric Dong
    /// @version 1.0
    /// Return list of cuboids generated from file in path.
    /// </summary>
    /// <param name="path">File path.</param>
    /// <returns></returns>
    private static IEnumerable<Cuboid> getCuboidsFromFile(string path)
    {
        List<Cuboid> cubes = new List<Cuboid>();
        try
        {
            using (StreamReader streamReader = new StreamReader(path))
            {
                string line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    string[] dimensions = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    Cuboid cube = new Cuboid(Int32.Parse(dimensions[0]), Int32.Parse(dimensions[1]), Int32.Parse(dimensions[2]));
                    cubes.Add(cube); 
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
    /// @Author: Eric Dong
    /// @version 2.0
    /// Writes list of cuboids into a text file as a json from bin packing result.
    /// </summary>
    /// <param name="path">Path to write to.</param>
    /// <param name="result">Bin packing output</param>
    /// <returns></returns>
    private static Boolean writeCubeAndCoordsToFile(String path, BinPackResult result)
    {
        try
        {
            if (File.Exists(path))
            {
                using (StreamWriter streamWriter = new StreamWriter(path))
                {
                    foreach (var bins in result.BestResult)
                    {
                        String json = JsonConvert.SerializeObject(bins);
                        streamWriter.WriteLine(json);
                    }

                }
            }
            else
            {
                Console.WriteLine("Path does not exist");
            }
        }
        catch (IOException e)
        {
            Console.Write(e.ToString());
            return false;
        }

        return true;
    }
    /// <summary>
    /// @Author: Eric Dong
    /// @version 2.0
    /// Runs at the initialization, reads from text file, and uses algorithm to bin pack it, then writes the result as a json in a text file for spawner script.
    /// </summary>
    [RuntimeInitializeOnLoadMethod]
    public static void BinPack()
    {
        IEnumerable<Cuboid> cubeList = getCuboidsFromFile(READ_FILE_PATH);
        var binPacker = BinPacker.GetDefault(BinPackerVerifyOption.BestOnly);
        BinPackParameter parameter = new BinPackParameter(roomWidth, roomHeight, roomDepth, cubeList);
        var result = binPacker.Pack(parameter);
        writeCubeAndCoordsToFile(WRITE_FILE_PATH, result);
    }
}



