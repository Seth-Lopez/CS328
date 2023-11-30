using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Animations;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine.AI;
using System.Data;
using System.Runtime.CompilerServices;
using UnityEditor;
public class EnemyBehavior : MonoBehaviour
{
    // For Movement: 
    [SerializeField] private float walkingSpeed = 1f;
    private float sprintSpeed = 3f;
    private bool walkPointSet = false;
    private Vector3 destPoint;
    [SerializeField] private float rangePos = 3f;
    [SerializeField] private float waitToMove = 5f;
    [SerializeField] private float timer = 0f;
    //NavMesh:
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform target;
    UnityEngine.AI.NavMeshAgent agent;
    // RigidBody 2D:
    private Rigidbody2D rb;
    // For Health:
    private Image healthBar;
    [SerializeField] private float currentHealth = 100f;
    private float maxHealth = 100;
    // For Energy:
    private Image energyBar;
    [SerializeField] private float currentEnergy = 50;
    private float maxEnergy = 50;
    //Line Of Sight
    private GameObject player;
    private bool hasLOS = false;
    [SerializeField] private float LOSDist = 3f;
    private void Start()
    {
        //Set RigidBody:
        rb = GetComponent<Rigidbody2D>();
        //Set Player Object:
        player = GameObject.FindGameObjectWithTag("Player");
        //Get Health Bar Component:
        healthBar = this.GetComponentInChildren<Image>();
        //Get Energy Bar Component:
        //energyBarGameObject = GameObject.FindWithTag("EnergyBarEnemy");
        //if(energyBarGameObject != null) energyBar = energyBarGameObject.GetComponent<Image>();
        //Set Variables:
        currentHealth = maxHealth;
        currentEnergy = maxEnergy;
        //Set NavMesh
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        //Line Of Sight
    }

    private void Update()
    {
        updatingHealthAndEnergy();
        updatingMovement();
    }
    private void FixedUpdate()
    {
        
        lineOfSight();
    }
    private void updatingMovement() 
    { 
        if(hasLOS && player.transform.position.x - transform.position.x <= LOSDist && player.transform.position.y - transform.position.y <= LOSDist)
        {
            agent.speed = sprintSpeed;
            agent.SetDestination(target.position);
        }
        else
        {
            timer += Time.deltaTime;
            if(timer >= waitToMove)
            {
                agent.speed = sprintSpeed;
                if(!walkPointSet) searchForDest();
                if(walkPointSet) {agent.SetDestination(destPoint); }
                if(Vector3.Distance(transform.position, destPoint) < 10f)
                {
                    walkPointSet = false;
                    timer = 0;
                }   
            }
        } 
        
    }
    // Updating Movement Speed: 
    public void setWalkingSpeed(float newWalkingSpeed) { walkingSpeed = newWalkingSpeed; }
    public void setSprintSpeed(float newSprintSpeed) { sprintSpeed = newSprintSpeed; }
    public void setLineOfSightDistance(float newLOSDist) { LOSDist = newLOSDist; }
    public void setHealth(float damageTaken){currentHealth -= damageTaken;}
    private void updatingHealthAndEnergy()
    {
        if (healthBar != null) healthBar.fillAmount = Mathf.Clamp(currentHealth / maxHealth, 0, 100);
        if (energyBar != null) energyBar.fillAmount = Mathf.Clamp(currentEnergy / maxEnergy, 0, 50);
        if (healthBar!= null && currentHealth <= 0) Destroy(gameObject);
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
        destPoint = new Vector3(transform.position.x + x, transform.position.y + y, 0f);
        walkPointSet = true;
    }
    // Recursively set the opacity to 0 for the specified Transform and its children
    void SetOpacityToZeroRecursive(Transform parent)
    {
        SpriteRenderer renderer = parent.GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
            Color color = renderer.color;
            color.a = 0f; // Set alpha (opacity) to 0
            renderer.color = color;
        }
        for (int i = 0; i < parent.childCount; i++) SetOpacityToZeroRecursive(parent.GetChild(i));       
    }
}

