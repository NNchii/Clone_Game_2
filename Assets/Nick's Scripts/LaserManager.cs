using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserManager : MonoBehaviour
{
    public List<GameObject> lasers;
    private PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(playerController.transform.position.x + 4f, transform.position.y, 0);
    }

    public void ActivateLasers()
    {
        int randNum = Random.Range(1, 5);
        
        if(randNum == 1)
        {
            lasers[0].transform.gameObject.SetActive(true);
            lasers[1].transform.gameObject.SetActive(true);
        }else if (randNum == 2)
        {
            lasers[3].transform.gameObject.SetActive(true);
            lasers[4].transform.gameObject.SetActive(true);
        }
        else if (randNum == 3)
        {
            lasers[0].transform.gameObject.SetActive(true);
            lasers[4].transform.gameObject.SetActive(true);
        }
        else if (randNum == 2)
        {
            lasers[0].transform.gameObject.SetActive(true);
            lasers[1].transform.gameObject.SetActive(true);
            lasers[2].transform.gameObject.SetActive(true);
        }
        else if (randNum == 2)
        {
            lasers[2].transform.gameObject.SetActive(true);
            lasers[3].transform.gameObject.SetActive(true);
            lasers[4].transform.gameObject.SetActive(true);
        }
        StartCoroutine(Timer());
    }

    public void DeactivateLasers()
    {
        for (int i = 0; i <= lasers.Count - 1; i++)
        {
            lasers[i].transform.GetChild(0).GetComponent<Laser>().laserTurnOff();
            lasers[i].transform.gameObject.SetActive(false);
        }
    }

    public IEnumerator Timer()
    {
        yield return new WaitForSeconds(5);
        DeactivateLasers();
    }
}
