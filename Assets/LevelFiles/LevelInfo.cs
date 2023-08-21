using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;

/// <summary>
/// Stores the user information about a level
/// </summary>
public class LevelInfo// : MonoBehaviour
{
    public static readonly string header = "pzl48INF";
    public static readonly string customLvlDir = "/customLvl";
    public static readonly string gameLvl =  "/gameLvl";
    public static readonly string extension = "lvlinf";

    public string path;
    public ushort index;

    public string levelName;
    public string author;
    public LevelData levelData;

    //public string menuName { get; private set; }

    public byte starsAchived;
    public byte bestScore;
    public bool solved;

    /*
    public LevelInfo(string filepath)
    {
        path = filepath;
        levelDataPath = "";
        leveldata = new levelData(levelDataPath);
        levelName = leveldata.levelName;

        starsAchived = -1;
        bestScore = -1;
        solved = false;
    }
    */
    //File IO
    /// <summary>
    /// Saves the LevelInfo to a separate file with a header
    /// </summary>
    /// <param name="savepath"></param>
    public void SaveToBinaryFile()
    {
        using (FileStream fileStream = new FileStream(Application.persistentDataPath + path, FileMode.OpenOrCreate))
        {
            BinaryWriter writer = new BinaryWriter(fileStream);

            //Writes the header into the file
            for (int i = 0; i < header.Length; i++)
            {
                writer.Write(header[i]);
            }
            //Then writes the variables
            ToBinaryWriter(writer);
            writer.Close();
        }
    }

    /// <summary>
    /// Writes the LevelInfo to a stream using a binary writer without a header
    /// </summary>
    /// <param name="writer"></param>
    public void ToBinaryWriter(BinaryWriter writer)
    {
        writer.Write(index);

        writer.Write(levelName);
        writer.Write(levelData.ToBase64());

        //writer.Write(menuName);

        writer.Write(starsAchived);
        writer.Write(bestScore);

        if (!solved)
            writer.Write((byte)0);
        else
            writer.Write((byte)1);

        writer.Close();
    }

    //Constructors
    /// <summary>
    /// Reads the levelInfo from a separate file with the header
    /// </summary>
    /// <param name="filepath"></param>
    public void LoadFromBinaryFile()
    {
        using (FileStream fileStream = new FileStream(Application.persistentDataPath + path, FileMode.Open))
        {
            BinaryReader reader = new BinaryReader(fileStream);

            //Check header
            for (int i = 0; i < header.Length; i++)
            {
                //Throw exception if charaters don't match
                if (reader.ReadChar() != header[i])
                    throw new NotLevelInfoFile(path);
            }
            FromBinaryReader(reader);

            reader.Close();
        }
    }

    public static LevelInfo CreateFromBinaryFile(string path)
    {
        LevelInfo temp = new LevelInfo
        {
            path = path
        };
        temp.LoadFromBinaryFile();
        return temp;
    }

    /// <summary>
    /// Reads the LevelInfo from a stream using a BinaryReader withput a header.
    /// </summary>
    /// <param name="reader"></param>
    public void FromBinaryReader(BinaryReader reader)
    {
        index = reader.ReadUInt16();

        levelName = reader.ReadString();
        levelData = new LevelData();
        levelData.FromBase64(reader.ReadString());

        //menuName = reader.ReadString();

        starsAchived = reader.ReadByte();
        bestScore = reader.ReadByte();

        if (reader.ReadByte() == 0)
            solved = false;
        else
            solved = true;
    }

    public static LevelInfo FromLevelData(string path, LevelData levelData, ushort index)
    {
        return new LevelInfo(path, index, levelData.levelName, levelData.authorName, levelData);
    }

    /// <summary>
    /// EDITOR use only!!!
    /// </summary>
    /// <param name="levelname"></param>
    /// <param name="leveldatapath"></param>
    /// <param name="menuname"></param>
    private LevelInfo(string Path, ushort Index, string levelname, string levelAuthor, LevelData leveldata)
    {
        path = Path;
        index = Index;
        levelName = levelname;

        author = levelAuthor;

        levelData = leveldata;

        //menuName = menuname;

        starsAchived = 0;
        bestScore = 255;

        solved = false;
    }

    private LevelInfo() { }

    public class NotLevelInfoFile : System.Exception
    {
        public NotLevelInfoFile(string filepath) : base(string.Format("The file {0} is not a LevelInfo file.", filepath))
        {

        }
    }
    public class IndexAlreadyExsist : System.Exception
    {
        public IndexAlreadyExsist(string filepath, ushort index) : base(string.Format("The file {0}'s index ({1}) is already in use.", filepath, index))
        {

        }
    }
}
