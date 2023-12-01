using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    // For Movement: 
    [SerializeField] private float walkingSpeed = 5f;
    [SerializeField] private float sprintSpeed;
    private float currentMovementSpeed;
    private UnityEngine.Vector2 movementDirection;
    private GameObject vCamera;
    // RigidBody 2D:
    private Rigidbody2D rb;
    // For Health:
    private GameObject healthBarGameObject;
    private Image healthBar;
    [SerializeField] private float currentHealth = 100f;
    private float maxHealth = 100;
    // For Energy:
    private GameObject energyBarGameObject;
    private Image energyBar;
    [SerializeField] private float currentEnergy = 50;
    private float maxEnergy = 50;
    // For Combat
    [SerializeField] private GameObject projPrefab;
    [SerializeField] private Transform launchOffset;
    [SerializeField] private int spellSelected;
    private float tempPowerUp = 5f;
    // For ScoreBoard
    [SerializeField] private TMPro.TMP_Text scoreVal;
    private int currentScore = 0;
    private void Start()
    {
        // Set RigidBody:
        rb = GetComponent<Rigidbody2D>();
        //Get Health Bar Component:
        healthBarGameObject = GameObject.FindWithTag("HealthBar");
        if(healthBarGameObject != null)
            healthBar = healthBarGameObject.GetComponent<Image>();
        //Get Energy Bar Component:
        energyBarGameObject = GameObject.FindWithTag("EnergyBar");
        if(energyBarGameObject != null)
            energyBar = energyBarGameObject.GetComponent<Image>();
        //Set Variables:
        currentMovementSpeed = walkingSpeed;
        currentHealth =  maxHealth;
        currentEnergy = maxEnergy;
    }

    private void Update()
    {
        updatingMovement();
        updatingHealthAndEnergy();
        updatingFacingDirection();
        updatingProjectile();  
    }
    private void FixedUpdate()
    {
        rb.velocity = movementDirection * currentMovementSpeed;
    }

    // Updating Movement Speed: 
    public void setWalkingSpeed(float newWalkingSpeed){walkingSpeed = newWalkingSpeed;}
    public void setSprintSpeed(float newSprintSpeed){sprintSpeed = newSprintSpeed;}
    public void setScoreVal(int newValue){ currentScore += newValue; scoreVal.text = "" + currentScore;}
    public float getCurrentSpeed(){return currentMovementSpeed;}
    private void updatingMovement()
    {
        movementDirection = new UnityEngine.Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        sprintSpeed = walkingSpeed + 5f;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if(currentEnergy != 0)
                currentMovementSpeed = sprintSpeed;
            else
                currentMovementSpeed = walkingSpeed;
            currentEnergy -= 50*Time.deltaTime;
        }
        else
        {
            currentMovementSpeed = walkingSpeed;
            currentEnergy += 10*Time.deltaTime;
        }
        currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);
        
        if(movementDirection != null)
        {
            if(movementDirection.x == 0 && movementDirection.y == 0) currentMovementSpeed = 0;
        }
    }
    private void updatingFacingDirection()
    {
        vCamera = GameObject.FindWithTag("Camera");
        UnityEngine.Vector3 mousePosition = vCamera.GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;
        if(Input.GetButtonDown("Fire1"))
        {
            if(transform.position.x > mousePosition.x)
            {
                transform.rotation = UnityEngine.Quaternion.Euler(0f, 180f, 0f);
            }
            else
            {
                transform.rotation = UnityEngine.Quaternion.Euler(0f, 0f, 0f);
            }
        }
        else
        {
            UnityEngine.Vector2 dir = new UnityEngine.Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            if(dir.x > 0) transform.rotation = UnityEngine.Quaternion.Euler(0f, 0f, 0f);
            if(dir.x < 0) transform.rotation = UnityEngine.Quaternion.Euler(0f, 180f, 0f);
        }
        
    }
    public void setHealth(float damageTaken){currentHealth -= damageTaken;}
    private void updatingHealthAndEnergy()
    {
        if(healthBar != null && energyBar != null)
        {
            healthBar.fillAmount = Mathf.Clamp(currentHealth / maxHealth, 0, 100);
            energyBar.fillAmount = Mathf.Clamp(currentEnergy / maxEnergy, 0, 50);
        }
        if(currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
    private void updatingProjectile()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            GameObject instantiatedObject = Instantiate(projPrefab, launchOffset.position, transform.rotation);
            instantiatedObject.GetComponent<ProjectileBehaviour>().setIsPlayer();
        }
        if(tempPowerUp != 5f)
        {
            tempPowerUp -= Time.deltaTime;
        }
        if(tempPowerUp <= 0)
        {
            tempPowerUp = 5f;
            spellSelected = 2;
        }
    }
    public int getSpellSelected(){return spellSelected;}
    public void SetSpellSelected(int newValue){spellSelected = newValue; tempPowerUp -= Time.deltaTime;}
}
