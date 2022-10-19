using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public enum State
    {
        Living, Dying
    }
    public int force;
    [SerializeField] private Sprite imgLiving, imgDying;
    [SerializeField] private float speedMove, speedToPoint;
    [SerializeField] private MovementJoystick movementJoystick;
    [SerializeField] private Transform posStatePlay, posStateHome;
    [SerializeField] private ParticleSystem particle;

    private ParticleSystem.EmissionModule emissionModule;
    private State curState;
    private Image imgObj;
    [HideInInspector]
    public bool autoMove;
    private bool isCollideVehicle;
    private Rigidbody2D rgbody;

    public State CurState { get => curState; set => curState = value; }

    private void Awake()
    {
        imgObj = GetComponent<Image>();
        rgbody = GetComponent<Rigidbody2D>();
    }
    private void OnEnable()
    {
    }
    // Start is called before the first frame update
    void Start()
    {
        emissionModule = particle.emission;
        rgbody.drag = 0;
        SetState(State.Living);
    }
    private void FixedUpdate()
    {
        if (imgObj.sprite == imgDying)
        {
            return;
        }
        Move();
    }
    private void Update()
    {
     
    }
    private void Direction(float angle)
    {
        transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
    }
    public void SetState(State state)
    {
        curState = state;
        switch (state)
        {
            case State.Living:
                imgObj.sprite = imgLiving;
                break;
            case State.Dying:
                imgObj.sprite = imgDying;
                GameManager.Instance.SetStateGame(GameManager.StateGame.Over);
                break;
        }
    }
    private void Move()
    {
        if (movementJoystick.joystickVec.y != 0)
        {
            rgbody.velocity = new Vector2(movementJoystick.joystickVec.x, movementJoystick.joystickVec.y) * speedMove * Time.fixedDeltaTime;
            //AudioManager.instance.SoundPlayerRun();
            Direction(movementJoystick.angle);
            FireSmoke(15f);
        }
        else
        {
            //AudioManager.instance.SoundPlayerStop();
            rgbody.velocity = Vector2.zero;
            FireSmoke(0f);
        }
    }
    public void MoveToGamePlayPosition()
    {
        transform.position = posStatePlay.position;
        Direction(90f);
    }
    public void MoveToGameHomePosition()
    {
        movementJoystick.gameObject.SetActive(false);
        transform.position = posStateHome.position;
        Direction(115f + 90f);
    }
    private void FireSmoke(float rateEmission)
    {
        emissionModule.rateOverTime = rateEmission;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Vehicle"))
        {
            isCollideVehicle = true;
            rgbody.drag = 10;
            SetState(State.Dying);
            Vehicle vehicle = collision.collider.GetComponent<Vehicle>();
            float forceVehicle = (Math.Abs(vehicle.GetComponent<Rigidbody2D>().velocity.x) + Math.Abs(vehicle.GetComponent<Rigidbody2D>().velocity.y)) / 2;
            float forceThis = (Math.Abs(rgbody.velocity.x) + Math.Abs(rgbody.velocity.y)) / 2;
            Vector2 directionImpulse = -(vehicle.transform.position - transform.position);
            rgbody.AddForce(directionImpulse * force * (forceVehicle + forceThis), ForceMode2D.Force);
        }
        else if (collision.collider.CompareTag("Train"))
        {
            isCollideVehicle = true;
            rgbody.drag = 10;
            SetState(State.Dying);  
            Train train = collision.collider.GetComponent<Train>();
            float forceVehicle = (Math.Abs(train.GetComponent<Rigidbody2D>().velocity.x) + Math.Abs(train.GetComponent<Rigidbody2D>().velocity.y)) / 2;
            float forceThis = (Math.Abs(rgbody.velocity.x) + Math.Abs(rgbody.velocity.y)) / 2;
            Vector2 directionImpulse = -(train.transform.position - transform.position);
            rgbody.AddForce(directionImpulse * force * (forceVehicle + forceThis), ForceMode2D.Force);
        }
        else if (collision.collider.CompareTag("Wall") && isCollideVehicle)
        {
            collision.collider.GetComponentInParent<Wall>().SetTrigger(collision.collider);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Vehicle"))
        {
            isCollideVehicle = false;
        }
    }
}
