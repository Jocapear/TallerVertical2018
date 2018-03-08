using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinocularBehaviorScript : MonoBehaviour {
    public GameObject player;
    private GameObject pivot;
    public bool current;
    // Use this for initialization
    void Start () {
        this.current = false;
        this.pivot = this.transform.GetChild(0).gameObject;
	}
	
	// Update is called once per frame
	void Update () {
        //if(System.Math.Abs(Vector3.Distance(this.player.transform.position, this.transform.position)) < 3) { 
        if (current) { 
            pivot.SetActive(false);
        }
        else
        {
            pivot.SetActive(true);
        }
	}
}
