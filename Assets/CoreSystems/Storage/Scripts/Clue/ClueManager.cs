using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClueManager : MonoBehaviour
{
    public static ClueManager Instance { get; private set; }
    
    [SerializeField] private GameObject enlargedCluePanel;
    [SerializeField] private GameObject clueInspectionPanel;
    
    private ClueData currentClue;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetCurrentClue(ClueData data)
    {
        currentClue = data;
    }

    public void InspectCurrentClue()
    {
        if (currentClue != null && enlargedCluePanel != null)
        {
            enlargedCluePanel.GetComponent<Image>().sprite = currentClue.sprite;
            clueInspectionPanel.SetActive(true);
        }
    }

    public void CloseInspection()
    {
        if (clueInspectionPanel != null)
        {
            clueInspectionPanel.SetActive(false);
        }
    }
}
