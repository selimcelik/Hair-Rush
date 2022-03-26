using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using ManagerActorFramework;
using Cinemachine;

public class PlayerMovement : Actor<LevelManager>
{
    public static PlayerMovement instance;
    //public GameObject explosionParticle;

    public bool levelEnd = false;
    public bool startLevel = false;

    public bool MoveByTouch;
    private Vector3 _mouseStartPos, PlayerStartPos;
    [Range(0f, 100f)] public float maxAcceleration;
    private Vector3 move, direction;
    public Transform target; // the player will look at this object, which is in front of it

    public bool canSwing = false;

    public float speed;

    Animator animator;

    private GameObject parent;


    protected override void MB_Awake()
    {
        instance = this;
        startLevel = false;
    }

    protected override void MB_Start()
    {
        parent = gameObject.transform.parent.gameObject;
        animator = GetComponent<Animator>();
        maxAcceleration = 0.05f;
    }

    protected override void MB_Update()
    {
        if (!levelEnd && startLevel && !canSwing)
        {

            //transform.Rotate(new Vector3(500, 0, 0) * Time.deltaTime);
            transform.position += new Vector3(0, 0f, 1f) * Time.deltaTime * speed;

            if (Input.GetMouseButtonDown(0))
            {
                Plane plane = new Plane(Vector3.up, 0f);

                float Distance;

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (plane.Raycast(ray, out Distance))
                {
                    _mouseStartPos = ray.GetPoint(Distance);
                    PlayerStartPos = transform.position;
                }

                MoveByTouch = true;

            }
            else if (Input.GetMouseButtonUp(0))
            {
                MoveByTouch = false;
            }

            if (MoveByTouch)
            {
                Plane plane = new Plane(Vector3.up, 0f);
                float Distance;

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);


                if (plane.Raycast(ray, out Distance))
                {
                    var newray = ray.GetPoint(Distance);
                    move = newray - _mouseStartPos;
                    var controller = PlayerStartPos + move;

                    controller.x = Mathf.Clamp(controller.x, -3.77f, 3.37f);

                    var TargetNewPos = target.position;

                    TargetNewPos.x = Mathf.MoveTowards(TargetNewPos.x, controller.x, 80f * Time.deltaTime);
                    TargetNewPos.z = Mathf.MoveTowards(TargetNewPos.z, 1000f, 10f * Time.deltaTime);

                    target.position = TargetNewPos;

                    var PlayerNewPos = transform.position;

                    PlayerNewPos.x = Mathf.MoveTowards(PlayerNewPos.x, controller.x, 10 * Time.deltaTime);
                    //PlayerNewPos.z = Mathf.MoveTowards(PlayerNewPos.z, 1000f, 10f * Time.deltaTime);
                    transform.position = PlayerNewPos;
                }
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "swingTrigger")
        {
            if (!canSwing)
            {
                canSwing = true;
                other.gameObject.SetActive(false);
                gameObject.transform.parent = GameObject.FindGameObjectWithTag("swing").transform;
                gameObject.transform.localPosition = new Vector3(0, -2.1f, 0);
                animator.SetBool("CanSit", canSwing);
            }
            else
            {
                canSwing = false;
                other.gameObject.SetActive(false);
                gameObject.transform.parent = parent.transform;
                gameObject.transform.position = new Vector3(0, -1, transform.position.z);
                gameObject.transform.rotation = new Quaternion(0, 0, 0,0);
                animator.SetBool("CanSit", canSwing);
            }

        }

        if(other.gameObject.tag == "levelEnd")
        {
            other.gameObject.SetActive(false);
            levelEnd = true;
            gameObject.transform.DORotate(new Vector3(-90, -180, 0), 0.5f);
            GameObject cam = GameObject.FindGameObjectWithTag("MainCamera");
            CinemachineBrain cinemachineBrain = cam.GetComponent<CinemachineBrain>();
            cinemachineBrain.enabled = false;
            cam.transform.position = new Vector3(0.39f, -2.5f, 248.88f);
            cam.transform.rotation = new Quaternion(0, 180, 0, 0);
            DynamicBonePlaneCollider dbpc = GameObject.FindGameObjectWithTag("podiumPlatform").GetComponent<DynamicBonePlaneCollider>();
            dbpc.m_Center = new Vector3(0, -30, 0);
            dbpc.m_Bound = DynamicBoneColliderBase.Bound.Inside;
            StartCoroutine(levelEndCor());
        }
    }

    IEnumerator levelEndCor()
    {
        yield return new WaitForSeconds(2);
        Push(ManagerEvents.FinishLevel, true);
    }
}