using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UIElements;

public class ProjectileBehaviour : MonoBehaviour
{
    [SerializeField] private float speed;
    private Rigidbody2D rb;
    private float timeTillDelete = 3f;
    //[SerializeField] private AnimatorController animCntr;
    private Animator animator;
    [SerializeField] private Sprite[] images;
    private SpriteRenderer projSprite;
    [SerializeField] private PlayerScript plyrMov;

    void Start()
    {
        plyrMov = FindObjectOfType<PlayerScript>();
        projSprite = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
        if(plyrMov != null)
        {
            setProjectile(plyrMov.getSpellSelected());
        }
        else
        {
            setProjectile(0);
        }
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * speed;
    }
    void Update()
    {
        timeTillDelete -= Time.deltaTime;
        if(timeTillDelete <= 0)
        {
            Destroy(gameObject);
        }
    }
    public void setSpeed(float setSpeed)
    {
        speed = setSpeed;
    }
    public void setProjectile(int spell)
    {
        projSprite.sprite = images[spell];
        animator.SetInteger("SpellVar", spell);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Object is colliding with: " + other.gameObject.name);
        Destroy(gameObject);
    }
}
