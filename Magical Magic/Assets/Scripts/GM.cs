using UnityEngine;
using System.Collections;
using System;

public class GM : MonoBehaviour {

    public static GM Instance;

    public static int cashAmount;


    //camera
    public enum CameraState
    {
        LongDistance,
        CloseDistance
    }

    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if(Instance!= this)
        {
            DestroyImmediate(gameObject);
        }
    }

	void Start () {
	
	}
	
	void Update () {
	
	}

}

