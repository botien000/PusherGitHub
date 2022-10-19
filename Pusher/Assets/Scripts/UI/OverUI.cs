using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class OverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtLv;
    [SerializeField] private TextMeshProUGUI txtBtnN_A;
    [SerializeField] private TextMeshProUGUI txtTime;
    [SerializeField] private Button btnNext;

    private int minuteG, secondG;
    private GameManager instanceGM;
    // Start is called before the first frame update
    void Start()
    {
        instanceGM = GameManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void BtnHome()
    {
        AudioManager.Instance.PlaySoundButton();
        HandleBtnHome();
        instanceGM.RemoveBox_GarbageParentPrevious();
    }
    private void HandleBtnHome()
    {
        SceneManager.LoadScene(0);
    }
    public void BtnNextLevel()
    {
        AudioManager.Instance.PlaySoundButton();
        HandleBtnNextLevel();
    }
    private void HandleBtnNextLevel()
    {
        if (txtBtnN_A.text.Contains("Again"))
        {
            instanceGM.ReLoadLevel();
        }
        instanceGM.RemoveAllVehicle();
        instanceGM.RemoveBox_GarbageParentPrevious();
        instanceGM.SetStateGame(GameManager.StateGame.Play);
    }
    public void Init(bool onlyOver, int lv, int minute, int second)
    {
        minuteG = minute;
        secondG = second;
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
        if (onlyOver)
        {
            txtLv.text = "Stage " + lv.ToString();
            txtBtnN_A.text = "Again";
            txtTime.fontStyle = FontStyles.Strikethrough;
        }
        else
        {
            if (lv == 10)
            {
                btnNext.interactable = false;
            }
            else
            {
                btnNext.interactable = true;
            }
            txtLv.text = "Stage " + lv.ToString();
            txtBtnN_A.text = "Next";
            txtTime.fontStyle = FontStyles.Normal;
        }
        txtTime.text = m + "m " + s + "s";
    }
}
