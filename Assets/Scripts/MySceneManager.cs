using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MySceneManager : MonoBehaviour 
{
    public Text currentLevelText;
    public bool isEditMode = false;
    public GameObject[] allLevels = new GameObject[0];
    public GameObject[] alllevelsIcon = new GameObject[0];

    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            SceneManager.LoadScene(1);
            return;
        }

        currentLevelText.text = "LEVEL " + (GetLevelCount() + 1).ToString();
        SaveAndLoad gameSave = GetComponent<SaveAndLoad>();
        gameSave.fileName = GetLevel().ToString();
        gameSave.LoadLevel();
        if (allLevels.Length > 0)
            SetLevelBackground();

        if (isEditMode)
            return;
        gameSave.LoadPlayerSave();

    }

    void SetLevelBackground()
    {
        int i = (GetLevelCount() / 5) % 5;
        i = Mathf.Clamp(i, 0, allLevels.Length);
        allLevels[i].SetActive(true);

        if (alllevelsIcon.Length > 0)
        {
            alllevelsIcon[i].SetActive(true);
            float f = GetLevelCount() / 5f;
            f -= GetLevelCount() / 5;
            f += .2f;
            alllevelsIcon[i].GetComponent<Image>().fillAmount = f;
        }

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
        int l = GetLevel();
        l++;
        if (l > 85)
            l = 50;        
        AddNextLevelCount();
        PlayerPrefs.SetInt("level", l);
    }

    public static void AddNextLevelCount ()
    {
        int l = PlayerPrefs.GetInt("levelCount", GetLevel());
        l++;
        PlayerPrefs.SetInt("levelCount", l);
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
        int money = PlayerPrefs.GetInt("money", 30000000);
        return money;
    }

    public static void AddSecondMoney(int money)
    {
        int m = GetSecondMoney() + money;
        m = Mathf.Max(0, m);
        PlayerPrefs.SetInt("secondmoney", m);
    }

    public static void SetSecondMoney(int m)
    {
        PlayerPrefs.SetInt("secondmoney", m);
    }

    public static int GetSecondMoney()
    {
        int money = PlayerPrefs.GetInt("secondmoney", 1);
        return money;
    }

    public static int GetRageLevel ()
    {
        int r = PlayerPrefs.GetInt("rage", 1);
        return r;
    }

    public static void AddNextLevelRage ()
    {
        int r = GetRageLevel();
        r++;
        PlayerPrefs.SetInt("rage", r);
    }

    public static int GetHealingLevel()
    {
        int r = PlayerPrefs.GetInt("healing", 1);
        return r;
    }

    public static void AddNextLevelHealing()
    {
        int r = GetHealingLevel();
        r++;
        PlayerPrefs.SetInt("healing", r);
    }

    public static int GetFreezingLevel()
    {
        int r = PlayerPrefs.GetInt("freezing", 1);
        return r;
    }

    public static void AddNextLevelFreezing()
    {
        int r = GetFreezingLevel();
        r++;
        PlayerPrefs.SetInt("freezing", r);
    }

    public static int GetLevel ()
    {
        int l = PlayerPrefs.GetInt("level", 0);
        return l;
    }

    [ContextMenu("SetLevel")]
    public void SetLevel ()
    {        
        SaveAndLoad gameSave = GetComponent<SaveAndLoad>();
        //gameSave.fileName = gameSave.fileName;        
        gameSave.LoadLevel();
    }

    public static int GetLevelCount()
    {
        int l = PlayerPrefs.GetInt("levelCount", GetLevel());
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
    public void ResetMoneyInEditor()
    {
        ResetMoney();
        ResetBotCount();
    }

    public static void ResetMoney ()
    {
        PlayerPrefs.SetInt("money", 30);
    }

    [ContextMenu("ResetLevel")]
    public void ResetLevelInEditor() => ResetLevel();

    public static void ResetLevel ()
    {
        PlayerPrefs.SetInt("level", 1);
        PlayerPrefs.DeleteKey("levelCount");
    }

    public static void ResetBotCount ()
    {
        SetManCount(10);
        SetArrowmanCount(10);
    }

    public static int GetMidleCount ()
    {
        int i = (GetArrowmanCount() + GetManCount()) / 2;
        return i;
    }
}
