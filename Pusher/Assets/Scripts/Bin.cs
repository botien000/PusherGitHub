using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bin : MonoBehaviour
{
    [SerializeField] private Hole hole_1, hole_2, hole_3;
    [SerializeField] private Transform[] parentGarbage;
    public void GetGarbageMatch(List<Garbage> garbagesMatch, int type)
    {
        switch (type)
        {
            case 1:
                hole_1.GetGarbage(garbagesMatch,parentGarbage[0]);
                break;
            case 2:
                hole_2.GetGarbage(garbagesMatch, parentGarbage[1]);
                break;
            case 3:
                hole_3.GetGarbage(garbagesMatch, parentGarbage[2]);
                break;
        }
    }

}
