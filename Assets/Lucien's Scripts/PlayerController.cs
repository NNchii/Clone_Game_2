using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float jetpackForce; 
    public float forwardMovementSpeed; 

    private Rigidbody2D playerRigidbody; 
    private bool jetpackActive = false;

    // bool for when mans dies
    private bool isDead = false;
    private bool movement = true;

    private float distanceTraveled = 0.0f;
    private int coinsCollected = 0;

    public TMP_Text distanceText;
    public TMP_Text coinText;

    private bool speedBoostActive = false;
    private bool shieldActive = false;
    private bool rocketBoostActive = false;
    private float rocketBoostDuration = 5.0f;
    public float rocketBoostForce = 10.0f;

    public GameObject[] powerUpPrefabs; // Array of power-up prefabs

    private float nextPowerUpDistance = 1000.0f;

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // if statement to check if mans is dead
        if (!isDead)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                jetpackActive = true;
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                jetpackActive = false;
            }
        }
        else
        {
            StartCoroutine(Death());
            jetpackActive = false;
        }
        
    }

    void FixedUpdate()
    {
        // if player is alive and moving 
        if (movement)
        {
            // Apply forward movement
            Vector2 newVelocity = playerRigidbody.velocity;
            newVelocity.x = forwardMovementSpeed;
            playerRigidbody.velocity = newVelocity;

            // Update the distance traveled
            distanceTraveled += forwardMovementSpeed * Time.fixedDeltaTime;

            if (Mathf.FloorToInt(distanceTraveled) % 100 == 0 && distanceTraveled != 0)
            {
                // Increase the forward movement speed
                forwardMovementSpeed += 0.50f;
            }

            distanceText.text = "Distance: " + Mathf.FloorToInt(distanceTraveled) + " m";
        }

        if (distanceTraveled >= nextPowerUpDistance)
        {
            // Spawn a random power-up
            SpawnPowerUp();

            // Update the distance for the next power-up spawn
            nextPowerUpDistance += 1000.0f;
        }

        // Apply the jetpack's upward force
        if (jetpackActive)
        {
            playerRigidbody.AddForce(new Vector2(0, jetpackForce));
        }

        coinText.text = "Coins: " + coinsCollected;

        if (rocketBoostActive)
        {
            // Apply additional forward force
            playerRigidbody.AddForce(new Vector2(rocketBoostForce, 0));
        }
    }

    // timer to stop movement after player death
    public IEnumerator Death()
    {
        yield return new WaitForSeconds(2);
        movement = false;
    }


    public void SetIsDead(bool temp)
    {
        isDead = temp;
    }

    // Method to get the distance traveled
    public float GetDistanceTraveled()
    {
        return distanceTraveled;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Coin"))
        {
            // Increment the coin counter
            coinsCollected++;

            // Optionally, destroy the coin object
            Destroy(other.gameObject);
        }

        // Check for power-ups
        if (other.CompareTag("SpeedBoost"))
        {
            // Activate speed boost
            forwardMovementSpeed *= 2; // Double the speed
            speedBoostActive = true;
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Shield"))
        {
            // Activate shield
            shieldActive = true;
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("RocketBoost"))
        {
            // Activate rocket boost
            rocketBoostActive = true;
            Destroy(other.gameObject);

            // Deactivate rocket boost after a duration
            Invoke("DeactivateRocketBoost", rocketBoostDuration);
        }
    }

    // Method to deactivate rocket boost
    private void DeactivateRocketBoost()
    {
        rocketBoostActive = false;
    }

    private void SpawnPowerUp()
    {
        // Choose a random power-up prefab
        GameObject powerUpPrefab = powerUpPrefabs[Random.Range(0, powerUpPrefabs.Length)];

        // Calculate the spawn position just in front of the player, out of view
        Vector3 spawnPosition = transform.position + new Vector3(20.0f, Random.Range(-5.0f, 5.0f), 0);

        // Instantiate the power-up
        Instantiate(powerUpPrefab, spawnPosition, Quaternion.identity);
    }
}


