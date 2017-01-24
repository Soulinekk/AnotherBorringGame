using UnityEngine;
using System.Collections;

public class camtry : MonoBehaviour {

    //Game Objects
    public GameObject player;
    public GameObject cam;
    public GameObject mainCam;


    Vector3 velocity = Vector3.zero;
    public float dampTime = 0.15f;
    float cameraY = -1;

    //Close camera behaviour
    public float sensitivity = 15F;

    public float minimumY = -60F;
    public float maximumY = 60F;

    public float rotationY = 0F;
    public float rotationX = 0F;


    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        CameraDamp();
        CloseCameraBeh();
    }

    void CameraDamp()
    {
        if (player != null)
        {
            Vector3 destination = new Vector3(player.transform.position.x, player.transform.position.y + cameraY, player.transform.position.z);
            gameObject.transform.position = Vector3.SmoothDamp(gameObject.transform.position, destination, ref velocity, dampTime);
        }
    }

    void CloseCameraBeh()
    {
        rotationY += Input.GetAxis("Mouse Y") * sensitivity;
        rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);
        rotationX += Input.GetAxis("Mouse X") * sensitivity;

        transform.rotation = Quaternion.Euler(0f, rotationX, rotationY);
        //transform.rotation = Quaternion.Euler(0f, 0f, -rotationY);
        //transform.localEulerAngles = new Vector3(0f, transform.localEulerAngles.y, -rotationY);

    }
}
