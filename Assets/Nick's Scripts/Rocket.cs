using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    public float moveSpeed = 10f;
    private PlayerController playerController;
    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(WarningIcon());
        transform.parent.gameObject.transform.GetChild(1).position = new Vector3(playerController.transform.position.x + 12, transform.parent.gameObject.transform.GetChild(1).position.y, 0);
        Vector3 newPosition = transform.position;
        newPosition.x -= moveSpeed * Time.deltaTime;
        transform.parent.position = newPosition;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerController.SetIsDead(true);
            Destroy(transform.parent.gameObject);
        }
    }

    public IEnumerator WarningIcon()
    {
        yield return new WaitForSeconds(2);
        transform.parent.gameObject.transform.GetChild(1).gameObject.SetActive(false);
    }
}
