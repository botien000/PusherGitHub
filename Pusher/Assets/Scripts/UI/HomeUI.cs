using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;

public class HomeUI : MonoBehaviour
{
    [SerializeField] private Button btnLv;
    [SerializeField] private Button btnPlay;
    [SerializeField] private int finalLevel;
    [SerializeField] private GameObject panelSettingGO;
    [SerializeField] private GameObject xSoundGO, xMusicGO;
    [SerializeField] private TextMeshProUGUI txtScore;

    private AudioManager instanceAM;
    private AnimationManager aniManager;
    private void Awake()
    {
        aniManager = FindObjectOfType<AnimationManager>();
        //init first level
        if (PlayerPrefs.GetInt("Second of level 1", -1) == -1)
        {
            PlayerPrefs.SetInt("Player in level ", 1);
            PlayerPrefs.SetInt("Second of level 1", 0);
            PlayerPrefs.SetInt("Minute of level 1", 0);
        }
        PlayerPrefs.SetInt("Final level", finalLevel);
        txtScore.text = PlayerPrefs.GetInt("Score", 0).ToString();
    }
    private void Start()
    {
        instanceAM = AudioManager.Instance;
    }
    /// <summary>
    /// Load scene 1 lv1
    /// </summary>
    public void BtnPlay()
    {
        instanceAM.PlaySoundButton();
        ///send data
        if (PlayerPrefs.GetInt("Player in level ", 1) > 5)
        {
            //scene 2
            SceneManager.LoadScene(2);
        }
        else
        {
            //scene 1
            SceneManager.LoadScene(1);
        }
    }
    /// <summary>
    /// Load panel Level
    /// </summary>
    public void BtnLv()
    {
        instanceAM.PlaySoundButton();
        aniManager.TransitionToLevel();
    }
    public void BtnSetting()
    {
        instanceAM.PlaySoundButton();
        panelSettingGO.SetActive(true);
        xMusicGO.SetActive(instanceAM.GetMuteMusic() ? true : false);
        xSoundGO.SetActive(instanceAM.GetMuteSound() ? true : false);
    }
    public void PointUpSetting(BaseEventData baseEvent)
    {
        PointerEventData pointerEvent = baseEvent as PointerEventData;
        if (pointerEvent.pointerEnter == panelSettingGO)
        {
            panelSettingGO.SetActive(false);
        }
    }
    public void BtnSoundMusic(int type)
    {
        if (type == 1)
        {
            xSoundGO.SetActive(!xSoundGO.activeInHierarchy);
            instanceAM.SetMuteorPause(xSoundGO.activeInHierarchy, type, true);
        }
        else
        {
            xMusicGO.SetActive(!xMusicGO.activeInHierarchy);
            instanceAM.SetMuteorPause(xMusicGO.activeInHierarchy, type, true);
        }
        instanceAM.PlaySoundButton();
    }

}
