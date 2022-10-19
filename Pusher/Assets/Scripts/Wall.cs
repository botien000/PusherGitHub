using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    private Collider2D[] collider2Ds;
    // Start is called before the first frame update
    void Start()
    {
        collider2Ds = GetComponentsInChildren<Collider2D>();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetTrigger(Collider2D collider2D)
    {
        if(collider2D != null)
        {
            foreach (var item in collider2Ds)
            {
                if(item == collider2D)
                {
                    item.isTrigger = true;
                    break;
                }
            }
        }
        else
        {
            foreach (var item in collider2Ds)
            {
                item.isTrigger = false;
            }
        }
    }
}
