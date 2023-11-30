using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
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
    private float damage = 20f;
    private bool isPlayer = false;
    private GameObject player;

    void Start()
    {
        plyrMov = FindObjectOfType<PlayerScript>();
        projSprite = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        if(isPlayer == true)
        {
            setProjectile(plyrMov.getSpellSelected());
            rb.velocity = transform.right * speed;
        }
        else
        {
            setProjectile(0);
            player = GameObject.FindGameObjectWithTag("Player");
            Vector3 direction = player.transform.position - transform.position;
            rb.velocity = new Vector2(direction.x, direction.y).normalized * speed;
            float rot = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, rot + 180);
            int ignoreRaycastLayer = LayerMask.NameToLayer("Ignore Raycast");
            this.GetComponent<CircleCollider2D>().usedByEffector = true;
            this.GetComponent<CircleCollider2D>().excludeLayers = ignoreRaycastLayer;
            
        }
        
        
    }
    void Update()
    {
        timeTillDelete -= Time.deltaTime;
        if(timeTillDelete <= 0)
        {
            Destroy(gameObject);
        }
    }
    public void setSpeed(float setSpeed){speed = setSpeed;}
    public void setProjectile(int spell)
    {
        projSprite.sprite = images[spell];
        animator.SetInteger("SpellVar", spell);
    }
    public void setIsPlayer(){isPlayer = true; Debug.Log("CALLED HERE!");}
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Object is Triggering with: " + other.gameObject.name);
        if(other.gameObject.name == "Enemy")
        {
            GameObject target = other.gameObject;
            target.GetComponent<EnemyBehavior>().setHealth(damage);
        }
        Destroy(gameObject);
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("Object is colliding with: " + other.gameObject.name);
        if(other.gameObject.name == "Enemy")
        {
            GameObject target = other.gameObject;
            target.GetComponent<EnemyBehavior>().setHealth(damage);
        }
        Destroy(gameObject);
    }
}
