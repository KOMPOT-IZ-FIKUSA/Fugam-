
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MoveRuller : MonoBehaviour
{
    PadLockPassword _lockPassword;
    PadLockEmissionColor _pLockColor;
    [SerializeField] public SlotItem keyItemSource;
    PlayerInventory playerInventory;
    

    [Header("Cameras")]
    public GameObject mainCam;
    public GameObject lockCam;

    private BoxCollider _boxCollider;
    [HideInInspector]
    public List <GameObject> _rullers = new List<GameObject>();
    private int _scroolRuller = 0;
    private int _changeRuller = 0;
    [HideInInspector]
    public int[] _numberArray = {0,0,0,0};

    private int _numberRuller = 0;

    

    private bool _isActveEmission = false;
    private bool isClicked = false;


    private void OnMouseDown()
    {
        isClicked = true;
    }
    private void Start()
    {
        _boxCollider = GetComponent<BoxCollider>();
        playerInventory = FindObjectOfType<PlayerInventory>();
        

    }
    void Awake()
    {
        
        _lockPassword = FindObjectOfType<PadLockPassword>();
        _pLockColor = FindObjectOfType<PadLockEmissionColor>();

        _rullers.Add(GameObject.Find("Ruller1"));
        _rullers.Add(GameObject.Find("Ruller2"));
        _rullers.Add(GameObject.Find("Ruller3"));
        _rullers.Add(GameObject.Find("Ruller4"));

        foreach (GameObject r in _rullers)
        {
            r.transform.Rotate(-144, 0, 0, Space.Self);
        }
    }
    void Update()
    {
        if (isClicked)
        {
            
            MoveRulles();
            RotateRullers();
            _lockPassword.Password();
            Debug.Log("LockCam Active");
            LockCameras();

            if (_lockPassword.passSolved == true)
            {
                Invoke("PlayerCamera", 1.05f);
                playerInventory.AddItem(keyItemSource.Copy());
                Debug.Log("Pass Solved");
            }
            
        }
        if (Input.GetKeyDown(KeyCode.Escape) && Application.isPlaying)
        {
            Debug.Log("Back to player Cam!");
            EscapeCamera();
            isClicked = false;
            
        }
        
    }
    public void LockCameras()
    {
        lockCam.SetActive(true);
        mainCam.SetActive(false);
    }
    public void EscapeCamera()
    {
        lockCam.SetActive(false);
        mainCam.SetActive(true);
    }
    public void PlayerCamera()
    {
        lockCam.SetActive(false);
        Destroy(lockCam);
        mainCam.SetActive(true);
        Destroy(this.gameObject);
    }
    void MoveRulles()
    {
        if (Input.GetKeyDown(KeyCode.D)) 
        {
            _isActveEmission = true;
            _changeRuller ++;
            _numberRuller += 1;

            if (_numberRuller > 3)
            {
                _numberRuller = 0;
            }
        }
        if (Input.GetKeyDown(KeyCode.A)) 
        {
            _isActveEmission = true;
            _changeRuller --;
            _numberRuller -= 1;

            if (_numberRuller < 0)
            {
                _numberRuller = 3;
            }
        }
        _changeRuller = (_changeRuller + _rullers.Count) % _rullers.Count;


        for (int i = 0; i < _rullers.Count; i++)
        {
            if (_isActveEmission)
            {
                if (_changeRuller == i)
                {

                    _rullers[i].GetComponent<PadLockEmissionColor>()._isSelect = true;
                    _rullers[i].GetComponent<PadLockEmissionColor>().BlinkingMaterial();
                }
                else
                {
                    _rullers[i].GetComponent<PadLockEmissionColor>()._isSelect = false;
                    _rullers[i].GetComponent<PadLockEmissionColor>().BlinkingMaterial();
                }
            }
        }

    }

    void RotateRullers()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            _isActveEmission = true;
            _scroolRuller = 36;
            _rullers[_changeRuller].transform.Rotate(-_scroolRuller, 0, 0, Space.Self);

            _numberArray[_changeRuller] += 1;

            if (_numberArray[_changeRuller] > 9)
            {
                _numberArray[_changeRuller] = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            _isActveEmission = true;
            _scroolRuller = 36;
            _rullers[_changeRuller].transform.Rotate(_scroolRuller, 0, 0, Space.Self);

            _numberArray[_changeRuller] -= 1;

            if (_numberArray[_changeRuller] < 0)
            {
                _numberArray[_changeRuller] = 9;
            }
        }
    }
}
