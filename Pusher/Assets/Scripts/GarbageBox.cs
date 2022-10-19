using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GarbageBox : MonoBehaviour
{

    private Animator animator;
    private List<Garbage> listGarbagesContained;
    private Garbage garbageTarget;
    private Collider2D colider;
    private Player player;
    private int typeOfGarbage;
    private Collider2D[] playerColision;
    private bool isChosen;
    private BoxManager boxManager;

    //use variable this to set GetBox
    private bool onlyOneTime;


    public int TypeOfGarbage { get => typeOfGarbage; set => typeOfGarbage = value; }
    public Garbage GarbageTarget { get => garbageTarget; set => garbageTarget = value; }
    public Player Player { get => player; }
    public bool IsChosen { get => isChosen; set => isChosen = value; }

    private void Awake()
    {
        colider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        boxManager = GetComponentInParent<BoxManager>();
        listGarbagesContained = new List<Garbage>();
    }
    // Start is called before the first frame update
    void Start()
    {
        playerColision = FindObjectOfType<Player>().GetComponentsInChildren<Collider2D>();
        //StartCoroutine("HandleTarget");
    }

    // Update is called once per frame
    void Update()
    {
        if (isChosen)
            return;
        if (listGarbagesContained.Count != 1)
        {
            typeOfGarbage = 4;
            ChangeColor(4);
            onlyOneTime = false;
            if (GarbageTarget != null)
            {
                GarbageTarget.SetTarget(false, null);
                GarbageTarget = null;
            }
        }
        else
        {
            if (!listGarbagesContained[0].TakeGarbage(this))
            {
                typeOfGarbage = 4;
                ChangeColor(4);
                onlyOneTime = false;
                if (GarbageTarget != null)
                {
                    GarbageTarget.SetTarget(false, null);
                    GarbageTarget = null;
                }
                return;
            }
            TypeOfGarbage = listGarbagesContained[0].typeOfGarbage;
            ChangeColor(TypeOfGarbage);
            GarbageTarget = listGarbagesContained[0];
            //player out box
            if (player == null)
            {
                if (!GarbageTarget.GetComponent<Collider2D>().IsTouching(playerColision[0]) &&
                    !GarbageTarget.GetComponent<Collider2D>().IsTouching(playerColision[1]))
                {
                    GarbageTarget.SetTarget(true, this);
                    if (!onlyOneTime)
                        if ((Vector2)garbageTarget.transform.position == (Vector2)transform.position)
                        {
                            onlyOneTime = true;
                            boxManager.GetBox(this);
                        }
                }
            }
            //player in box
            else
            {
                onlyOneTime = false;
                if (GarbageTarget != null)
                {
                    GarbageTarget.SetTarget(false, null);
                    GarbageTarget = null;
                }
            }
        }
    }

    private void ChangeColor(int type)
    {
        animator.SetInteger("TypeG", type);
    }
    public void Flip(int type)
    {
        garbageTarget.GetComponent<Collider2D>().isTrigger = true;
        colider.isTrigger = false;
        animator.SetInteger("TypeG", type);
        animator.SetTrigger("Change");
    }
    public void AddGarbage(Garbage garbage)
    {
        listGarbagesContained.Add(garbage);
    }
    public void RemoveGarbage(Garbage garbage)
    {
        listGarbagesContained.Remove(garbage);
    }
    //IEnumerator HandleTarget()
    //{
    //    while (true)
    //    {

    //        if (listGarbagesContained.Count == 1 && player == null)
    //        {
    //            GarbageTarget = listGarbagesContained[0];
    //            if (!GarbageTarget.GetComponent<Collider2D>().IsTouching(playerColision[0]) && !GarbageTarget.GetComponent<Collider2D>().IsTouching(playerColision[1]))
    //            {
    //                GarbageTarget.SetTarget(true, this);
    //                if (!onlyOneTime)
    //                    if ((Vector2)garbageTarget.transform.position == (Vector2)transform.position)
    //                    {
    //                        onlyOneTime = true;
    //                        GetComponentInParent<BoxManager>().GetBox(this);
    //                    }
    //            }
    //        }
    //        else if (listGarbagesContained.Count != 1  || player != null)
    //        {
    //            onlyOneTime = false;
    //            if (GarbageTarget != null)
    //            {
    //                GarbageTarget.SetTarget(false, null);
    //                GarbageTarget = null;
    //            }
    //        }
    //        yield return null;
    //    }

    //}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = collision.GetComponentInParent<Player>();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (colider.IsTouching(playerColision[0]) || colider.IsTouching(playerColision[1]))
            {
                return;
            }
            player = null;
            if (isChosen)
            {
                //boxManager.SetBoxesMatch();
            }
        }
    }
    public void EventDisableGarbage()
    {
        garbageTarget.gameObject.SetActive(false);
    }
    public void EventDisableObject()
    {
        gameObject.SetActive(false);
        typeOfGarbage = 4;
    }
    private void OnDisable()
    {
        colider.isTrigger = true;
        //StopAllCoroutines();
    }
}
