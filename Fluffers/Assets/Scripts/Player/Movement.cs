using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float TestVar;
    public float speed;
    public Animator anim;

    private Vector3 orgScale;
    private Vector3 flippedScale;

    Rigidbody2D rb;
    public float jumpPower;

    public bool grounded = false;

    public Transform jumpPosition;
    public Transform backPosition;
    public Transform frontPosition;
    public Transform hangingPosition;

    public float gravityMuliplier;
    Vector3 MovementInput;

    public bool facingRight = true;
    private PushPull pushPull;
    public bool climbing = false;
    private bool justGrounded = false;

    // Start is called before the first frame update
    void Start()
    {
        orgScale = transform.localScale;
        flippedScale = new Vector3(-orgScale.x, orgScale.y, orgScale.z);
        anim.SetFloat("facingRight", 1);
        rb = GetComponent<Rigidbody2D>();
        pushPull = GetComponent<PushPull>();
    }



    // Update is called once per frame
    void Update()
    {
        if(!climbing)
        {
            MovementInput = new Vector3(Input.GetAxis("MoveHorizontal"), 0, 0.0f);

            transform.position = transform.position + MovementInput * Time.deltaTime * speed;

            anim.SetFloat("speed", MovementInput.x);

            //Reverse Scale
            if (MovementInput.x < -.05f && !pushPull.grabbing)
            {
                transform.localScale = flippedScale;
                facingRight = false;
                anim.SetFloat("facingRight", 0);
            } else if (MovementInput.x > .05f && !pushPull.grabbing)
            {
                transform.localScale = orgScale;
                facingRight = true;
                anim.SetFloat("facingRight", 1);
            }
        

            //Jump
            if(Input.GetButtonDown("A_Button") && grounded)
            {
                anim.SetTrigger("jump");
                rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            }

            //Gravity Multiplier for falling
            if(rb.velocity.y < 0)
            {
                rb.AddForce(Vector2.down * gravityMuliplier);
            }
        }

    }

    IEnumerator GroundedReset()
    {
        justGrounded = true;
        yield return new WaitForSeconds(0.1f);
        justGrounded = false;
    }

    private void FixedUpdate()
    {      
        //Middle Position
        RaycastHit2D hit = Physics2D.Raycast(jumpPosition.position, -Vector2.up, 1);

        //Back Position Test
        RaycastHit2D backHit = Physics2D.Raycast(backPosition.position, -Vector2.up, 1);

        //Front Position Test
        RaycastHit2D frontHit= Physics2D.Raycast(frontPosition.position, -Vector2.up, 1);

        //Hanging Position Test
        RaycastHit2D hangingHit = Physics2D.Raycast(hangingPosition.position, -Vector2.up, 2f);


        //Grounded Testing
        if (hit.collider == null && backHit.collider == null && frontHit.collider == null && justGrounded == false)
        {
            grounded = false;
            anim.SetBool("grounded", false);
            StartCoroutine(GroundedReset());
        }
        else if (hit.collider != null && backHit.collider != null && frontHit.collider != null)
        {
            grounded = true;
            anim.SetBool("grounded", true);
        }
        else if (grounded == false && (hit.collider != null || backHit.collider != null || frontHit.collider != null))
        {
            grounded = true;
            anim.SetBool("grounded", true);
        }



        //Almost Falling
        if (hit.collider == null && backHit.collider != null && frontHit.collider == null && MovementInput.x == 0)
        {
            anim.SetBool("fallingForward", true);
        }
        else if (hit.collider == null && backHit.collider == null && frontHit.collider != null && MovementInput.x == 0)
        {
            anim.SetBool("fallingBackward", true);
        }
        else
        {
            anim.SetBool("fallingForward", false);
            anim.SetBool("fallingBackward", false);
        }

        //Hanging Testing
        if (hangingHit.collider != null)
            print(hangingHit.collider.name + " and " + rb.velocity + " and " + climbing);

        if(hangingHit.collider != null && rb.velocity.y < 0 && !climbing)
        {            
            StartCoroutine(ClimbingRoutine(hangingHit.point));            
        }
    }

    IEnumerator ClimbingRoutine(Vector3 landingPosition)
    {
        anim.SetTrigger("climbUp");
        climbing = true;
        rb.velocity = Vector3.zero;
        rb.isKinematic = true;

        yield return new WaitForSeconds(1.7f);

        rb.isKinematic = false;
        climbing = false;
    }
}
