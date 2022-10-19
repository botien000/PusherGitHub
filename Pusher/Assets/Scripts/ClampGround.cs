using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClampGround : MonoBehaviour
{
    [SerializeField] private Transform transfPlayer;
    [SerializeField] private MovementJoystick mov;
    [SerializeField] private int divideBy;
    [SerializeField] private float timeMoveGround,timeMoveInGround;
    [SerializeField] private Transform transfLeft, transfRight, transfTop, transfBottom;
    [SerializeField] private Transform transfBoundTop;
    [SerializeField] private Transform transfZoneOutLeft, transfZoneOutRight, transfZoneOutBottom, transfZoneOutTop;

    private int widthScreen, heightScreen;
    Vector3 vecCurGround;
    private Vector3 vecRangeLeft, vecRangeRight, vecRangeTop, vecRangeBottom;
    private void Awake()
    {
    }
    // Start is called before the first frame update
    void Start()
    {
        vecCurGround = transform.position;
        widthScreen = Screen.width;
        heightScreen = Screen.height;
        vecRangeLeft = -transfLeft.position + Camera.main.ScreenToWorldPoint(Vector3.up * heightScreen / 2);
        vecRangeRight = -transfRight.position + Camera.main.ScreenToWorldPoint(new Vector3(widthScreen, heightScreen / 2, 0));
        vecRangeTop = -transfTop.position + transfBoundTop.position;
        vecRangeBottom = -transfBottom.position + Camera.main.ScreenToWorldPoint(Vector3.right * widthScreen / 2);
    }

    // Update is called once per frame
    void Update()
    {
        //left
        if (transfPlayer.position.x <= transfZoneOutLeft.position.x)
        {
            if (mov.joystickVec.x < 0)
            {
                vecCurGround += new Vector3(mov.joystickVec.x * -1 * Time.deltaTime * timeMoveGround, 0, 0);
                //clamp width
                float xx = Mathf.Clamp(vecCurGround.x, vecRangeRight.x, vecRangeLeft.x);
                transform.position = new Vector3(xx, vecCurGround.y, vecCurGround.z);
                vecCurGround = transform.position;
            }
        }

        //right
        else if (transfPlayer.position.x >= transfZoneOutRight.position.x)
        {
            if (mov.joystickVec.x > 0)
            {
                vecCurGround += new Vector3(mov.joystickVec.x * -1 * Time.deltaTime * timeMoveGround, 0, 0);
                //clamp width
                float xx = Mathf.Clamp(vecCurGround.x, vecRangeRight.x, vecRangeLeft.x);
                transform.position = new Vector3(xx, vecCurGround.y, vecCurGround.z);
                vecCurGround = transform.position;
            }
        }
        //middle x
        else if (vecCurGround.x != 0)
        {
            if (transfPlayer.position.x > transfZoneOutLeft.position.x && transfPlayer.position.x < 0)
            {
                vecCurGround -= new Vector3(Time.deltaTime * timeMoveInGround, 0, 0);
                //clamp left
                if (vecCurGround.x <= 0)
                {
                    vecCurGround = new Vector3(0, vecCurGround.y, vecCurGround.z);
                }
                transform.position = vecCurGround;
            }
            else if (transfPlayer.position.x < transfZoneOutRight.position.x && transfPlayer.position.x > 0)
            {
                vecCurGround += new Vector3(Time.deltaTime * timeMoveInGround, 0, 0);
                //clamp right
                if (vecCurGround.x >= 0)
                {
                    vecCurGround = new Vector3(0, vecCurGround.y, vecCurGround.z);
                }
                transform.position = vecCurGround;
            }
        }

        //top
        if (transfPlayer.position.y >= transfZoneOutTop.position.y)
        {
            if (mov.joystickVec.y > 0)
            {
                vecCurGround += new Vector3(0, mov.joystickVec.y * -1 * Time.deltaTime * timeMoveGround, 0);
                //clamp width
                float yy = Mathf.Clamp(vecCurGround.y, vecRangeTop.y, vecRangeBottom.y);
                transform.position = new Vector3(vecCurGround.x, yy, vecCurGround.z);
                vecCurGround = transform.position;
            }
        }
        //bottom
        else if (transfPlayer.position.y <= transfZoneOutBottom.position.y)
        {
            if (mov.joystickVec.y < 0)
            {
                vecCurGround += new Vector3(0, mov.joystickVec.y * -1 * Time.deltaTime * timeMoveGround, 0);
                //clamp width
                float yy = Mathf.Clamp(vecCurGround.y, vecRangeTop.y, vecRangeBottom.y);
                transform.position = new Vector3(vecCurGround.x, yy, vecCurGround.z);
                vecCurGround = transform.position;
            }
        }
        //middle y
        if (vecCurGround.y != 0)
        {
            if (transfPlayer.position.y < transfZoneOutTop.position.y && transfPlayer.position.y > 0)
            {
                vecCurGround += new Vector3(0, Time.deltaTime * timeMoveInGround, 0);
                //clamp top
                if (vecCurGround.y >= 0)
                {
                    vecCurGround = new Vector3(vecCurGround.x, 0, vecCurGround.z);
                }
                transform.position = vecCurGround;
            }
            else if (transfPlayer.position.y > transfZoneOutBottom.position.y && transfPlayer.position.y < 0)
            {
                vecCurGround -= new Vector3(0, Time.deltaTime * timeMoveInGround, 0);
                //clamp bottom
                if (vecCurGround.y <= 0)
                {
                    vecCurGround = new Vector3(vecCurGround.x, 0, vecCurGround.z);
                }
                transform.position = vecCurGround;
            }
        }

        ////middle
        //if (vecCurGround.x == 0 && vecCurGround.y == 0)
        //    return;
        //if (transfPlayer.position.x > transfZoneOutLeft.position.x && transfPlayer.position.x < 0)
        //{
        //    vecCurGround += new Vector3(mov.joystickVec.x * -1 * Time.deltaTime * timeMoveGround, 0, 0);
        //    //clamp left
        //    if (vecCurGround.x >= 0)
        //    {
        //        vecCurGround = new Vector3(0, vecCurGround.y, vecCurGround.z);
        //    }
        //    Debug.Log(vecCurGround + " left ");
        //    transform.position = vecCurGround;
        //}
        //else if (transfPlayer.position.x < transfZoneOutRight.position.x && transfPlayer.position.x > 0)
        //{
        //    vecCurGround += new Vector3(mov.joystickVec.x * -1 * Time.deltaTime * timeMoveGround, 0, 0);
        //    //clamp right
        //    if (vecCurGround.x <= 0)
        //    {
        //        vecCurGround = new Vector3(0, vecCurGround.y, vecCurGround.z);
        //    }
        //    Debug.Log(vecCurGround + " right ");
        //    transform.position = vecCurGround;
        //}
        //if (transfPlayer.position.y < transfZoneOutTop.position.y && transfPlayer.position.y > 0)
        //{
        //    vecCurGround += new Vector3(0, mov.joystickVec.y * -1 * Time.deltaTime * timeMoveGround, 0);
        //    //clamp top
        //    if (vecCurGround.y <= 0)
        //    {
        //        vecCurGround = new Vector3(vecCurGround.x, 0, vecCurGround.z);
        //    }
        //    Debug.Log(vecCurGround + " top ");
        //    transform.position = vecCurGround;
        //}
        //else if (transfPlayer.position.y > transfZoneOutBottom.position.y && transfPlayer.position.y < 0)
        //{
        //    vecCurGround += new Vector3(0, mov.joystickVec.y * -1 * Time.deltaTime * timeMoveGround, 0);
        //    //clamp bottom
        //    if (vecCurGround.y >= 0)
        //    {
        //        vecCurGround = new Vector3(vecCurGround.x, 0, vecCurGround.z);
        //    }
        //    Debug.Log(vecCurGround + " bottom ");
        //    transform.position = vecCurGround;
        //}
        //if (transfPlayer.position.x > transfZoneOutLeft.position.x && transfPlayer.position.x < transfZoneOutRight.position.x &&
        //    transfPlayer.position.y < transfZoneOutTop.position.y && transfPlayer.position.y > transfZoneOutBottom.position.y
        //    )
        //{
        //    if (vecCurGround.x == 0 && vecCurGround.y == 0)
        //        return;

        //    if (mov.joystickVec.x <= 0)
        //    {
        //        vecCurGround += new Vector3(mov.joystickVec.x * -1 * Time.deltaTime * timeMoveGround, 0, 0);
        //        //clamp width
        //        float xx = Mathf.Clamp(vecCurGround.x, 0, vecCurGround.x + 1);
        //        transform.position = new Vector3(xx, vecCurGround.y, vecCurGround.z);
        //        vecCurGround = transform.position;
        //    }
        //    else if (mov.joystickVec.x > 0)
        //    {
        //        vecCurGround += new Vector3(mov.joystickVec.x * -1 * Time.deltaTime * timeMoveGround, 0, 0);
        //        //clamp width
        //        float xx = Mathf.Clamp(vecCurGround.x, vecCurGround.x - 1, 0);
        //        transform.position = new Vector3(xx, vecCurGround.y, vecCurGround.z);
        //        vecCurGround = transform.position;
        //    }
        //    if (mov.joystickVec.y <= 0)
        //    {
        //        vecCurGround += new Vector3(0, mov.joystickVec.y * -1 * Time.deltaTime * timeMoveGround, 0);
        //        //clamp width
        //        float yy = Mathf.Clamp(vecCurGround.y, 0, vecCurGround.y + 1);
        //        transform.position = new Vector3(vecCurGround.x, yy, vecCurGround.z);
        //        vecCurGround = transform.position;
        //    }
        //    else if (mov.joystickVec.y > 0)
        //    {
        //        vecCurGround += new Vector3(0, mov.joystickVec.y * -1 * Time.deltaTime * timeMoveGround, 0);
        //        //clamp width
        //        float yy = Mathf.Clamp(vecCurGround.y, vecCurGround.y - 1, 0);
        //        transform.position = new Vector3(vecCurGround.x, yy, vecCurGround.z);
        //        vecCurGround = transform.position;
        //    }
        //}
    }
}
