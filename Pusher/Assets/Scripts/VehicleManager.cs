using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleManager : MonoBehaviour
{
    [SerializeField] private Vehicle[] vehicles;
    [SerializeField] private float timeSpawn;
    [SerializeField] private Transform pos;
    [SerializeField] private Transform posRemove;

    private float curTimeSpawn;
    private bool isPlayCarHorn;
    private bool isStopped;
    private AudioManager instanceAM;
    void OnEnable()
    {
        GameManager.Instance.actionControlVehicle += Handle;
    }
    void OnDisable()
    {
        GameManager.Instance.actionControlVehicle -= Handle;
    }
    private void Start()
    {
        instanceAM = AudioManager.Instance;
        isPlayCarHorn = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (isStopped)
            return;
        curTimeSpawn -= Time.deltaTime;
        if(curTimeSpawn <= 2 && isPlayCarHorn)
        {
            isPlayCarHorn = false;
            instanceAM.PlayCarHornSound();
        }
        if (curTimeSpawn <= 0)
        {
            curTimeSpawn = timeSpawn;
            isPlayCarHorn = true;
            //random vehicle
            int indexVehicle = Random.Range(0, vehicles.Length);
            //spawn vehicle
            Instantiate(vehicles[indexVehicle], pos.position, Quaternion.identity, transform).Init(posRemove.position);
        }
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
            curTimeSpawn = timeSpawn;
        }
    }

}
