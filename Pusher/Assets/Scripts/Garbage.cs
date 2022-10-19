using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Garbage : MonoBehaviour
{
    enum State
    {
        idle, fall
    }

    private State curState;
    public float speedMove;
    public int typeOfGarbage;

    private Collider2D colider;
    private Rigidbody2D rgbody;
    private bool isTargeted;
    private GarbageBox garbageBox;
    private List<GarbageBox> gbBoxes;

    private void Awake()
    {
        gbBoxes = new List<GarbageBox>();
        rgbody = GetComponent<Rigidbody2D>();
        colider = GetComponent<Collider2D>();
    }
    public void Init(bool stateFall)
    {
        isTargeted = false;
        garbageBox = null;
        gbBoxes.Clear();
        if (stateFall)
        {
            transform.rotation = Quaternion.identity;
            curState = State.fall;
            rgbody.drag = 5;
            rgbody.gravityScale = 1;
            colider.isTrigger = true;
        }
        else
        {
            rgbody.drag = 100;
            rgbody.gravityScale = 0;
            curState = State.idle;
            colider.isTrigger = false;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        curState = State.idle;
    }

    // Update is called once per frame
    void Update()
    {
        if(gbBoxes.Count > 1)
        {
            int i = 0;
            while(i < gbBoxes.Count)
            {
                if (!gbBoxes[i].gameObject.activeInHierarchy)
                {
                    gbBoxes.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }
        }
    }
    private void FixedUpdate()
    {
        if (isTargeted) 
        {
            if (transform.position == garbageBox.transform.position)
                return;
            rgbody.MovePosition(Vector2.MoveTowards(transform.position, garbageBox.transform.position, Time.fixedDeltaTime * speedMove));
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("GarbageBox"))
        {
            GarbageBox garbageBox = collision.GetComponent<GarbageBox>();
            garbageBox.AddGarbage(this);
            gbBoxes.Add(garbageBox);
            //CheckTotalCollide();
        }
        else if (collision.CompareTag("Bin") && curState == State.fall)
        {
            GameManager.Instance.ReduceCurGarbage(1);
            gameObject.SetActive(false);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("GarbageBox"))
        {
            GarbageBox garbageBox = collision.GetComponent<GarbageBox>();
            garbageBox.RemoveGarbage(this);
            gbBoxes.Remove(garbageBox);
            //CheckTotalCollide();
        }
    }

    public bool TakeGarbage(GarbageBox garbageBox)
    {
        if (gbBoxes.Count != 1)
            return false;
        else
        {
            if (gbBoxes[0] == garbageBox)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    //private void CheckTotalCollide()
    //{
    //    if(gbBoxes.Count > 1)
    //    {
    //        foreach (var box in gbBoxes)
    //        {
    //            box.RemoveGarbage(this);
    //        }
    //    }
    //    else if (gbBoxes.Count == 1)
    //    {
    //        gbBoxes[0].AddGarbage(this);
    //    }
    //}
    public void SetTarget(bool target,GarbageBox grBox)
    {
        isTargeted = target;
        garbageBox = grBox;
    }

}
