using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FlameCircleMgr : MonoBehaviour
{
    public GameObject childPrefab;
    public GameObject projectilePrefab; // Assign the projectile prefab in the Inspector
    private int numberOfChildren = 5;
    private float projectileSpeed = 3f; // Adjust as needed

    private List<Transform> childObjects = new List<Transform>();
    private List<Vector3> childPositions = new List<Vector3>();

    private float counter = 1f;
    private int firedelay = 0;
    private float firedelayCounter = 10f;

    private Vector3 previousParentPosition;

    void Start()
    {
        if (childPrefab == null || projectilePrefab == null)
        {
            Debug.LogWarning("Child prefab or projectile prefab not assigned.");
            return;
        }

        InstantiateChildObjects();
        StoreChildPositions();
        RotateChildObjects();

        previousParentPosition = transform.position;
    }
    void Update()
    {
        shoot();

        // Calculate the parent's movement since the last frame
        Vector3 parentMovement = transform.position - previousParentPosition;

        // Update child positions based on parent's movement
        UpdateChildPositions(parentMovement);

        // Update previousParentPosition for the next frame
        previousParentPosition = transform.position;
    }

    void UpdateChildPositions(Vector3 parentMovement)
    {
        for (int i = 0; i < childObjects.Count; i++)
        {
            // Update child positions by adding the parent's movement
            childPositions[i] += parentMovement;
            childObjects[i].position = childPositions[i];
        }
    }

    private void shoot()
    {
        if(firedelay != 3)
        {
            if(counter == 1f)
            {
                SpawnProjectilesFromChildPositions();
            }
            counter -= Time.deltaTime;
            if(counter < 0)
            {
                counter = 1f;
                firedelay += 1;
            }
        }
        else
        {
            firedelayCounter -= Time.deltaTime;
            if(firedelayCounter < 0)
            {
                firedelayCounter = 10;
                firedelay = 0;
            }
        }
    }
    void InstantiateChildObjects()
    {
        float angleStep = 360f / numberOfChildren;
        float radius = 1.5f; // You can adjust this to change the distance from the parent's center

        for (int i = 0; i < numberOfChildren; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            Vector3 newPos = new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0f);
            GameObject newChild = Instantiate(childPrefab, transform.position + newPos, Quaternion.identity, transform);
            childObjects.Add(newChild.transform);
        }
    }

    void StoreChildPositions()
    {
        foreach (Transform child in childObjects)
        {
            childPositions.Add(child.position);
        }
    }

    void RotateChildObjects()
    {
        foreach (Transform child in childObjects)
        {
            Vector3 direction = (transform.position - child.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            child.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
    void SpawnProjectilesFromChildPositions()
    {
        foreach (Vector3 pos in childPositions)
        {
            Vector3 direction = (transform.position - pos).normalized;
            Quaternion rotation = Quaternion.LookRotation(Vector3.forward, direction);
            GameObject newProjectile = Instantiate(projectilePrefab, pos, rotation);
            newProjectile.GetComponent<Rigidbody2D>().velocity = direction * projectileSpeed;
            newProjectile.GetComponent<ProjectileBehaviour>().setIsFireStart();
        }
    }
}
