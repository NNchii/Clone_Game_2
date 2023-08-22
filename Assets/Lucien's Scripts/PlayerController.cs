using System.Collections;
using System.Collections.Generic;
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
        }

        // Apply the jetpack's upward force
        if (jetpackActive)
        {
            playerRigidbody.AddForce(new Vector2(0, jetpackForce));
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
}

    
