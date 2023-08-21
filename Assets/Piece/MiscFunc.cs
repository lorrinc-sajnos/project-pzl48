using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

class MiscFunc
{
    public static void LogInt2D(int[,] array)
    {
        int width = array.GetLength(0);
        int height = array.GetLength(1);
        string mem;
        for (int i = 0; i < height; i++)
        {
            mem = "";
            for (int j = 0; j < width; j++)
                mem += array[j, i] + " ";
            Debug.Log(mem);
        }
    }
}

