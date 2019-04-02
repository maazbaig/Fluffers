using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushPull : MonoBehaviour
{
    public GameObject grabbedObject;
    public GameObject target;
    public bool grabbing;
    
    public Transform grabDetectPos;
    private Movement movementScript;
    

    // Start is called before the first frame update
    void Start()
    {
        movementScript = GetComponent<Movement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("LeftTrigger") > 0.5f)
        {
            grabbing = true;

            if (target != null)
            {
                grabbedObject = target;
            }
        }
        else
        {
            grabbing = false;
        }

        if (grabbedObject != null)
        {
            if (grabbing)
            {
                grabbedObject.transform.parent = transform;
            }             
            else
            {
                grabbedObject.transform.parent = null;
                grabbedObject = null;
            }
               
        }
    }

    private void FixedUpdate()
    {
        RaycastHit2D hit;
        if (movementScript.facingRight)
        {
            hit = Physics2D.Raycast(grabDetectPos.position, transform.right, 3);
        } else
        {
            hit = Physics2D.Raycast(grabDetectPos.position, -transform.right, 3);
        }
         

        if(hit.collider != null)
        {
            target = hit.collider.gameObject;
        } else
        {
            target = null;
        }


    }
}
