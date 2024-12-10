using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.Experimental.GlobalIllumination;

public class MoveRuller : InteractableObject
{
    [SerializeField] private SlotItem _keyItemSource;
    [SerializeField] private List<GameObject> _rullers;
    private Quaternion[] _rullersInitialRotations;
    private float[] _rullersCurrentAngles; // changed a bit every frame to animate the angle


    private PadLockEmissionColor _pLockColor;
    private PlayerInventory _playerInventory;
    private PlayerInteractController _playerInteractController;
    private Animation _openLockAnimation;
    private JournalUI _journalUI;
    InteractionUIController interactionUIController; //INTERACTIONUI for our hint message

    [Header("Cameras")] public GameObject mainCam;
    public GameObject lockCam;
    public Light lockLight;

    private int _scroolRuller = 0;
    private int _selectedRullerIndex = 0;
    private int[] _selectedNumbers = { 0, 0, 0, 0 };

    private bool _isActveEmission = false;
    private bool _isOpened = false;

    public bool IsSolved => _selectedNumbers.SequenceEqual(CorrectPassword);
    private bool _keyGiven = false;

    public int[] CorrectPassword = { 0, 0, 0, 0 };

    protected override void Start()
    {
        base.Start();
        _playerInventory = FindObjectOfType<PlayerInventory>();
        _playerInteractController = FindObjectOfType<PlayerInteractController>();
        _journalUI = FindObjectOfType<JournalUI>();
        _rullersInitialRotations = new Quaternion[_rullers.Count];
        for (int i = 0; i < _rullers.Count; i++)
        {
            _rullersInitialRotations[i] = _rullers[i].transform.localRotation;
        }

        _rullersCurrentAngles = new float[_rullers.Count]; // set to 0 by default as we have all rullers set to 0 
    }

    protected override void Awake()
    {
        base.Awake();
        _pLockColor = FindObjectOfType<PadLockEmissionColor>();
        _openLockAnimation = GetComponent<Animation>();

        if (CorrectPassword.SequenceEqual(new int[] { 0, 0, 0, 0 }))
        {
            Debug.LogWarning("Warning: The correct password is set to { 0, 0, 0, 0 }");
        }
    }

    protected override void Update()
    {
        base.Update();
        if (_isOpened)
        {
            if (IsSolved)
            {
                if (!_keyGiven)
                {
                    Invoke("SetCameraAndDestroy", 1.05f);
                    _playerInventory.AddItem(_keyItemSource.Copy());
                    _openLockAnimation.Play();
                    for (int i = 0; i < _rullers.Count; i++)
                    {
                        _rullers[i].GetComponent<PadLockEmissionColor>()._isSelect = false;
                        _rullers[i].GetComponent<PadLockEmissionColor>().BlinkingMaterial();
                    }

                    _keyGiven = true;
                }
            }
            else
            {
               
                MoveRulles();
                RotateRullers();
                
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
        _journalUI.canToggle = false;
        lockCam.SetActive(true);
        mainCam.SetActive(false);
        lockLight.gameObject.SetActive(true);
        _playerInteractController.SetUIPauseControls(true);
    }

    public void EscapeCamera()
    {
        _journalUI.canToggle = true;
        lockCam.SetActive(false);
        mainCam.SetActive(true);
        lockLight.gameObject.SetActive(false);
        _playerInteractController.SetUIPauseControls(false);
    }

    public void SetCameraAndDestroy()
    {
        EscapeCamera();
        Destroy(gameObject);

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

        for (int i = 0; i < _selectedNumbers.Length; i++)
        {
            float targetAngle = 360 - anglePerUnit * _selectedNumbers[i];
            float newAngle = _calculateNewAngle(_rullersCurrentAngles[i], targetAngle, Time.deltaTime);
            _rullersCurrentAngles[i] = newAngle;
            Quaternion rotQNew = Quaternion.Euler(newAngle, 0, 0);
            Quaternion rotQInitial = _rullersInitialRotations[i];
            _rullers[i].transform.localRotation = rotQInitial * rotQNew;
        }
    }

    private static float _calculateNewAngle(float currentAngle, float targetAngle, float deltaTime)
    {
        // 1) normalize both to [0, 360]
        currentAngle = (currentAngle % 360 + 360) % 360;
        targetAngle = (targetAngle % 360 + 360) % 360;
        float increase;
        if (currentAngle > targetAngle)
        {
            if (currentAngle - targetAngle >= 180)
            {
                increase = 1;
            }
            else
            {
                increase = -1;
            }
        }
        else
        {
            if (targetAngle - currentAngle >= 180)
            {
                increase = -1;
            }
            else
            {
                increase = 1;
            }
        }

        float absoluteDifference = Mathf.Abs(currentAngle - targetAngle);
        float animationSpeed = 180; // degrees per second
        if (absoluteDifference < 5)
        {
            animationSpeed = animationSpeed / 10f * absoluteDifference;
        }

        return currentAngle + increase * deltaTime * animationSpeed;
    }

    public override List<InteractionOptionInstance> GetAvailabeleOptions()
    {
        return new List<InteractionOptionInstance>
            { new InteractionOptionInstance(InteractionOption.OPEN, "Enter code") };
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

    public override bool CanBeSelected()
    {
        return !_isOpened;
    }
}