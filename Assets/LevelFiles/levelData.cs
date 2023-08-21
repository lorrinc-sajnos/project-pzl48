using System;
using System.Collections;
using System.Collections.Generic;

using System.IO;
using UnityEngine;

public class LevelData //: MonoBehaviour
{
    //Primary Variables
    public int levelVersion;

    public string levelName;
    public string authorName;

    public Vector2Int boardSize;

    public int solutionCount;
    public Vector2Int[] solutionSwipes;

    public int goalCount;
    public bool[,] isGoal;

    public int[,] startSetup;

    //Variables that can be assigned from data
    public int playerCount;
    public int[] movesForStars;

    //Variables for randomization
    public bool generateRandom;
    int minGen;
    int maxGen;
    public int[] indexToGenerate;
    public float[] chanceToGenerate;


    //IO stuff
    public const string levelDirectory = "Assets/Levels/";
    public const string header = "pzl48";

    public static LevelData CreateFromBase64(string base64)
    {
        var temp = new LevelData();
        temp.FromBase64(base64);
        return temp;
    }
    //IO
    public void FromBase64(string text)
    {
        //So basically what happens here is that we 
        //create a new memory stream from the text, 
        //whics is in base64,
        //and for its file path we pass in the 
        //file itself as text.

        FromStream(new MemoryStream(
            Convert.FromBase64String(
                text
                )
            ), "Text:" + text);

    }
    public void FromFile(string path)
    {
        using (FileStream fileStream = new FileStream(levelDirectory + path, FileMode.Open))
        {
            FromStream(fileStream, path);
            fileStream.Close();
        }
    }
    public void FromStream(Stream stream, string path)
    {
        Debug.Log("Starting Level reading");
        BinaryReader reader = new BinaryReader(stream);

        //Checking the header
        for (int i = 0; i < header.Length; i++)
        {
            if (header[i] != reader.ReadChar())
                throw new LevelIOException("Header is not correct in file ", path);
        }

        levelVersion = reader.ReadByte();

        //Version interpretation
        if (levelVersion == 0 || levelVersion == 1)
        {
            //Read in name
            levelName = reader.ReadString();
            //Read in author
            authorName = reader.ReadString();

            //Board size
            boardSize = SplitByte(reader.ReadByte());
            if (boardSize.x < 2 || boardSize.y < 2)
                throw new LevelIOException("This size for the grid(" + boardSize.x + " " + boardSize.y + ") is not allowed", path);

            //Misc boolean
            bool[] misc = Byte2Bool(reader.ReadByte());

            generateRandom = misc[0];

            //

            startSetup = new int[boardSize.x, boardSize.y];

            //Filling in solution
            solutionCount = reader.ReadByte();

            //Reading solution swipes
            solutionSwipes = new Vector2Int[solutionCount];
            Debug.Log(solutionCount);


            int solutionByteCount = solutionCount / 4;

            if (solutionCount % 4 != 0)
                solutionByteCount++;

            Debug.Log(solutionByteCount);

            Vector2Int[] temp;
            int counter = 0;
            for (int i = 0; i < solutionByteCount; i++)
            {
                temp = DirectionsFromByte(reader.ReadByte());

                for (int j = 0; j < 4 && counter < solutionCount; j++)
                {
                    solutionSwipes[counter] = temp[j];
                    counter++;
                }
            }

            //Goal positions
            goalCount = reader.ReadByte();
            isGoal = new bool[boardSize.x, boardSize.y];
            Vector2Int pos;

            for (int i = 0; i < goalCount; i++)
            {
                pos = SplitByte(reader.ReadByte());
                isGoal[pos.x, pos.y] = true;
            }

            //Diversion between verson 0 and 1

            //Version 0 is storing pieces with a coordinate and an index
            if (levelVersion == 0)
            {
                int pieceCount = reader.ReadByte();
                Vector2Int currentPos;

                for (int i = 0; i < pieceCount; i++)
                {
                    currentPos = SplitByte(reader.ReadByte());
                    startSetup[currentPos.x, currentPos.y] = reader.ReadByte();
                }
            }
            //Version 1 stores the whole grid, thus empty spaces as well
            else
            {
                for (int i = 0; i < boardSize.x; i++)
                    for (int j = 0; j < boardSize.y; j++)
                    {
                        startSetup[i, j] = reader.ReadByte();
                    }
            }

            VariableInit();

        }
        else
            throw new LevelIOException("Level version " + levelVersion + " cannot be interpreted", path);

    }


