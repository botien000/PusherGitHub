using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public enum StateAnimate
    {
        MoveUILevel, MoveUIHome, MovePlayer, NoAnimate
    }
    [SerializeField] private Transform transContent;
    [SerializeField] private Transform transLevel;
    [SerializeField] private Transform transViewportUI;
    private Vector2 pointOriginContent;
    private Vector2 pointTo;
    [SerializeField] private float speedMoveUI;

    [SerializeField] private GameObject btnBackHomeGO;
    [SerializeField] private Transform transInsideGO;
    [SerializeField] private Transform transOutsideGO;
    [SerializeField] private float timeMoveBtnBackHome;
    private bool allowBtnBHMove;
    private Transform transMove;

    [SerializeField] private BoardDetailLevelUI boardDetailLv;
    [SerializeField] private float timeMovePlayer;
    private float curTimePlayer;
    private Vector3 pointOriginPlayer;
    private Transform transPlayerLv, transPointMove, transPointCurrent;
    private ButtonLevel btnLv;

    [SerializeField] private GameObject raycastPanel;

    private StateAnimate curState;

    public StateAnimate CurState { get => curState; set => curState = value; }

    // Start is called before the first frame update
    void Start()
    {

        curState = StateAnimate.NoAnimate;
        pointOriginContent = transContent.position;
        btnBackHomeGO.transform.position = transOutsideGO.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (allowBtnBHMove)
        {
            btnBackHomeGO.transform.position = Vector2.MoveTowards(btnBackHomeGO.transform.position, transMove.position, Time.deltaTime * 10f * timeMoveBtnBackHome);
            if (btnBackHomeGO.transform.position == transMove.position)
                allowBtnBHMove = false;
        }
        HandleAnimation();
    }
    public void TransitionToLevel()
    {
        raycastPanel.SetActive(true);
        Vector2 movetovector = transLevel.position - transViewportUI.position;
        pointTo = (Vector2)transContent.position - movetovector;
        curState = StateAnimate.MoveUILevel;
    }
    public void TransitionToHome()
    {
        AudioManager.Instance.PlaySoundButton();
        allowBtnBHMove = true;
        transMove = transOutsideGO;
        raycastPanel.SetActive(true);
        curState = StateAnimate.MoveUIHome;
    }
    public void StartPlayerMoveToPointLevel(Transform transPlayer, ButtonLevel buttonLevel, Transform transPoint)
    {

        pointOriginPlayer = transPlayer.position;
        transPlayerLv = transPlayer;
        transPointMove = transPoint;
        btnLv = buttonLevel;
        curState = StateAnimate.MovePlayer;

    }
    private void DirectionPlayer(Vector2 pointPlayer, Vector2 pointTo)
    {
        Vector2 vector = pointTo - pointPlayer;
        if (vector == Vector2.zero)
            return;
        float angle = Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;
        transPlayerLv.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
    }
    private void HandleAnimation()
    {
        switch (curState)
        {
            case StateAnimate.MoveUILevel:
                transContent.position = Vector2.MoveTowards(transContent.position, pointTo, Time.deltaTime * speedMoveUI * 10f);
                if ((Vector2)transContent.position == pointTo)
                {
                    allowBtnBHMove = true;
                    transMove = transInsideGO;
                    raycastPanel.SetActive(false);
                }
                break;
            case StateAnimate.MoveUIHome:
                transContent.position = Vector2.MoveTowards(transContent.position, pointOriginContent, Time.deltaTime * speedMoveUI * 10f);
                if ((Vector2)transContent.position == pointOriginContent)
                {
                    raycastPanel.SetActive(false);
                }
                break;
            case StateAnimate.MovePlayer:
                curTimePlayer += Time.deltaTime;
                if (transPointMove == null)
                {
                    //go ward
                    transPointCurrent = btnLv.transform;
                    Debug.Log("player: " + transPlayerLv.position + "  point: " + transPointCurrent.position);
                    if (transPlayerLv.position == transPointCurrent.position)
                    {
                        Debug.Log("ok");
                        //handle button
                        raycastPanel.SetActive(false);
                        curState = StateAnimate.NoAnimate;
                        transPointCurrent = null;
                        curTimePlayer = 0;
                        boardDetailLv.gameObject.SetActive(true);
                        boardDetailLv.Init(btnLv);
                        break;
                    }
                    float time = curTimePlayer / timeMovePlayer;
                    transPlayerLv.position = Vector2.Lerp(pointOriginPlayer, transPointCurrent.position, time);
                }
                else
                {
                    //go to point then to ward
                    //go to point
                    if (transPointCurrent == null)
                    {
                        transPointCurrent = transPointMove;
                    }
                    else
                    {
                        //go to ward    
                        if ((Vector2)transPlayerLv.position == (Vector2) transPointCurrent.position && (Vector2)transPlayerLv.position != (Vector2)btnLv.transform.position)
                        {
                            transPointCurrent = transPointMove;
                        }
                        else if ((Vector2)transPlayerLv.position == (Vector2)btnLv.transform.position)
                        {
                            //handle button
                            raycastPanel.SetActive(false);
                            curState = StateAnimate.NoAnimate;
                            transPointCurrent = null;
                            curTimePlayer = 0;
                            break;
                        }
                    }
                    transPlayerLv.position = Vector2.Lerp(pointOriginPlayer, transPointCurrent.position, curTimePlayer / timeMovePlayer);
                }
                raycastPanel.SetActive(true);
                DirectionPlayer(transPlayerLv.position, transPointCurrent.position);
                break;
            default:
                break;
        }
    }
}