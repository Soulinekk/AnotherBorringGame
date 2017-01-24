using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class capsuleplayer : MonoBehaviour {

    //GameObjects
    public Transform cam;
    
    //Input
    private float horizontal;
    private float vertical;

    //Movement
    public bool canMove = true;
    private Rigidbody rb;
    [SerializeField] private float turnSpeed = 5;
    [SerializeField] private float speed = 1f;
    private Vector3 storeDirection;
    private Vector3 directionPosition;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

	void Start () {
	
	}
	
	void Update () {
        HandleInput();

    }

    void FixedUpdate()
    {
        MovementCloseNormal();

    }

    void HandleInput()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        storeDirection = cam.right;
    }

    void MovementCloseNormal()
    {
        Vector3 dirForward = storeDirection * vertical;
        Vector3 dirSides = cam.forward * -horizontal;
        if (canMove)
        {
            rb.AddForce((dirForward + dirSides).normalized * speed / Time.deltaTime);
        }

        directionPosition = transform.position + (storeDirection * horizontal) + (cam.forward * vertical);
        Vector3 dir = directionPosition - transform.position;
        dir.y = 0;
        float angle = Vector3.Angle(transform.forward, dir);
        //float animValue = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
        //animValue = Mathf.Clamp01(animValue);

        if (horizontal != 0 || vertical != 0)
        {
            if (angle != 0 && canMove)
            {
                rb.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), turnSpeed + Time.deltaTime);
            }
        }
    }
}