    public MemoryStream ToStream( string path)
    {
        MemoryStream stream = new MemoryStream();
        BinaryWriter writer = new BinaryWriter(stream);

        for (int i = 0; i < header.Length; i++)
        {
            writer.Write(header[i]);
        }

        writer.Write((byte)levelVersion);

        //Encoding evaluation
        if (levelVersion == 0 || levelVersion == 1)
        {
            writer.Write(levelName);
            writer.Write(authorName);

            writer.Write(StickByte(boardSize));
            //PLaceholder for miscelanious booleans
            writer.Write((byte)0);

            //Solutions
            writer.Write((byte)solutionCount);

            writer.Write(BytesFromDirections(solutionSwipes));

            //Goals
            writer.Write((byte)goalCount);

            for (int i = 0; i < boardSize.x; i++)
                for (int j = 0; j < boardSize.y; j++)
                {
                    if (isGoal[i, j])
                        writer.Write(StickByte(new Vector2Int(i, j)));
                }

            //Split between VER 0 and 1
            if (levelVersion == 0)
            {
                List<(byte, byte)> temp = new List<(byte, byte)>();
                for (int i = 0; i < boardSize.x; i++)
                    for (int j = 0; j < boardSize.y; j++)
                    {
                        if (startSetup[i, j] != 0)
                        {
                            temp.Add((StickByte(new Vector2Int(i, j)), (byte)startSetup[i, j]));
                        }
                    }

                writer.Write((byte)temp.Count);

                for (int i = 0; i < temp.Count; i++)
                {
                    writer.Write(temp[i].Item1);
                    writer.Write(temp[i].Item2);
                }
            }
            else
            {
                for (int i = 0; i < boardSize.x; i++)
                    for (int j = 0; j < boardSize.y; j++)
                        writer.Write((byte)startSetup[i, j]);
            }

        }
        else
            throw new LevelIOException("Level version " + levelVersion + " cannot be interpreted", path);


        return stream;
    }
    public void ToFile(string path)
    {
        byte[] levelBytes = ToStream(path).ToArray();

        if (!File.Exists(path))
            File.Create(path);

        File.WriteAllBytes(path, levelBytes);
    }
    public string ToBase64()
    {
        byte[] levelBytes = ToStream("base64COV").ToArray();

        string levelInBase64 =Convert.ToBase64String(levelBytes, Base64FormattingOptions.None);

        return levelInBase64;
    }

    public void VariableInit()
    {
        //setting StarCount
        movesForStars = new int[3];

        movesForStars[2] = solutionCount;
        movesForStars[1] = (int)(1.25f * solutionCount);

        //Failsafe: it must be at least 1 more
        while (movesForStars[1] <= movesForStars[2])
            movesForStars[1]++;

        movesForStars[0] = (int)(1.5f * solutionCount);

        //Failsafe: it must be at least 1 more
        while (movesForStars[0] <= movesForStars[1])
            movesForStars[0]++;

        //Counting players
        playerCount = 0;
        for (int i = 0; i < boardSize.x; i++)
            for (int j = 0; j < boardSize.y; j++)
            {
                if (startSetup[i, j] == 20)
                    playerCount++;
            }
    }

    Vector2Int SplitByte(byte number)
    {
        return new Vector2Int(number / 16, number % 16);
    }
    byte StickByte(Vector2Int numbers)
    {
        return (byte)(numbers.x * 16 + numbers.y);
    }


    Vector2Int[] DirectionsFromByte(byte num)
    {
        Vector2Int[] directions = new Vector2Int[4];

        int[] dirInt = new int[4];

        dirInt[0] = num / 64;
        dirInt[1] = num % 64 / 16;
        dirInt[2] = num % 16 / 4;
        dirInt[3] = num % 4;

        for (int i = 0; i < 4; i++)
            switch (dirInt[i])
            {
                case 0:
                    directions[i] = Vector2Int.up;
                    break;

                case 1:
                    directions[i] = Vector2Int.right;
                    break;

                case 2:
                    directions[i] = Vector2Int.down;
                    break;

                case 3:
                    directions[i] = Vector2Int.left;
                    break;
            }


        return directions;
    }
    byte[] BytesFromDirections(Vector2Int[] directions)
    {
        int byteCount = solutionCount / 4;
        int remainder = solutionCount % 4;
        if (remainder != 0)
            byteCount++;

        byte[] solutionMoves = new byte[byteCount];
        byte[] temp;
        for (int i = 0; i < byteCount; i++)
        {
            temp = new byte[4];
            for (int j = 0; j < 4 && i * 4 + j < solutionCount; j++)
            {
                if (directions[i * 4 + j] == Vector2Int.up)
                    temp[j] = 0;
                else if (directions[i * 4 + j] == Vector2Int.right)
                    temp[j] = 1;
                else if (directions[i * 4 + j] == Vector2Int.down)
                    temp[j] = 2;
                else if (directions[i * 4 + j] == Vector2Int.left)
                    temp[j] = 3;
            }
            int tempInt;
            tempInt = temp[0] * 64;
            tempInt += temp[1] * 16;
            tempInt += temp[2] * 4;
            tempInt += temp[3];

            solutionMoves[i] = (byte)tempInt;
        }

        return solutionMoves;
    }

    bool[] Byte2Bool(byte num)
    {
        bool[] bits = new bool[8];
        string temp = System.Convert.ToString(256 + num, 2);

        for (int i = 0; i < 8; i++)
        {
            if (temp[i + 1] == '1')
                bits[i] = true;
        }
        return bits;
    }


    public class LevelIOException : System.Exception
    {
        public LevelIOException(string message, string path)
            : base(message + ", in file \"" + levelDirectory + path + "\"")
        {
        }
    }
}
