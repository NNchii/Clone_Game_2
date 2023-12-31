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

    public bool speedBoostActive = false;
    public bool shieldActive = false;
    public bool rocketBoostActive = false;
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

    private int speedBoostLevel = 0;
    private int shieldLevel = 0;
    private int rocketBoostLevel = 0;

    public int upgradeIncrement = 50;

    private bool rocketCoolDown;

    public GameObject rocketPrefab;
    private SpriteRenderer spriteRenderer;


    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        
        StartCoroutine(RocketWait(Random.Range(5, 15)));

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {

        if (!rocketCoolDown)
        {
            StartCoroutine(RocketWait(Random.Range(5, 15)));
        }
        
        
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
        GetComponent<Collider2D>().enabled = false;
        shopController.OpenShop();
    }

    public void SetIsDead(bool temp)
    {
        isDead = temp;
    }

    public void ResetPlayer()
    {
        //Debug.Log("ResetPlayer called");

        // Reset player position to a Transform object's position
        transform.position = respawnPoint.position;

        // Reset player state
        isDead = false;
        movement = true;
        GetComponent<Collider2D>().enabled = true;

        // Reset other gameplay elements if needed
        distanceTraveled = 0.0f;
        distanceText.text = "Distance: 0 m";
        deathCoroutineStarted = false;
    }
    public void ToggleGameplay(bool isActive)
    {
        //Debug.Log("ToggleGameplay called with: " + isActive);
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
        Debug.Log("Collision detected with: " + other.tag);  // Debug statement

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
            Debug.Log("SpeedBoost activated");  // Debug statement
            ActivateSpeedBoost();
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Shield"))
        {
            Debug.Log("Shield activated");  // Debug statement
            ActivateShield();
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("RocketBoost"))
        {
            Debug.Log("RocketBoost activated");  // Debug statement
            ActivateRocketBoost();
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Obstacle"))  // Assuming the obstacle tag is "Obstacle"
        {
            Debug.Log("Obstacle detected");  // Debug statement

            if (!shieldActive && !rocketBoostActive)  // Check if either power-up is active
            {
                Debug.Log("Player should die now");  // Debug statement
                                                     // Code to kill the player
                isDead = true;
                StartCoroutine(Death());  // Assuming you want to call the Death coroutine here
            }
            else
            {
                Debug.Log("Player has an active power-up, should not die");  // Debug statement
            }
        }
    }


    // Method to deactivate rocket boost
    private void ActivateSpeedBoost()
    {
        forwardMovementSpeed *= (1.5f + 0.1f * speedBoostLevel);
        speedBoostTimer = 5.0f + speedBoostLevel;
        speedBoostActive = true;
        spriteRenderer.color = Color.green;  // Set the player color to green
    }

    private void DeactivateSpeedBoost()
    {
        forwardMovementSpeed /= 1.5f; // Reset speed
        speedBoostActive = false;
        spriteRenderer.color = Color.white;  // Reset the player color to white
    }

    private void ActivateShield()
    {
        shieldActive = true;
        shieldTimer = 10.0f + shieldLevel;
        spriteRenderer.color = Color.blue;  // Set the player color to blue
    }

    private void DeactivateShield()
    {
        shieldActive = false;
        spriteRenderer.color = Color.white;  // Reset the player color to white
    }

    private void ActivateRocketBoost()
    {
        rocketBoostActive = true;
        rocketBoostTimer = 5.0f + rocketBoostLevel;
        forwardMovementSpeed *= (1.1f + 0.05f * rocketBoostLevel);
        jetpackActive = true;
        spriteRenderer.color = new Color(1.0f, 0.5f, 0.0f);  // Set the player color to orange
    }

    private void DeactivateRocketBoost()
    {
        rocketBoostActive = false;
        forwardMovementSpeed /= 1.1f; // Reset speed
        jetpackActive = false; // Allow the player to fall
        spriteRenderer.color = Color.white;  // Reset the player color to white
    }

    public int GetSpeedBoostLevel()
    {
        return speedBoostLevel;
    }

    public int GetShieldLevel()
    {
        return shieldLevel;
    }

    public int GetRocketBoostLevel()
    {
        return rocketBoostLevel;
    }

    public bool CanAffordUpgrade(int currentLevel)
    {
        int upgradeCost = (currentLevel + 1) * upgradeIncrement;
        return coinsCollected >= upgradeCost;
    }

    public void UpgradeSpeedBoost()
    {
        int upgradeCost = (speedBoostLevel + 1) * upgradeIncrement;
        if (CanAffordUpgrade(speedBoostLevel))
        {
            coinsCollected -= upgradeCost;
            speedBoostLevel++;
            coinText.text = "Coins: " + coinsCollected;
        }
    }

    public void UpgradeShield()
    {
        int upgradeCost = (shieldLevel + 1) * upgradeIncrement;
        if (coinsCollected >= upgradeCost)
        {
            coinsCollected -= upgradeCost;
            shieldLevel++;
            coinText.text = "Coins: " + coinsCollected;
        }
    }

    public void UpgradeRocketBoost()
    {
        int upgradeCost = (rocketBoostLevel + 1) * upgradeIncrement;
        if (coinsCollected >= upgradeCost)
        {
            coinsCollected -= upgradeCost;
            rocketBoostLevel++;
            coinText.text = "Coins: " + coinsCollected;
        }
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

    public IEnumerator RocketWait(int num)
    {
        rocketCoolDown = true;
        yield return new WaitForSeconds(num);

        if (Random.Range(1, 10) >= 7)
        {
            for (int i = 0; i <= Random.Range(1, 3); i++)
            {
                float randomY = Random.Range(-4.0f, 4.0f); // Random Y-coordinate between -4.93 and 4.92
                Vector3 spawnPosition = new Vector3(transform.position.x + 40.0f, randomY, transform.position.z);

                GameObject rocket = Instantiate(rocketPrefab, spawnPosition, Quaternion.identity);
                Destroy(rocket, 10);
            }
        }
        else
        {
            float randomY = Random.Range(-4.0f, 4.0f); // Random Y-coordinate between -4.93 and 4.92
            Vector3 spawnPosition = new Vector3(transform.position.x + 40.0f, randomY, transform.position.z);

            GameObject rocket = Instantiate(rocketPrefab, spawnPosition, Quaternion.identity);
            Destroy(rocket, 10);
        }
        rocketCoolDown = false;
    }
}