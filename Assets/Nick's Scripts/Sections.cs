using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sections : MonoBehaviour
{
    private LevelManager levelManager;
    private GameObject player;
    
    // Start is called before the first frame update
    void Start()
    {
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        player = GameObject.Find("Player");
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

            GameObject newSection = Instantiate(levelManager.GetSectionPrefab(temp), new Vector3(transform.parent.position.x + 50, 0, 0), Quaternion.identity);
            levelManager.AddOnScreen(newSection);
            if(levelManager.GetOnScreenCount() >= 3)
            {
                levelManager.RemoveOnScreen(0);
            }
        }
    }

}
