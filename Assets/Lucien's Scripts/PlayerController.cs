using System.Collections;
using TMPro;
using UnityEngine;

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
    //private float rocketBoostDuration = 5.0f;
    public float rocketBoostForce = 10.0f;

    public GameObject[] powerUpPrefabs; // Array of power-up prefabs

    private float nextPowerUpDistance = 30.0f;

    private float speedBoostTimer = 0.0f; // Timer for speed boost
    private float shieldTimer = 0.0f; // Timer for shield
    private float rocketBoostTimer = 0.0f; // Timer for rocket boost

    public ShopController shopController;

    public Transform respawnPoint;
    private bool deathCoroutineStarted = false;

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
            if (!deathCoroutineStarted)
            {
                StartCoroutine(Death());
                deathCoroutineStarted = true;
                jetpackActive = false;
            }
        }
        if (speedBoostActive)
        {
            speedBoostTimer -= Time.deltaTime;
            if (speedBoostTimer <= 0)
            {
                DeactivateSpeedBoost();
            }
        }

        if (shieldActive)
        {
            shieldTimer -= Time.deltaTime;
            if (shieldTimer <= 0)
            {
                DeactivateShield();
            }
        }

        if (rocketBoostActive)
        {
            rocketBoostTimer -= Time.deltaTime;
            if (rocketBoostTimer <= 0)
            {
                DeactivateRocketBoost();
            }
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
            distanceText.text = "Distance: " + Mathf.FloorToInt(distanceTraveled) + " m";
        }

        // Check if it's time to spawn a power-up
        if (distanceTraveled >= nextPowerUpDistance)
        {
            // Spawn a random power-up
            SpawnPowerUp();

            // Update the distance for the next power-up spawn
            nextPowerUpDistance = Mathf.FloorToInt(distanceTraveled) + 30.0f; // Increment by 30 meters
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
        isDead = true;  // Set isDead to true
        shopController.OpenShop();
    }

    public void SetIsDead(bool temp)
    {
        isDead = temp;
    }

    public void ResetPlayer()
    {
        Debug.Log("ResetPlayer called");

        // Reset player position to a Transform object's position
        transform.position = respawnPoint.position;

        // Reset player state
        isDead = false;
        movement = true;

        // Reset other gameplay elements if needed
        distanceTraveled = 0.0f;
        distanceText.text = "Distance: 0 m";
        deathCoroutineStarted = false;
    }
    public void ToggleGameplay(bool isActive)
    {
        Debug.Log("ToggleGameplay called with: " + isActive);
        movement = isActive;
        // Add any other gameplay elements you want to pause here
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
            ActivateSpeedBoost();
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Shield"))
        {
            ActivateShield();
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("RocketBoost"))
        {
            ActivateRocketBoost();
            Destroy(other.gameObject);
        }
    }

    // Method to deactivate rocket boost
    private void ActivateSpeedBoost()
    {
        forwardMovementSpeed *= 1.5f; // Increase speed by 50%
        speedBoostActive = true;
        speedBoostTimer = 5.0f; // Set duration
    }

    private void DeactivateSpeedBoost()
    {
        forwardMovementSpeed /= 1.5f; // Reset speed
        speedBoostActive = false;
    }

    private void ActivateShield()
    {
        shieldActive = true;
        shieldTimer = 10.0f; // Set duration
    }

    private void DeactivateShield()
    {
        shieldActive = false;
    }

    private void ActivateRocketBoost()
    {
        rocketBoostActive = true;
        rocketBoostTimer = 5.0f; // Set duration
        forwardMovementSpeed *= 1.1f; // Increase speed by 10%
        jetpackActive = true; // Keep the player flying
    }

    private void DeactivateRocketBoost()
    {
        rocketBoostActive = false;
        forwardMovementSpeed /= 1.1f; // Reset speed
        jetpackActive = false; // Allow the player to fall
    }

    private void SpawnPowerUp()
    {
        // Choose a random power-up prefab
        GameObject powerUpPrefab = powerUpPrefabs[Random.Range(0, powerUpPrefabs.Length)];

        // Calculate the spawn position just in front of the player, out of view
        float randomY = Random.Range(-4.0f, 4.0f); // Random Y-coordinate between -4.93 and 4.92
        Vector3 spawnPosition = new Vector3(transform.position.x + 20.0f, randomY, transform.position.z);

        // Instantiate the power-up
        Instantiate(powerUpPrefab, spawnPosition, Quaternion.identity);
    }

    // Add a public method to get and set the coin count
    public int GetCoinsCollected()
    {
        return coinsCollected;
    }

    public void SetCoinsCollected(int newCount)
    {
        coinsCollected = newCount;
        coinText.text = "Coins: " + coinsCollected; // Update the UI
    }
}