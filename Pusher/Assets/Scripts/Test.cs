using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    ////private Vector3 endPosition = new Vector3(5, -2, 0);
    //public Transform trans;
    //private Transform startPosition;
    //private float desiredDuration = 3f;
    //private float elapsedTime;

    //[SerializeField]
    //private AnimationCurve curve;

    //// Start is called before the first frame update
    //void Start()
    //{
    //    startPosition = transform;
    //    startPosition.position = (Vector2)startPosition.position;
    //    Debug.Log(startPosition.position);
    //    transform.position = (Vector2)transform.position;
    //    trans.position = (Vector2)trans.position;
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    elapsedTime += Time.deltaTime;
    //    float percentageComplete = elapsedTime / desiredDuration;
    //    Vector2 vector2 = startPosition.position;
    //    Vector2 vector23 = trans.position;

    //    transform.position = Vector3.Lerp(vector2, vector23,/* curve.Evaluate(percentageComplete)*/ percentageComplete);
    //    Debug.Log(transform.position + "    " + trans.position);
    //}
    //private Vector3 endPosition = new Vector3(5, -2, 0);
    public Transform trans;
    private Transform startPosition;
    private float desiredDuration = 3f;
    private float elapsedTime;

    [SerializeField]
    private AnimationCurve curve;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform;
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        float percentageComplete = elapsedTime / desiredDuration;
        transform.position = Vector2.Lerp(startPosition.position, trans.position, percentageComplete);
    }
}
