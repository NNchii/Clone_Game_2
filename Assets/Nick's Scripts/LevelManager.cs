using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    public List<GameObject> sections;

    public List<GameObject> onScreen;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject GetSectionPrefab(int element)
    {
        return sections[element];
    }

    public int GetSectionCount()
    {
        return sections.Count;
    }

    public void AddOnScreen(GameObject temp)
    {
        onScreen.Add(temp);
    }
    public void RemoveOnScreen(int num)
    {
        Destroy(onScreen[num]);
        onScreen.RemoveAt(num);
    }

    public int GetOnScreenCount()
    {
        return onScreen.Count;
    }


}
