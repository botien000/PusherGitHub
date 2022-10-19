using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BoxManager : MonoBehaviour
{
    private Bin bin;
    public Row[] rows;
    private int index, indexOfIndex;
    private List<GarbageBox> listGarbagesBoxDes;
    private ElectricalSwitch electricalSwitch;
    public GarbageBox grB;
    private bool matching;

    private void Awake()
    {
        listGarbagesBoxDes = new List<GarbageBox>();
    }
    // Start is called before the first frame update
    void Start()
    {
        bin = FindObjectOfType<Bin>();
        electricalSwitch = FindObjectOfType<ElectricalSwitch>();
    }

    // Update is called once per frame
    void Update()
    {
    }
    //public void SetBoxesMatch()
    //{
    //    bool existingPlayer = false;
    //    foreach (var gbBox in listGarbagesBoxDes)
    //    {
    //        if (gbBox.Player != null)
    //        {
    //            existingPlayer = true;
    //            break;
    //        }
    //    }
    //    //check player in box
    //    if (!existingPlayer)
    //    {
    //        DestroyAllBox();
    //    }
    //}
    public void GetBox(GarbageBox obj)
    {
        grB = obj;
        for (int i = 0; i < rows.Length; i++)
        {
            indexOfIndex = Array.FindIndex(rows[i].GbBox, indexObj => indexObj == obj);
            if (indexOfIndex != -1)
            {
                index = i;
                break;
            }
        }
        HandleBoxMatch();
    }
    private void HandleBoxMatch()
    {
        //horizontial situlation
        List<GarbageBox> garbagesB = GetHorizontial();
        //check matches more than or equal 2
        if (garbagesB.Count >= 2) 
        {
            foreach (var garbageB in garbagesB)
            {
                garbageB.IsChosen = true;
                listGarbagesBoxDes.Add(garbageB);
            }
        }
        //vertical situlation
        garbagesB = GetVertical();
        //check matches more than or equal 2
        if (garbagesB.Count >= 2)
        {
            foreach (var garbageB in garbagesB)
            {
                garbageB.IsChosen = true;
                listGarbagesBoxDes.Add(garbageB);
            }
        }
        //check match exist
        if (listGarbagesBoxDes.Count > 1)
        {
            //align collied box isChosen = true 
            grB.IsChosen = true;
            listGarbagesBoxDes.Add(grB);
            DestroyAllBox();
        }
    }

    private List<GarbageBox> GetHorizontial()
    {
        List<GarbageBox> listHorizontial = new List<GarbageBox>();
        //check left
        if (indexOfIndex != 0)
        {
            for (int i = indexOfIndex - 1; i >= 0; i--)
            {
                if (rows[index].GbBox[indexOfIndex].TypeOfGarbage == rows[index].GbBox[i].TypeOfGarbage
                    && rows[index].GbBox[indexOfIndex].GarbageTarget != null && rows[index].GbBox[i].GarbageTarget != null
                    )
                {
                    listHorizontial.Add(rows[index].GbBox[i]);
                }
                else
                {
                    break;
                }
            }
        }
        //check right
        if (indexOfIndex != rows[index].GbBox.Length - 1)
        {
            for (int i = indexOfIndex + 1; i < rows[index].GbBox.Length; i++)
            {
                if (rows[index].GbBox[indexOfIndex].TypeOfGarbage == rows[index].GbBox[i].TypeOfGarbage
                   && rows[index].GbBox[indexOfIndex].GarbageTarget != null && rows[index].GbBox[i].GarbageTarget != null
                   )
                {
                    listHorizontial.Add(rows[index].GbBox[i]);
                }
                else
                {
                    break;
                }
            }
        }
        return listHorizontial;
    }
    private List<GarbageBox> GetVertical()
    {
        List<GarbageBox> listVertical = new List<GarbageBox>();
        //check top 
        if (index != 0)
        {
            for (int i = index - 1; i >= 0; i--)
            {
                if (rows[index].GbBox[indexOfIndex].TypeOfGarbage == rows[i].GbBox[indexOfIndex].TypeOfGarbage && 
                    rows[index].GbBox[indexOfIndex].GarbageTarget != null && rows[i].GbBox[indexOfIndex].GarbageTarget != null)
                {
                    listVertical.Add(rows[i].GbBox[indexOfIndex]);
                }
                else
                {
                    break;
                }
            }
        }
        //check bottom
        if (index != rows.Length - 1)
        {
            for (int i = index + 1; i < rows.Length; i++)
            {
                if (rows[index].GbBox[indexOfIndex].TypeOfGarbage == rows[i].GbBox[indexOfIndex].TypeOfGarbage && 
                    rows[index].GbBox[indexOfIndex].GarbageTarget != null && rows[i].GbBox[indexOfIndex].GarbageTarget != null)
                {
                    listVertical.Add(rows[i].GbBox[indexOfIndex]);
                }
                else
                {
                    break;
                }
            }
        }
        return listVertical;
    }

    public void DestroyAllBox()
    {
        if(listGarbagesBoxDes.Count == 0)
        {
            Debug.LogError("It seems to have a bug.Find and fix it");
            return;
        }
        List<Garbage> garbages = new List<Garbage>();
        Garbage garbage = null;
        foreach (var garbageBox in listGarbagesBoxDes)
        {
            garbage = garbageBox.GarbageTarget;
            garbages.Add(garbage);
            garbageBox.Flip(garbage.typeOfGarbage);
        }
        GameManager.Instance.InitIconParent();
        bin.GetGarbageMatch(garbages, garbage.typeOfGarbage);
        listGarbagesBoxDes.Clear();
    }

    [System.Serializable]
    public class Row
    {
        public GarbageBox[] GbBox;
    }
}
