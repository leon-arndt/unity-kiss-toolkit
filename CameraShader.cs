using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShader : MonoBehaviour {
    public Shader newShader;
	// Use this for initialization
	void Start () {
        Camera.main.SetReplacementShader(newShader, "Opaque");

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
