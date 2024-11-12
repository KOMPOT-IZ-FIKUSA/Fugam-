using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ClueData", menuName = "ClueData", order = 1)]
public class ClueData : ScriptableObject
{
    public string clueName;
    public string clueDescription;
    public string clueMessage;
    public Sprite sprite;
}
