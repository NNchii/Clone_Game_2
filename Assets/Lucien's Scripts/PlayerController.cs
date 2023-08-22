using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float jetpackForce = 75.0f; 
    public float forwardMovementSpeed = 5.0f; 

    private Rigidbody2D playerRigidbody; 
    private bool jetpackActive = false; 

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
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

    void FixedUpdate()
    {
        // Apply forward movement
        Vector2 newVelocity = playerRigidbody.velocity;
        newVelocity.x = forwardMovementSpeed;
        playerRigidbody.velocity = newVelocity;

        // Apply the jetpack's upward force
        if (jetpackActive)
        {
            playerRigidbody.AddForce(new Vector2(0, jetpackForce));
        }
    }
}
