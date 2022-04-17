
using System;

[Serializable]
public class BotCharacters
{
    public int level;
    public float posX;
    public float posY;
    public float posZ;
    public TypeMan typeMan;
    public bool isPlayer;
    
    public BotCharacters ()
    { }


    public BotCharacters (int _level, float x, float y, float z, TypeMan _type, bool _isPlayer)
    {
        level = _level;
        posX = x;
        posY = y;
        posZ = z;
        typeMan = _type;
        isPlayer = _isPlayer;
    }
}

[Serializable]
public class GameData
{

    public BotCharacters[] allBot;

    public GameData(BotCharacters[] bots)
    {
        allBot = new BotCharacters[bots.Length];
        Array.Copy(bots, allBot, bots.Length);
    }
}



