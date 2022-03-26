using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingScript : MonoBehaviour
{
    public float rotatespeed = 100f;
    private float _startingPosition;

    private bool canRotate = false;

    private void Start()
    {

    }

    private void Update()
    {
        if (PlayerMovement.instance.canSwing)
        {
            transform.position += new Vector3(0, 0f, 1f) * Time.deltaTime * 3;

            if (Input.GetMouseButtonDown(0))
            {
                _startingPosition = Input.mousePosition.x;
                canRotate = true;
            }

            if (canRotate)
            {
                if (_startingPosition > Input.mousePosition.x)
                {
                    transform.Rotate(Vector3.back, -rotatespeed);
                }
                else if (_startingPosition < Input.mousePosition.x)
                {
                    transform.Rotate(Vector3.back, rotatespeed);
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                canRotate = false;
            }
        }
    }
        
}
