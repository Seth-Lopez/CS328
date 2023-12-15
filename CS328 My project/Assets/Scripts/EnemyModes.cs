using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;


public class EnemyModes : MonoBehaviour
{
    // For Movement: 
    [SerializeField] private float walkingSpeed = 1f;
    private float sprintSpeed = 3f;
    private bool walkPointSet = false;
    private Vector3 destPoint;
    private Vector3 previousPosition;
    [SerializeField] private float rangePos = 3f;
    [SerializeField] private float waitToMove = 5f;
    [SerializeField] private float timer = 0f;
    private float sprinting = 0f;
    //NavMesh:
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform target;
    UnityEngine.AI.NavMeshAgent agent;
    // RigidBody 2D:
    private Rigidbody2D rb;
    // For Health:
    private Image healthBar;
    [SerializeField] private float currentHealth = 100f;
    [SerializeField] private float maxHealth = 100;
    // For Energy:
    private Image energyBar;
    [SerializeField] private float currentEnergy = 0;
    private float maxEnergy = 50;
    [SerializeField] private bool isSiphoning = false;
    // Line Of Sight
    private GameObject player;
    private bool hasLOS = false;
    [SerializeField] private float LOSDist;

    private SpriteRenderer spriteRenderer; 
    private Color currentColor;
    [SerializeField] private float findNumber;
    [SerializeField] private int weaponType;
    [SerializeField] private List<GameObject> prefabList = new List<GameObject>();
    private float delaySummons = 20f;


    private void Start()
    {
        //Set RigidBody:
        rb = GetComponent<Rigidbody2D>();
        //Set Player Object:
        player = GameObject.FindGameObjectWithTag("Player");
        //Get Health Bar Component:
        healthBar = this.GetComponentInChildren<Image>();
        //Set Variables:
        currentHealth = maxHealth;
        currentEnergy = 0;
        previousPosition = transform.position;
        //Set NavMesh
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        //Set SpriteRenderer
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        currentColor = spriteRenderer.color;
    }

    void Update()
    {
        updatingMovement();
        updatingHealthAndEnergy();
        if(weaponType == 0)
            sickNPC();
    }
    private void FixedUpdate()
    {
        lineOfSight();
    }
    private void updatingMovement() 
    { 
        UnityEngine.Vector3 currentPosition = transform.position;
        bool movingLeft;
        bool movingRight;
        float x = Mathf.Abs(player.transform.position.x - transform.position.x);
        float y = Mathf.Abs(player.transform.position.y - transform.position.y);
        if(hasLOS && x <= LOSDist && y <= LOSDist)
        {
            agent.speed = sprintSpeed;
            if(weaponType == 0)
                agent.SetDestination(target.position);
            else if(weaponType == 1)
            {
                if(delaySummons == 20f)
                    summonMonsters();
                delaySummons -= Time.deltaTime;
                if(delaySummons <= 0)
                {
                    delaySummons = 20f;
                }
            }
            else if(weaponType == 2) {agent.SetDestination(-target.position);}
            isSiphoning = true;
            movingLeft = player.transform.position.x < currentPosition.x;
            movingRight = player.transform.position.x > currentPosition.x;
            if(movingLeft) transform.rotation = UnityEngine.Quaternion.Euler(0f, 0f, 0f); else if (!movingLeft && movingRight) transform.rotation = UnityEngine.Quaternion.Euler(0f, 180f, 0f);
        }
        else
        {
            
            isSiphoning = false;
            timer += Time.deltaTime;
            if(timer >= waitToMove)
            {
                agent.speed = sprintSpeed;
                if(!walkPointSet) searchForDest();
                if(walkPointSet) {agent.SetDestination(destPoint); }
                if(UnityEngine.Vector3.Distance(transform.position, destPoint) < 10f)
                {
                    walkPointSet = false;
                    timer = 0;
                }
            }
            movingLeft = currentPosition.x < previousPosition.x;
            movingRight = currentPosition.x > previousPosition.x;
            previousPosition = currentPosition;
            if(movingLeft) transform.rotation = UnityEngine.Quaternion.Euler(0f, 0f, 0f); else if (!movingLeft && movingRight) transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        } 
    }
    private void lineOfSight()
    {
        RaycastHit2D ray = Physics2D.Raycast(transform.position, player.transform.position - transform.position);
        if(ray.collider != null) hasLOS = ray.collider.CompareTag("Player");
    }
    private void searchForDest()
    {
        float x = UnityEngine.Random.Range(-rangePos, rangePos);
        float y = UnityEngine.Random.Range(-rangePos, rangePos);
        destPoint = new UnityEngine.Vector3(transform.position.x + x, transform.position.y + y, 0f);
        walkPointSet = true;
    }
    
    public void setSprintSpeed(float newSprintSpeed) { sprintSpeed = newSprintSpeed; }
    public void setLineOfSightDistance(float newLOSDist) { LOSDist = newLOSDist; }
    public void setHealth(float damageTaken){currentHealth -= damageTaken; if(currentHealth <= 0) player.GetComponent<PlayerScript>().setScoreVal(1);}
    private void summonMonsters()
    {
        int rand = UnityEngine.Random.Range(0, 2);
        List<UnityEngine.Vector3> spawnPoint = new List<UnityEngine.Vector3>(new UnityEngine.Vector3[3]);
        spawnPoint[0] = new Vector3(0, 0, 0);// player.transform.position;// + new UnityEngine.Vector3(0,1,0);
        spawnPoint[1] = new Vector3(0f, 0, 0);//player.transform.position;// + new UnityEngine.Vector3(1,0,0);
        spawnPoint[2] = new Vector3(0.22f, 0, 0);//player.transform.position;// + new UnityEngine.Vector3(-1,-1,0);
        for(int i = 0; i < 3; i++)
        {
            Instantiate(prefabList[rand], transform.position + spawnPoint[i], UnityEngine.Quaternion.identity);
        }
    }
    private void updatingHealthAndEnergy()
    {
        if (healthBar != null) healthBar.fillAmount = Mathf.Clamp(currentHealth / maxHealth, 0, 100);
        if (energyBar != null) energyBar.fillAmount = Mathf.Clamp(currentEnergy / maxEnergy, 0, 50);
        if (healthBar!= null && currentHealth <= 0) Destroy(gameObject);
    }
    private void sickNPC()
    {
        if(isSiphoning)
        {
            setColor(true);
            agent.speed = 0f;
            currentEnergy = Mathf.Clamp(currentEnergy + 50*Time.deltaTime, 0, 50);
            player.GetComponent<PlayerScript>().setisSick(true);
        }
        else
        {
            setColor(false);
            player.GetComponent<PlayerScript>().setisSick(false);
            agent.speed = sprintSpeed;
            currentEnergy = Mathf.Clamp(currentEnergy - 50*Time.deltaTime, 0, 50);
        }
    }
    private void setColor(bool mode)
    {
        if (mode)
            currentColor.a = Mathf.Clamp(currentColor.a - (findNumber*Time.deltaTime), 0, 1);
        else
            currentColor.a = Mathf.Clamp(currentColor.a + (findNumber*Time.deltaTime), 0, 1);
        spriteRenderer.color = currentColor;
    }
}
