using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Vehicle : MonoBehaviour
{
    [SerializeField] private float speed;

    private Vector2 posRemove;
    private Rigidbody2D rgbody;
    private bool collided;
    private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        rgbody = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        if (transform.position.x >= posRemove.x)
        {
            Destroy(gameObject);
        }
    }
    private void FixedUpdate()
    {
        if (collided)
        {
            rgbody.velocity = Vector2.zero;
            SetAnimationWheel(false);
            return;
        }
        rgbody.velocity = new Vector2(speed * Time.fixedDeltaTime, rgbody.velocity.y);
        SetAnimationWheel(true);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collided = true;
        }
    }
    private void SetAnimationWheel(bool isScrolling)
    {
        animator.SetBool("Scroll", isScrolling);
    }
    public void Init(Vector2 pos)
    {
        posRemove = pos;
    }
}
