using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeScript : MonoBehaviour {
    public Material treeMaterial;
    private new Renderer renderer;
    // Use this for initialization
    void Start () {
        renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update () {
        renderer.material = treeMaterial;
    }
}
