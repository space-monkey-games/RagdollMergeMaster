using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Lineup : MonoBehaviour
{
    public GameObject[] allMan;
    public GameObject[] allArrowman;
    public GameObject[] allBosses;
    public ParticleSystem mergeParticle;
    public AudioClip mergeAudioClip;
    private AudioSource _audioSource;
    public Material blueMaterial;
    public Material greenMaterial;
    public Material redMaterial;
    public Material orangeMaterial;
    public Material grayMaterial;
    public GameObject hpBarBlue;
    public GameObject hpBarRed;
    public GameObject freeAddManButton;
    public GameObject freeAddArrowmanButton;
    public GameObject boxIcon;
    public Text boxFreeMoney;

    [Space]
    public int coefficient = 25;
    public Text coinText;
    public Text manCoinText;
    public Text arrowmanCoinText;
    public int manCount = 10;
    public int arrowmanCount = 10;
    [Space]
    public Vector2 cellSize;
    public Vector2 gridSize = new Vector2(5, 3);
    public Vector2 offcetPosition;
    private Camera _myCamera;
    public LayerMask layerGrounded;
    public LayerMask layerPlayer;
    private Transform _target;
    private Vector3 _currentCell;
    private Vector3 _startCell;
    public static UnityEvent mergeEvent = new UnityEvent();


    void Start()
    {
        _myCamera = Camera.main;
        UpdateUI();
        _audioSource = FindObjectOfType<AudioSource>();
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _startCell = CheckCell(RaycastPosition(Input.mousePosition));
            _target = CheckTarget(_startCell);
            if (_target == null)
                return;
            _target.gameObject.layer = 0;

            AddLine(_target);
        }

        if (Input.GetMouseButton(0) && _target != null)
        {
            Vector3 newPos = RaycastPosition(Input.mousePosition);
            _target.position = newPos;
        }
        if (Input.GetMouseButtonUp(0) && _target != null)
        {            
            _currentCell = CheckCell(RaycastPosition(Input.mousePosition));
            _currentCell = ClampCell(_currentCell);
            SetPositionOnCell();
            ClearLine();
        }
    }

    Vector3 RaycastPosition(Vector3 inputPosition)
    {
        Vector3 newPos = Vector3.zero;
        Ray ray = _myCamera.ScreenPointToRay(inputPosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100000, layerGrounded))
        {
            if (hit.collider.CompareTag("Box"))
            {
                boxIcon.SetActive(true);
                boxFreeMoney.text = Convert(MySceneManager.GetMidleCount());
                Destroy(hit.collider.gameObject);

            }
            newPos = hit.point;
            newPos.y = 0;
        }
        return newPos;
    }

    Vector3 CheckCell(Vector3 inputPosition)
    {
        Vector3 newPos = new Vector3((Mathf.RoundToInt((inputPosition.x + offcetPosition.x) / cellSize.x)) * cellSize.x, 0, (Mathf.RoundToInt((inputPosition.z + offcetPosition.y) / cellSize.y)) * cellSize.y);

        return newPos;
    }

    Vector3 ClampCell(Vector3 newPos)
    {
        newPos = new Vector3(Mathf.Clamp(newPos.x, offcetPosition.x + cellSize.x, gridSize.x * cellSize.x), newPos.y, Mathf.Clamp(newPos.z, offcetPosition.y + cellSize.y, gridSize.y * cellSize.y));
        return newPos;
    }

    Vector3 GetCellByNumber(Vector2 num)
    {
        return new Vector3(cellSize.x * num.x + offcetPosition.x, 0, cellSize.y * num.y + offcetPosition.y);
    }

    Transform CheckTarget(Vector3 pos)
    {
        Transform _trg = null;
        Collider[] allColl = Physics.OverlapSphere(pos, cellSize.x / 2, layerPlayer);
        foreach (Collider col in allColl)
        {
            if (col.CompareTag("Player"))
            {
                _trg = col.transform;
                break;
            }
        }
        return _trg;
    }

    public GameObject[] currentPlayers = new GameObject[0];
    void AddLine(Transform tr)
    {
        if (tr == null)
            return;
        Indicators indicators = tr.GetComponent<Indicators>();
        currentPlayers = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject go in currentPlayers)
        {
            Indicators i = go.GetComponentInChildren<Indicators>();
            Outline outline = go.GetComponentInChildren<Outline>();
            if (i.typeMan == indicators.typeMan && i.level == indicators.level)
            {
                outline.OutlineColor = Color.green;
            }
            else
            {
                outline.SetMaterial(grayMaterial);
            }
        }
    }

    void ClearLine ()
    {
        foreach (GameObject go in currentPlayers)
        {
            Outline outline = go.GetComponentInChildren<Outline>();
            outline.SetStartMaterial();
            outline.OutlineColor = Color.black;
        }
    }

    void SetPositionOnCell()
    {
        Transform trInCell = CheckTarget(_currentCell);
        if (trInCell != null)
            Merger(trInCell, _target);
        else
            _target.position = _currentCell;

        _target.gameObject.layer = 3;//playerLayer
        _target = null;
    }

    int getLevel(GameObject go)
    {
        int level = 0;
        level = go.GetComponentInChildren<Indicators>().level;
        return level;
    }

    TypeMan GetTypeMan(GameObject go)
    {
        return go.GetComponentInChildren<Indicators>().typeMan;
    }

    void Merger(Transform trInCell, Transform tr)
    {
        if (GetTypeMan(trInCell.gameObject) != GetTypeMan(tr.gameObject))
        {
            ReturnStartCell(trInCell, tr);
            return;
        }
        int a = getLevel(trInCell.gameObject);
        int b = getLevel(tr.gameObject);
        if (a == b && a < 12)
        {
            RemoveGameObject(trInCell.root.gameObject);
            RemoveGameObject(tr.root.gameObject);
            InstantiateLevelMan(GetTypeMan(trInCell.gameObject), a + 1, trInCell.position, true);
        }
        else
            ReturnStartCell(trInCell, tr);
        ADSController.ShowInterstitial();
    }

    void ReturnStartCell(Transform trInCell, Transform tr)
    {
        //Vector3 pos = trInCell.position;

        tr.position = trInCell.position;
        trInCell.position = _startCell;
        //_target.position = _startCell;
    }

    public void InstantiateLevelMan(TypeMan typeMan, int currentLevel, Vector3 newPos, bool isPlayer)
    {
        if (isPlayer == false)
            newPos += new Vector3(0, 0, 25);
        GameObject go = null;
        if (typeMan == TypeMan.man)
        {
            go = Instantiate(allMan[currentLevel], newPos, Quaternion.identity);
            if (!go.activeSelf)
                go.SetActive(true);
        }
        else
        {
            if (typeMan == TypeMan.arrowman)
            {
                go = Instantiate(allArrowman[currentLevel], newPos, Quaternion.identity);
                if (!go.activeSelf)
                    go.SetActive(true);
            }
            else
            {
                go = Instantiate(allBosses[currentLevel], newPos, Quaternion.identity);
                if (!go.activeSelf)
                    go.SetActive(true);
            }
        }
        if (isPlayer)
        {
            go.GetComponentInChildren<Indicators>().gameObject.tag = "Player";            
            go.GetComponentInChildren<DamageBase>().hpBar = Instantiate(hpBarBlue);
            if (typeMan == TypeMan.man)
                go.GetComponentInChildren<SkinnedMeshRenderer>().material = blueMaterial;
            else
                go.GetComponentInChildren<SkinnedMeshRenderer>().material = greenMaterial;
            if (mergeEvent != null)
                mergeEvent.Invoke();
        }
        else
        {
            go.GetComponentInChildren<Indicators>().gameObject.tag = "Enemy";
            go.transform.rotation = Quaternion.Euler(0, 180, 0);
            if (typeMan != TypeMan.boss)
            {
                if (typeMan == TypeMan.man)
                    go.GetComponentInChildren<SkinnedMeshRenderer>().material = redMaterial;
                else
                    go.GetComponentInChildren<SkinnedMeshRenderer>().material = orangeMaterial;
            }
            go.GetComponentInChildren<DamageBase>().hpBar = Instantiate(hpBarRed);
        }
        if (_audioSource == null)
            return;
        mergeParticle.transform.position = newPos;
        mergeParticle.Play();
        _audioSource.PlayOneShot(mergeAudioClip);
        
    }

    public void AddNewManFree(bool isMan, int level)
    {
        if (isMan)
            MySceneManager.AddMoney(manCount);
        else
            MySceneManager.AddMoney(arrowmanCount);

        AddNewMan(isMan, level);
    }

    public void AddNewMan(bool isMan)
    {
        Vector3 newPos = Vector3.zero;
        for (int i = 1; i < gridSize.y + 1; i++)
        {
            for (int o = 1; o < gridSize.x + 1; o++)
            {
                Vector2 num = new Vector2(o, i);
                Transform tar = CheckTarget(GetCellByNumber(num));
                if (tar == null)
                    newPos = GetCellByNumber(num);
            }
        }

        if (newPos == Vector3.zero)
        {
            print("No clear cell");
            return;
        }

        if (isMan)
        {
            if (MySceneManager.GetMoney() < manCount)
                return;
            InstantiateLevelMan(TypeMan.man, 0, newPos, true);
            MySceneManager.AddMoney(-manCount);
            manCount += Mathf.CeilToInt(manCount * coefficient / 100);
            MySceneManager.SetManCount(manCount);
        }
        else
        {
            if (MySceneManager.GetMoney() < arrowmanCount)
                return;
            InstantiateLevelMan(TypeMan.arrowman, 0, newPos, true);
            MySceneManager.AddMoney(-arrowmanCount);
            arrowmanCount += Mathf.CeilToInt(arrowmanCount * coefficient / 100);
            MySceneManager.SetArrowmanCount(arrowmanCount);
        }
        UpdateUI();
    }

    public void AddNewMan(bool isMan, int level)
    {
        Vector3 newPos = Vector3.zero;
        for (int i = 1; i < gridSize.y + 1; i++)
        {
            for (int o = 1; o < gridSize.x + 1; o++)
            {
                Vector2 num = new Vector2(o, i);
                Transform tar = CheckTarget(GetCellByNumber(num));
                if (tar == null)
                    newPos = GetCellByNumber(num);
            }
        }

        if (newPos == Vector3.zero)
        {
            print("No clear cell");
            return;
        }

        if (isMan)
        {
            if (MySceneManager.GetMoney() < manCount)
                return;
            InstantiateLevelMan(TypeMan.man, level, newPos, true);
            MySceneManager.AddMoney(-manCount);
            manCount += Mathf.CeilToInt(manCount * coefficient / 100);
            MySceneManager.SetManCount(manCount);
        }
        else
        {
            if (MySceneManager.GetMoney() < arrowmanCount)
                return;
            InstantiateLevelMan(TypeMan.arrowman, level, newPos, true);
            MySceneManager.AddMoney(-arrowmanCount);
            arrowmanCount += Mathf.CeilToInt(arrowmanCount * coefficient / 100);
            MySceneManager.SetArrowmanCount(arrowmanCount);
        }
        UpdateUI();
        
    }

    void RemoveGameObject(GameObject go)
    {
        Destroy(go);
    }

    public void UpdateUI()
    {
        manCount = MySceneManager.GetManCount();
        arrowmanCount = MySceneManager.GetArrowmanCount();
        int m = MySceneManager.GetMoney();
        coinText.text = Convert(m);
        manCoinText.text = Convert(manCount);
        arrowmanCoinText.text = Convert(arrowmanCount);
        if (manCount > MySceneManager.GetMoney())
            freeAddManButton.SetActive(true);
        //Invoke("EnabledFreeMan", .8f);
        else
            freeAddManButton.SetActive(false);
        if (arrowmanCount > MySceneManager.GetMoney())
            freeAddArrowmanButton.SetActive(true);
        //Invoke("EnabledFreeArrowman", .8f);
        else
            freeAddArrowmanButton.SetActive(false);
    }
    /*
    void EnabledFreeMan()
    {
        freeAddManButton.SetActive(true);        
    }
    void EnabledFreeArrowman()
    {
        freeAddArrowmanButton.SetActive(true);
    }
    */
    public static string Convert(int count)
    {
        char[] allLetters = new char[] { ' ', 'K', 'M', 'B', 'T', 'Q', 'S', 'O', 'N', 'D', 'U', 'F', 'A', 'C', 'G', 'L', 'P', 'R', 'X', 'Y', 'Z' };
        string c = count.ToString();
        int i = 0;
        for (; c.Length > 3; i++)
            c = c.Remove(c.Length - 3);
        return (c + allLetters[i]);
    }


}
