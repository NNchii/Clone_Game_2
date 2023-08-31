using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private bool temp = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (temp)
        {
            temp = false;
            StartCoroutine(laserTurnOn());
        }
    }

    public IEnumerator laserTurnOn()
    {
        yield return new WaitForSeconds(2);
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
        gameObject.GetComponent<Obstacle>().enabled = true;
    }

    public void laserTurnOff()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        gameObject.GetComponent<Obstacle>().enabled = false;
        temp = true;
    }
}
