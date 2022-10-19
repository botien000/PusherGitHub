using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class LevelUI : MonoBehaviour
{
    [SerializeField] private Button[] buttons;
    [SerializeField] private Transform playerLevelUI;
    [SerializeField] private Transform transPointPlayerLvMove;
    [SerializeField] private float timeMove;

    private Vector3 vecOriginLevelUI;
    private AnimationManager aniManager;
    private int curLevelFromUser;
    private TextMeshProUGUI[] txtLevels;
    private void Awake()
    {
        txtLevels = new TextMeshProUGUI[buttons.Length];
        for (int i = 0; i < txtLevels.Length; i++)
        {
            txtLevels[i] = buttons[i].GetComponentInChildren<TextMeshProUGUI>();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        vecOriginLevelUI = transform.position;
        curLevelFromUser = PlayerPrefs.GetInt("Player in level ");
        Init();
        InitPlayerLv();
        aniManager = FindObjectOfType<AnimationManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(aniManager.CurState == AnimationManager.StateAnimate.MoveUIHome)
        {
            if(vecOriginLevelUI != transform.position)
            {
                transform.position = Vector2.MoveTowards(transform.position, vecOriginLevelUI, Time.deltaTime * timeMove);
            }
        }
    }
    public void Init()
    {
        int time;
        for (int i = 0; i < buttons.Length; i++)
        {
            time = PlayerPrefs.GetInt("Second of level " + (i + 1), -1);
            txtLevels[i].text = (i + 1).ToString() + "";
            if (time >= 0)
            {
                buttons[i].interactable = true;
            }
            else
            {
                buttons[i].interactable = false;
            }
        }
    }
    public void BtnLevel(ButtonLevel buttonLevel)
    {
        aniManager.StartPlayerMoveToPointLevel(playerLevelUI, buttonLevel, null);
    }
    public void InitPlayerLv()
    {
        playerLevelUI.position = buttons[curLevelFromUser - 1].transform.position;
    }
}
