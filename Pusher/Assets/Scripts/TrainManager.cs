using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainManager : MonoBehaviour
{
    [SerializeField] private Train trainPrefab;
    [SerializeField] private float timeLight;
    [SerializeField] private Transform posSpawn;
    [SerializeField] private Transform posRemove;

    private Animator anitorLight;
    private bool turnLight;
    private float curTimeLight;
    private bool isStopped;
    private AudioManager instanceAM;
    private void Awake()
    {
        anitorLight = GetComponent<Animator>();
        instanceAM = AudioManager.Instance;
    }

    void OnEnable()
    {
        GameManager.Instance.actionControlVehicle += Handle;
    }
    void OnDisable()
    {
        GameManager.Instance.actionControlVehicle -= Handle;
    }


    // Update is called once per frame
    void Update()
    {
        if (isStopped)
            return;
        curTimeLight -= Time.deltaTime;
        if (curTimeLight <= 0 && !turnLight)
        {
            SetTrainLight(true);
        }
    }
    private void SetTrainLight(bool turn)
    {
        //turn on
        if (turn)
        {
            turnLight = turn;
            instanceAM.PlayTrainLightSound(true);
        }
        //turn off
        else
        {
            curTimeLight = timeLight;
            turnLight = turn;
        }
        anitorLight.SetBool("Turn Light", turn);

    }
    public void TrainRun()
    {
        //spawn train object
        Instantiate(trainPrefab, posSpawn.position, Quaternion.identity, posSpawn).Init(posRemove.position);
        //set light = false
        SetTrainLight(false);
    }
    public void PauseSoundTrain()
    {
        instanceAM.PlayTrainLightSound(false);
    }
    private void Handle(bool stop)
    {
        if (stop)
        {
            isStopped = true;
        }
        else
        {
            isStopped = false;
            curTimeLight = timeLight;
        }
    }
}
