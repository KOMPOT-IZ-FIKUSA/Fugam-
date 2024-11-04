
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using System.Linq;

public class MoveRuller : InteractableObject
{
    [SerializeField] private SlotItem _keyItemSource;
    [SerializeField] private List<GameObject> _rullers;
    private Quaternion[] _rullersInitialRotations;


    private PadLockEmissionColor _pLockColor;
    private PlayerInventory _playerInventory;
    private Animation _openLockAnimation;

    [Header("Cameras")]
    public GameObject mainCam;
    public GameObject lockCam;

    private int _scroolRuller = 0;
    private int _selectedRullerIndex = 0;
    private int[] _selectedNumbers = { 0, 0, 0, 0 };

    private bool _isActveEmission = false;
    private bool _isOpened = false;

    public bool IsSolved => _selectedNumbers.SequenceEqual(_selectedNumbers);

    public int[] CorrectPassword = { 0, 0, 0, 0 };

    protected override void Start()
    {
        base.Start();
        _playerInventory = FindObjectOfType<PlayerInventory>();

        _rullersInitialRotations = new Quaternion[_rullers.Count];
        for (int i = 0; i < _rullers.Count; i++)
        {
            _rullersInitialRotations[i] = _rullers[i].transform.localRotation;
        }

    }
    protected override void Awake()
    {
        base.Awake();
        _pLockColor = FindObjectOfType<PadLockEmissionColor>();
        _openLockAnimation = GetComponent<Animation>();
    }
    protected override void Update()
    {
        base.Update();
        if (_isOpened)
        {
            MoveRulles();
            RotateRullers();

            if (IsSolved)
            {
                return;
                Invoke("PlayerCamera", 1.05f);
                _playerInventory.AddItem(_keyItemSource.Copy());
                _openLockAnimation.Play();
                for (int i = 0; i < _rullers.Count; i++)
                {
                    _rullers[i].GetComponent<PadLockEmissionColor>()._isSelect = false;
                    _rullers[i].GetComponent<PadLockEmissionColor>().BlinkingMaterial();
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape) && Application.isPlaying)
        {
            EscapeCamera();
            _isOpened = false;
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
            _selectedRullerIndex++;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            _isActveEmission = true;
            _selectedRullerIndex--;
        }
        _selectedRullerIndex = (_selectedRullerIndex + _rullers.Count) % _rullers.Count;

        if (_isActveEmission)
        {
            for (int i = 0; i < _rullers.Count; i++)
            {
                if (_selectedRullerIndex == i)
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
        const int anglePerUnit = 360 / 10;
        int increase = 0;
        if (Input.GetKeyDown(KeyCode.W))
        {
            _isActveEmission = true;
            increase = 1;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            _isActveEmission = true;
            increase = -1;
        }
        _selectedNumbers[_selectedRullerIndex] = (_selectedNumbers[_selectedRullerIndex] + increase + 10) % 10;
        float angleX = anglePerUnit * _selectedNumbers[_selectedRullerIndex];
        Quaternion rotQNew = Quaternion.Euler(angleX, 0, 0);
        Quaternion rotQInitial = _rullersInitialRotations[_selectedRullerIndex];
        print(rotQInitial + " " + rotQNew);
        _rullers[_selectedRullerIndex].transform.localRotation = rotQInitial * rotQNew;
    }

    public override List<InteractionOptionInstance> GetAvailabeleOptions()
    {
        return new List<InteractionOptionInstance> { new InteractionOptionInstance(InteractionOption.OPEN, "Enter code") };
    }

    public override void Interact(InteractionOptionInstance option)
    {
        base.Interact(option);
        if (option.option == InteractionOption.OPEN)
        {
            _isOpened = true;
            LockCameras();
        }
    }
}
