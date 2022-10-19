using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Train : MonoBehaviour
{
    [SerializeField] private float speed;

    private Vector2 posRemove;
    private Rigidbody2D rgbody;
    private void Awake()
    {
        rgbody = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if (transform.position.x >= posRemove.x)
        {
            Destroy(gameObject);
        }
    }
    private void FixedUpdate()
    {
        rgbody.velocity = new Vector2(speed * Time.fixedDeltaTime, rgbody.velocity.y);
    }
    public void Init(Vector2 pos)
    {
        posRemove = pos;
    }

}
