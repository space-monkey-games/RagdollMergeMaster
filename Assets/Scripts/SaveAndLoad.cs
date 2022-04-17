using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[ExecuteAlways]
public class SaveAndLoad : MonoBehaviour
{
    public string fileName;

    [ContextMenu("SaveLevel")]
    public void SaveLevel() => SaveLevel(false);

    public void SaveLevel (bool isPlayerBots)
    {
        if (fileName == "" || fileName == null)
        {
            Debug.LogError("SET NAME");
            return;
        }

        Indicators[] all = FindObjectsOfType<Indicators>();


        BotCharacters[] bots = new BotCharacters[all.Length];
        for (int i = 0; i < all.Length; i++)
        {

            bool isPlayer = all[i].CompareTag("Player");
            if (isPlayer == isPlayerBots)
                bots[i] = new BotCharacters(all[i].level, all[i].transform.position.x, all[i].transform.position.y, all[i].transform.position.z, all[i].typeMan, isPlayer);
        }

        GameData gameData = new GameData(bots);
        SaveAndLoad saveAndLoad = new SaveAndLoad(fileName);
        saveAndLoad.Save(gameData);
        fileName = null;

#if UNITY_EDITOR
        if (!UnityEditor.EditorApplication.isPlaying)
        {
            foreach (Indicators i in all)
                DestroyImmediate(i.transform.root.gameObject);
        }
#endif
    }

    [ContextMenu("LoadLevel")]
    public void LoadLevel ()
    {
        if (fileName == "" || fileName == null)
        {
            Debug.LogError("SET NAME");
            return;
        }
        try
        {
            SaveAndLoad saveAndLoad = new SaveAndLoad(fileName);
            GameData gameData = (GameData)saveAndLoad.Load();
            if (gameData == null)
            {
                MySceneManager.ResetLevel();
                MySceneManager.Restart();
                return;
            }
            Lineup lineup = FindObjectOfType<Lineup>();
        
            foreach (BotCharacters bot in gameData.allBot)
            {
                if (bot != null)
                    lineup.InstantiateLevelMan(bot.typeMan, bot.level, new Vector3(bot.posX, bot.posY, bot.posZ), bot.isPlayer);

            }
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
            
        }

    }




    public SaveAndLoad (string _fileName)
    {
        fileName = _fileName;
    }






    public void Save (object saveData)
    {
        string file = Application.dataPath + "/Resources/" + fileName + ".bytes";

        using (FileStream fstream = new FileStream(file, FileMode.OpenOrCreate))
        {
            byte[] bts = ObjectToByteArray(saveData);
            fstream.Write(bts, 0, bts.Length);
            Debug.Log("Save file: " + fileName);
        }
    }

    public void Save(object saveData, string file)
    {
        using (FileStream fstream = new FileStream(file, FileMode.Create))
        {
            byte[] bts = ObjectToByteArray(saveData);
            fstream.Write(bts, 0, bts.Length);
        }
    }

    public object Load ()
    {
        try
        {
            byte[] bytes = ((TextAsset)Resources.Load(fileName, typeof(TextAsset))).bytes;
            GameData gd = ByteArrayToObject(bytes);
            return gd;
        }
        catch
        {
            return null;
        }
    }

    public void SaveMySave()
    {
        fileName = "PlayerSave";

        Indicators[] all = FindObjectsOfType<Indicators>();


        BotCharacters[] bots = new BotCharacters[all.Length];
        for (int i = 0; i < all.Length; i++)
        {
            bool isPlayer = all[i].CompareTag("Player");
            if (isPlayer)
                bots[i] = new BotCharacters(all[i].level, all[i].transform.position.x, all[i].transform.position.y, all[i].transform.position.z, all[i].typeMan, isPlayer);
        }

        GameData gameData = new GameData(bots);
        SaveAndLoad saveAndLoad = new SaveAndLoad(fileName);
        string path = Application.persistentDataPath + "/" + fileName + ".bytes";
        saveAndLoad.Save(gameData, path);
    }


    public object LoadMySave ()
    {
        fileName = "PlayerSave";
        string file = Application.persistentDataPath + "/" + fileName + ".bytes";
        using (FileStream fstream = new FileStream(file, FileMode.Open))
        {
            if (fstream == null)
                return null;
            byte[] bytes = new byte[fstream.Length];
            fstream.Read(bytes, 0, bytes.Length);
            GameData gd = ByteArrayToObject(bytes);
            return gd;
        }
    }

    public void LoadPlayerSave()
    {
        SaveAndLoad saveAndLoad = new SaveAndLoad(fileName);
        GameData gameData = (GameData)saveAndLoad.LoadMySave();
        if (gameData == null)
            return;
        Lineup lineup = FindObjectOfType<Lineup>();
        try
        {
            foreach (BotCharacters bot in gameData.allBot)
            {
                if (bot != null)
                    lineup.InstantiateLevelMan(bot.typeMan, bot.level, new Vector3(bot.posX, bot.posY, bot.posZ), bot.isPlayer);

            }
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
        }

    }

    public static byte[] ObjectToByteArray(object obj)
    {
        BinaryFormatter bf = new BinaryFormatter();
        using (var ms = new MemoryStream())
        {
            bf.Serialize(ms, obj);
            return ms.ToArray();
        }
    }

    public static GameData ByteArrayToObject(byte[] arrBytes)
    {
        using (var memStream = new MemoryStream())
        {
            var binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            GameData obj = (GameData)binForm.Deserialize(memStream);
            return obj;
        }
    }
}
