using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricalSwitch : MonoBehaviour
{
    public enum State
    {
        Back, Prepare, SwitchedOn, Excute
    }
    [SerializeField] private Transform transfSwitch, endPoint;
    [SerializeField] private float timeAutoSwitch;
    [SerializeField] private Rigidbody2D rgbodySwitch;

    private BoxManager boxManager;
    private State curState;
    private float curTimeAuto;
    private Vector3 vecOriginSwitch;
    // Start is called before the first frame update
    void Start()
    {
        vecOriginSwitch = transfSwitch.position;
        //SetState(State.SwitchedOn);
        transfSwitch.position = endPoint.position;
        curState = State.Excute;
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void FixedUpdate()
    {
        if (curState == State.Excute)
        {
            return;
        }
        if (curState == State.Prepare)
        {
            //Check if player push switch to this GO's position
            if (transfSwitch.position.y >= transform.position.y)
            {
                //auto
                SetState(State.SwitchedOn);
            }
        }
        if (curState == State.SwitchedOn)
        {
            SwitchedOn();
        }
        else if (curState == State.Back)
        {
            Back();
        }
    }
    private void Back()
    {
        rgbodySwitch.bodyType = RigidbodyType2D.Kinematic;
        curTimeAuto += Time.fixedDeltaTime;
        rgbodySwitch.MovePosition(Vector2.Lerp(endPoint.position, vecOriginSwitch, curTimeAuto / timeAutoSwitch));
        if (curTimeAuto >= timeAutoSwitch)
        {
            curTimeAuto = 0;
            SetState(State.Prepare);
        }
    }
    private void Prepare()
    {
        rgbodySwitch.bodyType = RigidbodyType2D.Dynamic;
        rgbodySwitch.gravityScale = 0;
        rgbodySwitch.drag = 1000;
        rgbodySwitch.mass = 5;
    }
    private void SwitchedOn()
    {
        rgbodySwitch.bodyType = RigidbodyType2D.Kinematic;
        curTimeAuto += Time.fixedDeltaTime;
        rgbodySwitch.MovePosition(Vector2.Lerp(transform.position, endPoint.position, curTimeAuto / timeAutoSwitch));
        if (curTimeAuto >= timeAutoSwitch)
        {
            curTimeAuto = 0;
            SetState(State.Excute);
        }
    }
    private void Execute()
    {
        rgbodySwitch.velocity = Vector2.zero;
        boxManager.DestroyAllBox();
    }
    public void SetState(State state)
    {
        curState = state;
        switch (state)
        {
            case State.Back:
                break;
            case State.Prepare:
                Prepare();
                break;
            case State.SwitchedOn:
                break;
            case State.Excute:
                Execute();
                break;
        }
    }
    public void SetUp(BoxManager boxM)
    {
        boxManager = boxM;
        SetState(State.Back);
    }
}