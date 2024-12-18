using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private float yaw, pitch;
    private Rigidbody rb;
    public float speed, sensitivity;
    [SerializeField] private float walk_speed, run_speed, jumpforce;
    [SerializeField] private Animator fps_anim;
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rb = gameObject.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        //Camera Controller
        pitch -= Input.GetAxisRaw("Mouse Y") * sensitivity;
        pitch = Mathf.Clamp(pitch, -90, 90);
        yaw += Input.GetAxisRaw("Mouse X") * sensitivity;
        Camera.main.transform.localRotation = Quaternion.Euler(pitch, yaw, 0);

        //Jump
        if (Input.GetKeyDown(KeyCode.Space) && Physics.Raycast(rb.transform.position, Vector3.down, 1.1f))
        {
            rb.AddForce(transform.up * (jumpforce*100));
        }
        Sprint();

        if (Input.GetKeyDown(KeyCode.F)){
            fps_anim.SetTrigger("Inspect");
        }
    }

    private void FixedUpdate()
    {
        //Movement
        Vector2 axis = new Vector2(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal")) * speed;
        Vector3 forward = new Vector3(-Camera.main.transform.right.z, 0, Camera.main.transform.right.x);
        Vector3 wishDirection = (forward * axis.x + Camera.main.transform.right * axis.y + Vector3.up * rb.linearVelocity.y);
        rb.linearVelocity = wishDirection;
        //Debug.Log(axis.x);
        if(axis.x>run_speed-1){
            //Debug.Log("what");
            fps_anim.SetBool("Running", true);
            fps_anim.SetBool("Walking", false);
        }
        else if (axis.x>walk_speed-1){
            fps_anim.SetBool("Walking", true);
            fps_anim.SetBool("Running", false);
        }
        else{
            fps_anim.SetBool("Running", false);
            fps_anim.SetBool("Walking", false);
        }
    }

    void Sprint(){
        if (Input.GetKey(KeyCode.LeftShift)){
            speed = run_speed;
        }
        else{
            speed = walk_speed;
        }
    }
}