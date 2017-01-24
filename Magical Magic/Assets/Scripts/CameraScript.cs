using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour
{
    //inspector
    public GameObject playerPrefab;
    public GameObject rotator;
    public GameObject camFollow;

    public float dampTime = 0.15f;
    public float cameraDistance = 10;
    public GameObject mainCam;
    public float timeTakenDuringLerp = 1.0f;


    public bool cameraSwitch = false;


    //
    private Vector3 velocity = Vector3.zero;
    private Transform player;
    
    private float previousTransform;
    private float previousCameraDistance;
    private bool _isLepring;
    private float _timeStartedLerping;
    private Vector3 _startRotatorPosition;
    private Vector3 _startCameraPosition;
    private Vector3 _endRotatorPosition;
    private Vector3 _endCameraPosition;
    private Quaternion _endRotatorRotation;
    private Quaternion _startRotation;
    private Quaternion _previousCameraRotation;
    private Quaternion _startCameraRotation;
    //private Quaternion _startBobRotation;
    //private Quaternion _endBobRotation;

    //Close camera behaviour
    public float sensitivity = 15F;

    public float minimumY = -60F;
    public float maximumY = 60F;

    public float rotationY = 0F;
    public float rotationX = 0F;

    //


    void Start()
    {

        player = GameObject.FindGameObjectWithTag("PlayerOrigin").transform;
        Vector3 destination = new Vector3(player.position.x, player.position.y + cameraDistance, player.position.z);
        transform.position = destination;
        _previousCameraRotation = Quaternion.Euler(47f, 0f, 0f);

    }

    void Update()
    {
        Keys();

        if (cameraSwitch== false)
        {
            DistantCameraScroll();
            DistantCameraClamp();
        }
        else
        {
            CloseCameraBeh();
        }
    }

    void FixedUpdate()
    {
        
        CameraStateSwitch();
        CameraDamp();

    }

    void Keys()
    {
        if (Input.GetKeyUp(KeyCode.Tab) && _isLepring == false)
        {
            if(cameraSwitch == false)
            {
                cameraSwitch = !cameraSwitch;
                previousTransform = rotator.transform.localPosition.y;
                previousCameraDistance = mainCam.transform.localPosition.z;
                StartLerping(0);
            }
            else
            {
                cameraSwitch = !cameraSwitch;
                StartLerping(1);
            }
        }
    }

    void CameraDamp()
    {
        if (player != null)
        {
            Vector3 destination = new Vector3(playerPrefab.transform.position.x, player.position.y + cameraDistance, playerPrefab.transform.position.z);
            camFollow.transform.position = Vector3.SmoothDamp(camFollow.transform.position, destination, ref velocity, dampTime);
        }
    }

    void DistantCameraScroll()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel") * 10;
        Vector3 scrollVector = new Vector3(0.0f, 0.0f, scroll);
        mainCam.transform.localPosition += scrollVector;
     }

    void DistantCameraClamp()
    {
        mainCam.transform.localPosition = new Vector3(mainCam.transform.localPosition.x, mainCam.transform.localPosition.y, Mathf.Clamp(mainCam.transform.localPosition.z, -30.0f, -10.0f));
    }

    void CameraStateSwitch()
    {
        if (_isLepring == true)
        {
            float timeSinceStarted = Time.time - _timeStartedLerping;
            float percentageComplete = timeSinceStarted / timeTakenDuringLerp;

            rotator.transform.localPosition = Vector3.Lerp(_startRotatorPosition, _endRotatorPosition, percentageComplete);
            rotator.transform.rotation = Quaternion.Lerp(_startRotation, _endRotatorRotation, percentageComplete);
            mainCam.transform.localPosition = Vector3.Lerp(_startCameraPosition, _endCameraPosition, percentageComplete);
            transform.rotation = Quaternion.Lerp(_startCameraRotation, _previousCameraRotation, percentageComplete);
            //camFollow.transform.rotation = Quaternion.Lerp(_startBobRotation, _endBobRotation, percentageComplete);

            if (percentageComplete >= 1.0f)
            {
                _isLepring = false;
            }
        }
    }

    void StartLerping(int i)
    {
        switch (i)
        {
            case 1:
                _startRotation = rotator.transform.rotation;
                _startCameraPosition = mainCam.transform.localPosition;
                _startRotatorPosition = rotator.transform.localPosition;
                _startCameraRotation = transform.rotation;

                //rotator position
                _endRotatorPosition = new Vector3(rotator.transform.localPosition.x, previousTransform, rotator.transform.localPosition.z);
                //camera position
                _endCameraPosition = new Vector3(0.0f, 0.0f, previousCameraDistance);
                //rotator rotation
                _endRotatorRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                break;
            case 0:
                _startRotation = rotator.transform.rotation;
                _startCameraPosition = mainCam.transform.localPosition;
                _startRotatorPosition = rotator.transform.localPosition;
                _startCameraRotation = transform.rotation;
                //_startBobRotation = camFollow.transform.rotation;
               
                //rotator position
                _endRotatorPosition = new Vector3(rotator.transform.localPosition.x, 3.7f, rotator.transform.localPosition.z);
                //camera position
                _endCameraPosition = new Vector3(0.0f, 0.5f, -5.0f);
                //rotator rotation
                _endRotatorRotation = Quaternion.Euler(-25.0f, rotator.transform.rotation.y, rotator.transform.rotation.z);
                //bob rotation
                //_endBobRotation = Quaternion.Euler(camFollow.transform.rotation.x, playerPrefab.transform.rotation.y, camFollow.transform.rotation.z);
                break;
            default:
                Debug.Log("switch case error");
                break;
        }

        _isLepring = true;
        _timeStartedLerping = Time.time;
    }

    void CloseCameraBeh()
    {
        rotationY += Input.GetAxis("Mouse Y") * sensitivity;
        rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);
        rotationX += Input.GetAxis("Mouse X") * sensitivity;

        camFollow.transform.rotation= Quaternion.Euler(0, rotationX, 0);
        transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
        
    }
}
