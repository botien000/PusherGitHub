using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Level", menuName = "ScriptableObject/Create level")]
public class LevelSctb : ScriptableObject
{
    public BoxManager gObjBoxParent; 
    public GarbageManager gObjGarbageParent;
}
