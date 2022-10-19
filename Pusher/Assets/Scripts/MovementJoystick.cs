using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MovementJoystick : MonoBehaviour
{
    public GameObject joystick;
    public GameObject joystickBG;
    public Transform rangeRadius;
    [HideInInspector]
    public Vector2 joystickVec;
    [HideInInspector]
    public float angle;
    private Vector2 joystickTouchPos;
    private Vector2 joystickOriginalPos;
    private float joystickRadius;
    private void OnEnable()
    {
        if(joystickOriginalPos != Vector2.zero)
        {
            joystickBG.transform.position = joystickOriginalPos;
            joystick.transform.position = joystickOriginalPos;
        }
    }
    void OnDisable()
    {
        //clear
        joystickVec = Vector2.zero;
    }
    // Start is called before the first frame update
    void Start()
    {
        joystickOriginalPos = joystickBG.transform.position;
        //joystickRadius = Camera.main.ScreenToWorldPoint(joystickBG.GetComponent<RectTransform>().sizeDelta).y / 2;
    }

    public void PointerDown()
    {
        joystick.transform.position = (Vector2) Camera.main.ScreenToWorldPoint(Input.mousePosition);
        joystickBG.transform.position = (Vector2) Camera.main.ScreenToWorldPoint(Input.mousePosition);
        joystickTouchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        joystickRadius = Vector2.Distance(joystickTouchPos, rangeRadius.position);
    }
    public void Drag(BaseEventData baseEventData)
    {
        PointerEventData pointerEventData = baseEventData as PointerEventData;
        Vector2 dragPos = Camera.main.ScreenToWorldPoint(pointerEventData.position);
        joystickVec = (dragPos - joystickTouchPos).normalized;
        //Debug.Log(joystickVec);
        angle = Mathf.Atan2(joystickVec.y, joystickVec.x) * Mathf.Rad2Deg;
        float joystickDist = Vector2.Distance(dragPos, joystickTouchPos);
        if (joystickDist < joystickRadius)
        {
            joystick.transform.position = joystickTouchPos + joystickVec * joystickDist;
        }
        else
        {
            joystick.transform.position = joystickTouchPos + joystickVec * joystickRadius;
        }
    }
    public void PointerUp()
    {
        joystickVec = Vector2.zero;
        joystick.transform.position = joystickOriginalPos;
        joystickBG.transform.position = joystickOriginalPos;
    }
}
