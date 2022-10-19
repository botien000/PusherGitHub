using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] private VideoClip[] clipTutorials;
    [SerializeField] private Button btnNext, btnBack;
    [SerializeField] private VideoPlayer videoPlayer;

    private Animator animator;
    private int curClip;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        Init();
    }
    public void ButtonVideoTutorial(int type)
    {
        videoPlayer.Prepare();
        //0 : back ; 1 : next
        if (type == 1 && curClip < clipTutorials.Length)
        {
            curClip++;
            videoPlayer.clip = clipTutorials[curClip - 1];
            videoPlayer.Play();
            SetEnableButtonTutorial(curClip);
        }
        else if (type == 0 && curClip > 1)
        {
            curClip--;
            videoPlayer.clip = clipTutorials[curClip - 1];
            videoPlayer.Play();
            SetEnableButtonTutorial(curClip);
        }
    }
    public void Init()
    {
        Time.timeScale = 0f;
        curClip = 1;
        videoPlayer.clip = clipTutorials[curClip - 1];
        videoPlayer.Play();
        SetEnableButtonTutorial(curClip);
    }
    private void OnDisable()
    {
        if (videoPlayer == null)
            return;
        videoPlayer.Stop();
        videoPlayer.Prepare();
    }   
    public void SetEnableButtonTutorial(int clip)
    {
        switch (clip)
        {
            case 1:
                btnBack.gameObject.SetActive(false);
                break;
            case 2:
                btnBack.gameObject.SetActive(true);
                btnNext.gameObject.SetActive(true);
                break;
            case 3:
                btnNext.gameObject.SetActive(false);
                break;
        }
    }
    public void BtnUnderStand()
    {
        animator.SetBool("isClose", true);
    }
    public void EventDisable()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1f;
    }
}
