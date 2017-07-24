using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardDisableFrustumCulling : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        MeshFilter mf = GetComponent<MeshFilter>();
        mf.sharedMesh.bounds = new Bounds(Vector3.zero, Vector3.one * 500f);      
	}
}
