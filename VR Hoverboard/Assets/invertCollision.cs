using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class invertCollision : MonoBehaviour {

	// Use this for initialization
	void Start () {
        MeshFilter filter = GetComponent<MeshFilter>();

        if (filter != null)
        {
            Mesh mesh = filter.mesh;

            Vector3[] normals = mesh.normals;
            for (int i = 0; i < normals.Length; i++)
            {
                normals[i] = -normals[i];
            }

            for (int m = 0; m < mesh.subMeshCount; m++)
            {
                int[] triangles = mesh.GetTriangles(m);
                for (int j = 0; j < triangles.Length; j+=3)
                {
                    int temp = triangles[j];
                    triangles[j] = triangles[j + 1];
                    triangles[j + 1] = temp;
                }
                mesh.SetTriangles(triangles, m);
            }

            this.GetComponent<MeshCollider>().sharedMesh = filter.mesh;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
