using UnityEngine;
using System.Collections;


[RequireComponent(typeof(Rigidbody))]
public class Bob : MonoBehaviour
{

    private CameraScript cameraScript;

    public float speed = 6f;

    private Vector3 movement;
    private Animator anim;
    private Rigidbody rb;
    private int floorMask;                      // A layer mask so that a ray can be cast just at gameobjects on the floor layer.
    private float camRayLength = 100f;          // The length of the ray from the camera into the scene.
    public bool distanceView;
    public bool cameraLock = false;
    public Transform mainCam;
    public bool canMove = true;

    public float turnSpeed = 5;
    private Vector3 storeDir;
    private Vector3 directionPos;

    //Movement
    public float inputDelay = 0.1f;
    public float forwarfVel = 12f;
    public GameObject cam;
    public float rpgSpeed = 1;


    void Awake()
    {
        floorMask = LayerMask.GetMask("Floor");

        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        GameObject _cam = GameObject.Find("_Camera");
        cameraScript = (CameraScript)_cam.GetComponent(typeof(CameraScript));
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Move2(h, v);
        float hT = Input.GetAxisRaw("Mouse X");
        float vT = Input.GetAxisRaw("Mouse Y");
        Turning(hT, vT);
    }

    void Move(float h, float v)
    {
        if (cameraScript.cameraSwitch == false)
        {
            if (Mathf.Abs(v) > inputDelay || Mathf.Abs(h) > inputDelay)
            {
                movement.Set(h, 0f, v);
                movement = movement.normalized * speed * Time.deltaTime;
                rb.MovePosition(transform.position + movement);
            }
            else
            {
                rb.velocity = Vector3.zero;
            }
        }
        else
        {
            if (Mathf.Abs(v) > inputDelay || Mathf.Abs(h)>inputDelay)
            {
                /*movement.Set(h, 0f, v);
                movement = movement.normalized * speed * Time.deltaTime;
                rb.MovePosition(transform.position + movement);
                //rb.velocity = transform.right * rightInput * forwarfVel;*/

                transform.position += cam.transform.forward * v * rpgSpeed;
                transform.position += cam.transform.right * h * rpgSpeed;
            }
            else
            {
                rb.velocity = Vector3.zero;
            }
        }
    }
    void Move2(float h, float v)
    {
        Vector3 dirForward = storeDir * h;
        Vector3 dirSides = mainCam.forward * v;
        if (canMove)
        {
            rb.AddForce((dirForward + dirSides).normalized * speed / Time.deltaTime);
        }

        directionPos = transform.position + (storeDir * h) + (mainCam.forward * v);
        Vector3 dir = directionPos - transform.position;
        dir.y = 0;
        float angle = Vector3.Angle(transform.forward, dir);
        float animValue = Mathf.Abs(h) + Mathf.Abs(v);
        animValue = Mathf.Clamp01(animValue);

        //anim.SetFloat("Forward;, animValue);
        //anim.SetBool("LockOn:, false);

        if (h != 0 || v != 0)
        {
            if (angle != 0 && canMove)
            {
                rb.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), turnSpeed + Time.deltaTime);
            }
        }
    }

    void Turning(float h, float v)
    {
        if (cameraScript.cameraSwitch == false)
        {
            // Create a ray from the mouse cursor on screen in the direction of the camera.
            Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            // Create a RaycastHit variable to store information about what was hit by the ray.
            RaycastHit floorHit;

            // Perform the raycast and if it hits something on the floor layer...
            if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask))
            {
                // Create a vector from the player to the point on the floor the raycast from the mouse hit.
                Vector3 playerToMouse = floorHit.point - transform.position;

                // Ensure the vector is entirely along the floor plane.
                playerToMouse.y = 0f;

                // Create a quaternion (rotation) based on looking down the vector from the player to the mouse.
                Quaternion newRotation = Quaternion.LookRotation(playerToMouse);

                // Set the player's rotation to this new rotation.
                rb.MoveRotation(newRotation);
            }
        }
    }

    void Animating(float h, float v)
    {
        // Create a boolean that is true if either of the input axes is non-zero.
        bool walking = h != 0f || v != 0f;

        // Tell the animator whether or not the player is walking.
        anim.SetBool("IsWalking", walking);
    }
}
