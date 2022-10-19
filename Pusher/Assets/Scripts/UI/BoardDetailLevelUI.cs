using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class BoardDetailLevelUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtLv;
    [SerializeField] private TextMeshProUGUI txtTime;
    [SerializeField] private GameObject raycastGO;

    private Animator animator;
    private int[] valuesOfBtn;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void Init(ButtonLevel buttonLevel)
    {
        valuesOfBtn = buttonLevel.GetButton();
        int level = valuesOfBtn[1] + valuesOfBtn[2];
        int minute = PlayerPrefs.GetInt("Minute of level " + level, 0);
        int second = PlayerPrefs.GetInt("Second of level " + level, 0);
        string m, s;
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
        PlayerPrefs.SetInt("Player in level ", level);
        PlayerPrefs.SetInt("Previous Level Total", valuesOfBtn[1]);
        txtLv.text = level.ToString();
        txtTime.text = "Time finish: " + m + "m " + s + "s";
    }
    public void BtnPlay()
    {
        SceneManager.LoadScene(valuesOfBtn[0]); 
    }
    public void PointUp(BaseEventData baseEvent)
    {
        PointerEventData pointerEvent = baseEvent as PointerEventData;
        if (pointerEvent.pointerEnter == raycastGO)
        {
            //turn off panel
            animator.SetBool("Off", true);
        }
    }
    public void EventDisable()
    {
        gameObject.SetActive(false);
    }

}
