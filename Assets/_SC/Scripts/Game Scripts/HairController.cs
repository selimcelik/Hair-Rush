using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HairController : MonoBehaviour
{
    public static HairController Instance;

    public List<GameObject> HairList = new List<GameObject>();

    public int HairIndexer = 0;

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (HairIndexer < 0)
        {
            HairIndexer = 0;
        }
    }

    public void AddHair()
    {
        HairList[HairIndexer].SetActive(true);
        HairIndexer++;
    }

    public void RemoveHair()
    {
        HairList[HairIndexer-1].SetActive(false);
        HairIndexer--;
    }
}
