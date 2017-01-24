using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShopScrollReset : MonoBehaviour {


	// Use this for initialization
	void Start () {
        this.GetComponent<Scrollbar>().value = 1;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
