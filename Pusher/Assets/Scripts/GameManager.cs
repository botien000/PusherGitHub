using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayUI playUI;
    [SerializeField] private OverUI overUI;
    [SerializeField] private GameObject tutorialUIGO;
    [SerializeField] private MovementJoystick joystick;
    [SerializeField] private Transform parentBox, parentGarbage;
    [SerializeField] private LevelSctb[] levels;
    [SerializeField] private int curScene;

    public Action<bool> actionControlVehicle;
    private int preLevelTotal;
    private int finalLevel;
    private Player player;
    private int curScore;
    private BoxManager curBoxParent;
    private GarbageManager curGarbageParent;
    private int totalGarbage;
    private int curLevelIndex;
    private int minute = 0, second = 0;

    public enum StateGame
    {
        Play, Over
    }

    public static GameManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    private void Start()
    {
        ResetLevel();
        player = FindObjectOfType<Player>();
        preLevelTotal = PlayerPrefs.GetInt("Previous Level Total",0);
        curLevelIndex = PlayerPrefs.GetInt("Player in level ",1) - preLevelTotal - 2;
        finalLevel = PlayerPrefs.GetInt("Final level");
        curScore = PlayerPrefs.GetInt("Score", 0);
        if (curLevelIndex >= levels.Length - 1)
        {
            Debug.LogError("Out array level.Fix bug");
            return;
        }
        playUI.SetScore(curScore);
        SetStateGame(StateGame.Play);
    }
    private void Update()
    {

    }
    public StateGame curState;

    public void SetStateGame(StateGame state)
    {
        curState = state;
        switch (state)
        {
            case StateGame.Play:
                playUI.SetTextTime("00", "00");
                StartCoroutine("IECountTime");
                //play
                playUI.SetActiveGOIngore(false);
                playUI.IsOver = false;
                playUI.VisibleUI(true);
                playUI.InitIconParent();
                if (playUI.ShowOn)
                {
                    playUI.BtnShowButton();
                }
                playUI.IsOver = false;
                if (curLevelIndex == levels.Length - 1)
                {
                    curLevelIndex = 0;
                    LoadNextScene();
                    break;
                }
                //next level
                NextLevel();
                //movement from player
                joystick.gameObject.SetActive(true);
                player.MoveToGamePlayPosition();
                //actionVehicle
                actionControlVehicle(false);
                if (curLevelIndex == 0 && curScene == 1)
                {
                    //show panel tutorial
                    tutorialUIGO.SetActive(true);
                }
                break;
            case StateGame.Over:
                StopCoroutine("IECountTime");
                //data
                PlayerPrefs.SetInt("Score", curScore);
                //actionVehicle
                actionControlVehicle(true);
                //check level to change text into AGAIN
                if (player.CurState == Player.State.Dying)
                {
                    //lose level
                    StartCoroutine(IEInitGameOverUI(true, preLevelTotal + curLevelIndex + 1, 2f));
                }
                else
                {
                    if (preLevelTotal + curLevelIndex + 1 == finalLevel)
                    {
                        //final level
                        StartCoroutine(IEInitGameOverUI(false, preLevelTotal + curLevelIndex + 1, 0.8f));
                        break;
                    }
                    StartCoroutine(IEInitGameOverUI(false, preLevelTotal + curLevelIndex + 1, 0.8f));
                }
                break;
        }
    }
    IEnumerator IEInitGameOverUI(bool isOver, int lv, float timeDelay)
    {
        playUI.SetActiveGOIngore(true);
        yield return new WaitForSeconds(timeDelay / 2f);
        if (!playUI.ShowOn)
        {
            playUI.BtnShowButton();
        }
        playUI.IsOver = true;
        yield return new WaitForSeconds(timeDelay);
        joystick.gameObject.SetActive(false);
        //over
        overUI.gameObject.SetActive(true);
        overUI.Init(isOver, lv, minute, second);
        if (!isOver)
        {
            //win level
            //set highest hour and second
            int curLevel = preLevelTotal + curLevelIndex + 1;
            if (PlayerPrefs.GetInt("Minute of level " + curLevel.ToString()) == 0 &&
                PlayerPrefs.GetInt("Second of level " + curLevel.ToString()) == 0)
            {
                PlayerPrefs.SetInt("Minute of level " + curLevel.ToString(), minute);
                PlayerPrefs.SetInt("Second of level " + curLevel.ToString(), second);
            }
            else
            {
                if (PlayerPrefs.GetInt("Minute of level " + curLevel.ToString()) > minute)
                {
                    PlayerPrefs.SetInt("Minute of level " + curLevel.ToString(), minute);
                    PlayerPrefs.SetInt("Second of level " + curLevel.ToString(), second);
                }
                else if (PlayerPrefs.GetInt("Minute of level " + curLevel.ToString()) == minute)
                {
                    if (PlayerPrefs.GetInt("Second of level " + curLevel.ToString()) > second)
                    {
                        PlayerPrefs.SetInt("Second of level " + curLevel.ToString(), second);
                    }
                }
            }
            if (curLevel < finalLevel)
            {
                //unlock next level
                if (PlayerPrefs.GetInt("Second of level " + (curLevel + 1).ToString(), -1) == -1)
                {
                    PlayerPrefs.SetInt("Second of level " + (curLevel + 1).ToString(), 0);
                    PlayerPrefs.SetInt("Minute of level " + (curLevel + 1).ToString(), 0);
                }
            }

        }
    }
    IEnumerator IECountTime()
    {
        second = 0;
        minute = 0;
        string m = "", s = "";
        while (true)
        {
            yield return new WaitForSeconds(1f);
            second++;
            if (second == 59)
            {
                minute++;
                second = 0;
            }
            if (minute < 10)
            {
                m = "0" + minute;
            }
            else
            {
                m = minute + "";
            }
            if (second < 10)
            {
                //add zero before it
                s = "0" + second;
            }
            else
            {
                s = second + "";
            }
            playUI.SetTextTime(m, s);
        }
    }
    public void ReLoadLevel()
    {
        curLevelIndex--;
        player.SetState(Player.State.Living);
    }
    internal void RemoveBox_GarbageParentPrevious()
    {
        //remove two parent
        Destroy(curBoxParent.gameObject);
        Destroy(curGarbageParent.gameObject);
    }
    public void NextLevel()
    {
        curLevelIndex++;
        PlayerPrefs.SetInt("Player in level ", preLevelTotal + curLevelIndex + 1);
        overUI.gameObject.SetActive(false);
        totalGarbage = levels[curLevelIndex].gObjGarbageParent.garbages.Length;
        //Handle spawn box parent and garbage parent
        HandleSpawnBoxParent_GarbageParent();
    }
    private void ResetLevel()
    {
        curLevelIndex = -1;
        totalGarbage = 0;
    }
    private void LoadNextScene()
    {
        SceneManager.LoadScene(curScene + 1);
        //save data
        PlayerPrefs.SetInt("Player in level ", levels.Length + curLevelIndex + 1);
        PlayerPrefs.SetInt("Previous Level Total", levels.Length);
    }
    public void ReduceCurGarbage(int total)
    {
        curScore += total;
        playUI.SetScore(curScore);
        totalGarbage -= total;
        if (totalGarbage == 0)
        {
            //game over
            SetStateGame(StateGame.Over);
        }
    }
    public void RemoveAllVehicle()
    {
        Vehicle[] vehicles = FindObjectsOfType<Vehicle>();
        foreach (var vehicle in vehicles)
        {
            Destroy(vehicle.gameObject);
        }
    }
    public void InitIconParent()
    {
        playUI.MoveIconParent();
    }
    private void HandleSpawnBoxParent_GarbageParent()
    {
        //spawn box parent
        curBoxParent = Instantiate(levels[curLevelIndex].gObjBoxParent, parentBox);
        //spawn garbage parent
        curGarbageParent = Instantiate(levels[curLevelIndex].gObjGarbageParent, parentGarbage);
    }
}