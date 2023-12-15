using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private GameObject vCamera;
    private Vector3 direction;
    private bool isFireStart = false;
    void Start()
    {
        plyrMov = FindObjectOfType<PlayerScript>();
        projSprite = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        if(!isFireStart)
        {
            normalStart();
        }
    }
    void normalStart()
    {
        if(isPlayer == true)
        {
            setProjectile(plyrMov.getSpellSelected());
            vCamera = GameObject.FindWithTag("Camera");
            Vector3 mousePosition = vCamera.GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;
            Vector3 direction = (mousePosition - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            rb.velocity = direction * (speed + player.GetComponent<PlayerScript>().getCurrentSpeed()/10);
            int ignoreplayer = 1 << LayerMask.NameToLayer("Player");
            this.GetComponent<CircleCollider2D>().usedByEffector = true;
            this.GetComponent<CircleCollider2D>().excludeLayers = ignoreplayer;
        }
        else
        {
            setProjectile(0);
            direction = player.transform.position - transform.position;
            rb.velocity = new Vector2(direction.x, direction.y).normalized * speed;
            float rot = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, rot + 180);
            //int ignoreRaycastLayer = LayerMask.NameToLayer("Ignore Raycast");
            //int extrasLayer = LayerMask.NameToLayer("Extras");
            int ignoreRaycastLayer = 1 << LayerMask.NameToLayer("Ignore Raycast");
            int extrasLayer = 1 << LayerMask.NameToLayer("Extras");
            int combinedLayerMask = ignoreRaycastLayer | extrasLayer;
            this.GetComponent<CircleCollider2D>().usedByEffector = true;
            this.GetComponent<CircleCollider2D>().excludeLayers = combinedLayerMask;
        }
    }
    void fireCircleStart()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }
    public void setIsFireStart(){isFireStart = true;}
    void Update()
    {
        timeTillDelete -= Time.deltaTime;
        if(timeTillDelete <= 0)
        {
            Destroy(gameObject);
        }
        if(isFireStart){fireCircleStart();}
    }
    public void setSpeed(float setSpeed){speed = setSpeed;}
    public void setProjectile(int spell)
    {
        projSprite.sprite = images[spell];
        animator.SetInteger("SpellVar", spell);
        if(spell == 1)
        {
            damage = 40;
        }
        else
        {
            damage = 20;
        }
    }
    public void setIsPlayer(){isPlayer = true;}
    
    void OnCollisionEnter2D(Collision2D other)
    {
        //Debug.Log("Object is colliding with: " + other.gameObject.name);
        if(other.gameObject.name == "Player" && gameObject.name.Substring(0, Mathf.Min(5, other.gameObject.name.Length)) == "Proje") 
        {
            other.gameObject.GetComponent<PlayerScript>().setHealth(damage);
            Destroy(gameObject);
        }
        else if(other.gameObject.name.Substring(0, Mathf.Min(7, other.gameObject.name.Length)) == "Enemy -")
        {
            other.gameObject.GetComponent<EnemyBehavior>().setHealth(damage);
        }
        else if(other.gameObject.name.Substring(0, Mathf.Min(7, other.gameObject.name.Length)) == "EnemyV2")
        {
            other.gameObject.GetComponent<EnemyModes>().setHealth(damage);
            Destroy(gameObject);
        }
        else if(other.gameObject.name != "Player")
        {
            Destroy(gameObject);
        }
    }
}
