using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;

public class PlayUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtScore;
    [SerializeField] private TextMeshProUGUI txtTime;
    [SerializeField] private Button btnOver;
    [SerializeField] private Animator anitorButtonShow;
    [SerializeField] private Transform transIconParent, transFrom, transTo;
    [SerializeField] private float timeTransitionIconParent;
    [SerializeField] private float timeCooldown;
    [SerializeField] private TutorialUI tutorialUI;
    [SerializeField] private GameObject ingoreTouchGO;

    private Transform pointTo;
    private float curTimeCooldown;
    private bool isRun;
    private bool isOver;
    private bool showOn;

    public bool ShowOn { get => showOn; set => showOn = value; }
    public bool IsOver { get => isOver; set => isOver = value; }

    private void Update()
    {
        if(isOver)
        {
            transIconParent.position = Vector3.MoveTowards(transIconParent.position, transTo.position, Time.deltaTime * timeTransitionIconParent);
            return;
        }
        if (!isRun)
            return;
        transIconParent.position = Vector3.MoveTowards(transIconParent.position, pointTo.position, Time.deltaTime * timeTransitionIconParent);
        if (transIconParent.position == pointTo.position && pointTo.position == transTo.position)
        {
            curTimeCooldown += Time.deltaTime;
            if (curTimeCooldown >= timeCooldown)
            {
                //go back
                pointTo.position = transFrom.position;
            }
        }
        else if (transIconParent.position == pointTo.position && pointTo.position == transFrom.position)
        {
            isRun = false;
        }
    }
    public void SetActiveGOIngore(bool isActive)
    {
        ingoreTouchGO.SetActive(isActive);
    }
    public void InitIconParent()
    {
        isRun = false;
        transIconParent.position = transFrom.position;
    }
    public void MoveIconParent()
    {
        isRun = true;
        curTimeCooldown = 0;
        pointTo = transTo;
    }
    public void SetScore(int score)
    {
        txtScore.text = score.ToString();
    }
    public void VisibleUI(bool isGamePlay)
    {
        if (!isGamePlay)
        {
            btnOver.gameObject.SetActive(false);
        }
        else
        {
            btnOver.gameObject.SetActive(true);
        }
    }

    public void BtnOver()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void BtnShowButton()
    {
        if (showOn)
        {
            showOn = false;
        }
        else
        {
            showOn = true;
        }
        anitorButtonShow.SetBool("Show", showOn);
    }
    public void BtnTutorial()
    {
        tutorialUI.gameObject.SetActive(true);
    }
    public void SetTextTime(string minute, string second)
    {
        txtTime.text = minute + ":" + second;
    }
}
