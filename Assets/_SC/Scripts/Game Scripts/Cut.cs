using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Cut : MonoBehaviour
{
    public enum ObstacleType
    {
        Giyotin,
        Saw,
        Punchs,
        None,
    }
    public ObstacleType obstacleType;


    [SerializeField]
    private GameObject objectToInstantiate;

    private bool canMove = true;

    private void Update()
    {
        if(obstacleType == ObstacleType.Giyotin && canMove)
        {
            StartCoroutine(giyotinMovement());
            canMove = false;
        }
        if(obstacleType == ObstacleType.Punchs)
        {
            gameObject.transform.GetChild(0).transform.Rotate(new Vector3(0, 500, 0) * Time.deltaTime);
            if (canMove)
            {
                StartCoroutine(punchMovement());
                canMove = false;
            }
        }

        if(obstacleType == ObstacleType.Saw)
        {
            gameObject.transform.Rotate(new Vector3(0, 0, 500) * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "HairCut")
        {
            gameObject.GetComponent<MeshCollider>().enabled = false;
            Debug.Log(other.gameObject.name);
            int a = int.Parse(other.gameObject.name);
            for (int i = 0; i < HairController.Instance.HairList.Count; i++)
            {
                if (i < a)
                {
                    HairController.Instance.HairList[i].SetActive(true);
                }
                else
                {
                    HairController.Instance.HairList[i].SetActive(false);
                }
            }
            GameObject go = Instantiate(objectToInstantiate, transform.position, transform.rotation, null);
            DynamicBone db = go.GetComponent<DynamicBone>();
            db.m_Gravity = new Vector3(0, -0.25f, 0);
            go.transform.DOMoveY(-.5f, 0.2f);
            go.transform.DORotate(new Vector3(90, 0, 0), 0.2f);
            for (int j = 0; j <= HairController.Instance.HairIndexer+1; j++)
            {
                if (j < a+2 && go.transform.GetChild(j).gameObject.GetComponent<SkinnedMeshRenderer>())
                {
                    go.transform.GetChild(j).gameObject.SetActive(false);
                }
                else
                {
                    go.transform.GetChild(j).gameObject.SetActive(true);
                }
            }
            HairController.Instance.HairIndexer = a;

        }
    }

    IEnumerator giyotinMovement()
    {
        gameObject.transform.DOMoveY(3f, 0.75f);
        yield return new WaitForSeconds(0.75f);
        gameObject.transform.DOMoveY(-.5f, 0.75f);
        yield return new WaitForSeconds(0.75f);
        StartCoroutine(giyotinMovement());
    }
    IEnumerator punchMovement()
    {
        gameObject.transform.DOMoveX(-4f, 0.75f);
        yield return new WaitForSeconds(.75f);
        gameObject.transform.DOMoveX(-8.5f, .75f);
        yield return new WaitForSeconds(.75f);
        StartCoroutine(punchMovement());
    }
}
