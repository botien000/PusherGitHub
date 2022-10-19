using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;

public class Hole : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public async void GetGarbage(List<Garbage> garbages, Transform parent)
    {
        await Task.Delay(1000);
        foreach (var garbage in garbages)
        {
            try
            {
                garbage.transform.position = transform.position;
                garbage.transform.SetParent(parent);
                garbage.gameObject.SetActive(true);
                garbage.Init(true);
                await Task.Delay(300);
            }
            catch (Exception e) { Debug.Log(e); }
        }
    }
}
