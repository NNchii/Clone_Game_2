using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sections : MonoBehaviour
{
    private LevelManager levelManager;
    private GameObject player;
    private LaserManager laserManager;
    private string lastSection = "";

    private bool timerActive = false;

    // Start is called before the first frame update
    void Start()
    {
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        player = GameObject.Find("Player");
        laserManager = GameObject.Find("Lasers").GetComponent<LaserManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            int temp = Random.Range(0, levelManager.GetSectionCount());

            if(levelManager.GetSectionPrefab(temp).name == lastSection)
            {
                temp--;
            }

            GameObject newSection = Instantiate(levelManager.GetSectionPrefab(temp), new Vector3(transform.parent.position.x + 50, 0, 0), Quaternion.identity);
            lastSection = levelManager.GetSectionPrefab(temp).name;

            levelManager.AddOnScreen(newSection);

            if (levelManager.GetSectionPrefab(temp).tag == "Laser")
            {
                StartCoroutine(SpawnLasers());
            }

            if(levelManager.GetOnScreenCount() >= 3)
            {
                levelManager.RemoveOnScreen(0);
            }
        }
    }

    public IEnumerator SpawnLasers()
    {
        yield return new WaitForSeconds(3);
        laserManager.ActivateLasers();
    }

}
