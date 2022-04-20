using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MySceneManager : MonoBehaviour 
{
    public Text currentLevelText;
    public bool isEditMode = false;

    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            SceneManager.LoadScene(1);
            return;
        }

        currentLevelText.text = "LEVEL " + GetLevel().ToString();
        SaveAndLoad gameSave = GetComponent<SaveAndLoad>();        
        gameSave.fileName = GetLevel().ToString();
        gameSave.LoadLevel();
        

        if (isEditMode)
            return;
        gameSave.LoadPlayerSave();
        
    }

    /*
    private AsyncOperation nextScene;
    public Text currentLevelText;
    public Text currentMoneyText;
    public static int currentLevelNum;
    public static string currentSceneName;



    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            LoadMenu();
            return;
        }
        int currentLvl = PlayerPrefs.GetInt("level", 1);
        currentLevelNum = currentLvl;
        currentLevelText.text = "LEVEL "+ currentLvl.ToString();
        
        if (SceneManager.GetActiveScene().name == "menu")
        {            
            currentMoneyText.text = PlayerPrefs.GetInt("money", 0).ToString();
            if (currentLvl > SceneManager.sceneCountInBuildSettings-2)
            {
                //Debug.Log(" " + currentLvl  + " " + (currentLvl % (SceneManager.sceneCountInBuildSettings-2)));
                currentLvl = (SceneManager.sceneCountInBuildSettings - 6) + ((currentLvl % (SceneManager.sceneCountInBuildSettings - 2)) % 5);
                
                //currentLvl = Mathf.Clamp(currentLvl, 1, SceneManager.sceneCountInBuildSettings-1);
            }
            LoadNextLevelAsync("level" + currentLvl.ToString());
            
        }
        currentSceneName = "level" + currentLvl.ToString();
        Debug.Log(currentSceneName);
    }

    public void LoadSceneNext ()
    {
        nextScene.allowSceneActivation = true;
    }

    public void LoadNextLevelAsync (string currentLvl)
    {        
        StartCoroutine(LoadYourAsyncScene(currentLvl));
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("menu");
    }

    IEnumerator LoadYourAsyncScene(string scene)
    {
        nextScene = SceneManager.LoadSceneAsync(scene);
        nextScene.allowSceneActivation = false;

        while (!nextScene.isDone)
        {
            yield return null;
        }
    }
    */

    public static void Restart ()
    {
        int i = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(i);
    }


    public static void AddNextLevel ()
    {
        int l = PlayerPrefs.GetInt("level", 1);
        l++;
        if (l > 38)
            l = 2;
        PlayerPrefs.SetInt("level", l);
    }

    public static void AddMoney (int money)
    {
        int m = GetMoney() + money;
        PlayerPrefs.SetInt("money", m);
    }

    public static void SetMoney (int m)
    {
        PlayerPrefs.SetInt("money", m);
    }

    // TEST 10000 MONEY
    public static int GetMoney ()
    {
        int money = PlayerPrefs.GetInt("money", 30);
        return money;
    }

    public static int GetLevel ()
    {
        int l = PlayerPrefs.GetInt("level", 1);
        return l;
    }

    public static int GetManCount()
    {
        int l = PlayerPrefs.GetInt("manCount", 10);
        return l;
    }

    public static int GetArrowmanCount()
    {
        int l = PlayerPrefs.GetInt("arrowmanCount", 10);
        return l;
    }

    public static void SetManCount(int m)
    {
        PlayerPrefs.SetInt("manCount", m);
    }

    public static void SetArrowmanCount(int m)
    {
        PlayerPrefs.SetInt("arrowmanCount", m);
    }

    [ContextMenu("ResetMoney")]
    public void ResetMoneyInEditor ()
    {
        ResetMoney();
    }

    public static void ResetMoney ()
    {
        PlayerPrefs.DeleteKey("money");
        PlayerPrefs.DeleteKey("manCount");
        PlayerPrefs.DeleteKey("arrowmanCount");
    }

    [ContextMenu("ResetLevel")]
    public void ResetLevelInEditor ()
    {
        ResetLevel();
    }

    public static void ResetLevel()
    {
        PlayerPrefs.SetInt("level", 1);
    }

    [ContextMenu("ResetCount")]
    public static void ResetBotCount ()
    {
        SetManCount(10);
        SetArrowmanCount(10);
    }
}
