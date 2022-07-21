using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Lineup : MonoBehaviour
{
    public GameObject[] allMan;
    public GameObject[] allArrowman;
    public ParticleSystem mergeParticle;
    public AudioClip mergeAudioClip;
    private AudioSource _audioSource;
    public Material blueMaterial;
    public Material redMaterial;
    public GameObject hpBarBlue;
    public GameObject hpBarRed;

    [Space]
    public int coefficient = 25;
    public int money;
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
        money = MySceneManager.GetMoney();
        UpdateUI();
        _audioSource = FindObjectOfType<AudioSource>();
    }

    public bool merge = true;
     int lvl = 0;
    public List<GameObject> merges = new List<GameObject>();

    void Update()
    {
        if (merge)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _startCell = CheckCell(RaycastPosition(Input.mousePosition));
                _target = CheckTarget(_startCell);
                if (_target == null)
                    return;
                merges.Add(_target.root.gameObject);
                lvl++;
                Outline outline = _target.GetComponentInChildren<Outline>();
                outline.OutlineColor = Color.green;
                outline.OutlineWidth = 8;
                _target.localScale = _target.localScale + _target.localScale * .1f;

            }
            if (Input.GetMouseButtonDown(1))
            {
                InstantiateLevelMan(TypeMan.man, merges.Count - 1, merges[merges.Count-1].transform.position, true);
                foreach (GameObject go in merges)
                    Destroy(go);
                lvl = 0;
                merges = new List<GameObject>();
            }
            return;
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                _startCell = CheckCell(RaycastPosition(Input.mousePosition));
                _target = CheckTarget(_startCell);
                if (_target == null)
                    return;
                _target.gameObject.layer = 0;
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
            }
        }
    }

    Vector3 RaycastPosition (Vector3 inputPosition)
    {
        Vector3 newPos = Vector3.zero;
        Ray ray = _myCamera.ScreenPointToRay(inputPosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100000, layerGrounded))
        {
            newPos = hit.point;
            newPos.y = 0;
        }
        return newPos;
    }

    Vector3 CheckCell (Vector3 inputPosition)
    {
        Vector3 newPos = new Vector3((Mathf.RoundToInt((inputPosition.x + offcetPosition.x) / cellSize.x))*cellSize.x, 0, (Mathf.RoundToInt((inputPosition.z + offcetPosition.y) / cellSize.y))* cellSize.y);
        
        return newPos;
    }

    Vector3 ClampCell(Vector3 newPos)
    {
        newPos = new Vector3(Mathf.Clamp(newPos.x, offcetPosition.x + cellSize.x, gridSize.x * cellSize.x), newPos.y, Mathf.Clamp(newPos.z, offcetPosition.y + cellSize.y, gridSize.y * cellSize.y));
        return newPos;
    }

    Vector3 GetCellByNumber (Vector2 num)
    {
        return new Vector3(cellSize.x*num.x + offcetPosition.x, 0, cellSize.y * num.y + offcetPosition.y);
    }

    Transform  CheckTarget (Vector3 pos)
    {
        Transform _trg = null;
        Collider[] allColl = Physics.OverlapSphere(pos, cellSize.x/2, layerPlayer);
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

    int getLevel (GameObject go)
    {
        int level = 0;
        level = go.GetComponentInChildren<Indicators>().level;
        return level;
    }

    TypeMan GetTypeMan (GameObject go)
    {
        return go.GetComponentInChildren<Indicators>().typeMan;
    }

    void Merger (Transform trInCell, Transform tr)
    {
        if (GetTypeMan(trInCell.gameObject) != GetTypeMan(tr.gameObject))
        {
            ReturnStartCell();
            return;
        }
        int a = getLevel(trInCell.gameObject);
        int b = getLevel(tr.gameObject);
        if (a == b && a < 7)
        {
            RemoveGameObject(trInCell.root.gameObject);
            RemoveGameObject(tr.root.gameObject);            
            InstantiateLevelMan(GetTypeMan(trInCell.gameObject), a+1, trInCell.position, true);            
        }
        else
            ReturnStartCell();
    }

    void ReturnStartCell()
    {
        _target.position = _startCell;
    }

    public void InstantiateLevelMan (TypeMan typeMan, int currentLevel, Vector3 newPos, bool isPlayer)
    {
        GameObject go = null;
        if (typeMan == TypeMan.man)
        {
            go = Instantiate(allMan[currentLevel], newPos, Quaternion.identity);
            if(!go.activeSelf)
                go.SetActive(true);
        }
        else
        {
            go = Instantiate(allArrowman[currentLevel], newPos, Quaternion.identity);
            if (!go.activeSelf)
                go.SetActive(true);
        }
        if (isPlayer)
        {
            go.GetComponentInChildren<Indicators>().gameObject.tag = "Player";
            go.GetComponentInChildren<SkinnedMeshRenderer>().material = blueMaterial;
            go.GetComponentInChildren<DamageBase>().hpBar = Instantiate(hpBarBlue);

            if (mergeEvent != null)
                mergeEvent.Invoke();
        }
        else
        {
            go.GetComponentInChildren<Indicators>().gameObject.tag = "Untagged";
            go.transform.rotation = Quaternion.Euler(0, 180, 0);
            go.GetComponentInChildren<SkinnedMeshRenderer>().material = redMaterial;
            go.GetComponentInChildren<DamageBase>().hpBar = Instantiate(hpBarRed);
        }
        if (_audioSource == null)
            return;
        mergeParticle.transform.position = newPos;
        mergeParticle.Play();
        _audioSource.PlayOneShot(mergeAudioClip);
    }

    public void AddNewMan(bool isMan)
    {
        Vector3 newPos = Vector3.zero;
        for (int i = 1; i < gridSize.y+1; i++)
        {
            for (int o = 1; o < gridSize.x+1; o++)
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
            if (money < manCount)
                return;
            InstantiateLevelMan(TypeMan.man, 0, newPos, true);
            money -= manCount;
            manCount += Mathf.CeilToInt(manCount*coefficient/100);
            MySceneManager.SetManCount(manCount);
        }
        else
        {
            if (money < arrowmanCount)
                return;
            InstantiateLevelMan(TypeMan.arrowman, 0, newPos, true);
            money -= arrowmanCount;
            arrowmanCount += Mathf.CeilToInt(arrowmanCount * coefficient / 100);
            MySceneManager.SetArrowmanCount(arrowmanCount);
        }
        UpdateUI();
    }

    void RemoveGameObject(GameObject go)
    {
        Destroy(go);
    }    

    void UpdateUI ()
    {
        manCount = MySceneManager.GetManCount();
        arrowmanCount = MySceneManager.GetArrowmanCount();
        coinText.text = money.ToString();
        MySceneManager.SetMoney(money);
        manCoinText.text = manCount.ToString();
        arrowmanCoinText.text = arrowmanCount.ToString();
    }

    


}
