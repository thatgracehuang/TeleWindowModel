using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CamFulcrum : MonoBehaviour {

    [Range(40, 120)]
    [Tooltip("Horizontal FOV in degrees. Camera's default is 69.4")]
    public float hFOV = 69.4f;

    [Range(30, 100)]
    [Tooltip("Vertical FOV in degrees. Camera's default is 42.5")]
    public float vFOV = 42.5f;

    [Range(0.3f, 10)]
    [Tooltip("Minimum viewing distance in meters. Specs minimum is 0.3")]
    public float minDist = 0.3f;

    [Range(0.3f, 10)]
    [Tooltip("Maximim viewing distance in meters. Specs max is 10")]
    public float maxDist = 10f;

    public Material frustrumMaterial;
    public bool displayFrustrum = true;
    public bool displaySpotlight = true;

    Mesh fulcrum;

    List<Vector3> verts;
    List<int> tris;
    List<List<int>> faces;

    MeshFilter mf;
    MeshRenderer mr;
    Projector proj;
    Light spot;

	// Use this for initialization
	void Start () {
        fulcrum = new Mesh();

        verts = new List<Vector3>();
        CalculateVerts(ref verts);

        tris = new List<int>();
        faces = new List<List<int>>();
        GenerateTris(ref tris);

        fulcrum.SetVertices(verts);
        fulcrum.SetTriangles(tris, 0);
        //for (int i = 0; i < faces.Count; ++i) {
        //    fulcrum.SetTriangles(faces[i], i);
        //}
        fulcrum.RecalculateNormals();
        fulcrum.RecalculateBounds();

        fulcrum.MarkDynamic();

        mf = GetComponent<MeshFilter>();
        mf.mesh = fulcrum;
        mr = GetComponent<MeshRenderer>();
        mr.material = frustrumMaterial;

        proj = GetComponent<Projector>();
        spot = GetComponent<Light>();
    }
	
	// Update is called once per frame
	void Update () {
        CalculateVerts(ref verts);
        fulcrum.SetVertices(verts);

        GenerateTris(ref tris);
        fulcrum.SetTriangles(tris, 0);

        fulcrum.RecalculateNormals();
        fulcrum.RecalculateBounds();


        proj.nearClipPlane = minDist;
        proj.farClipPlane = maxDist;
        proj.fieldOfView = vFOV;

        proj.material.SetVector("_Forward", (Vector4)proj.transform.forward);

        mr.enabled = displayFrustrum;
 
        spot.enabled = displaySpotlight;
        if (spot.enabled) {
            spot.spotAngle = hFOV;
            spot.range = maxDist;
        }
    }

    //private void OnDrawGizmos() {
    //    for (int i = 0; i < verts.Count; ++i) {
            
    //        Gizmos.DrawSphere(verts[i], 0.01f);
    //    }
    //}

    void CalculateVerts(ref List<Vector3> v) {
        // Cache relevant angles in radians
        float hRad = hFOV * Mathf.Deg2Rad * 0.5f;
        float vRad = vFOV * Mathf.Deg2Rad * 0.5f;

        // Wipe old list
        v.Clear();
        // Near plane
        v.Add(new Vector3(Mathf.Tan(hRad), Mathf.Tan(vRad), 1) * minDist);
        v.Add(new Vector3(-Mathf.Tan(hRad), Mathf.Tan(vRad), 1) * minDist);
        v.Add(new Vector3(-Mathf.Tan(hRad), -Mathf.Tan(vRad), 1) * minDist);
        v.Add(new Vector3(Mathf.Tan(hRad), -Mathf.Tan(vRad), 1) * minDist);
        // Far Plane
        v.Add(new Vector3(Mathf.Tan(hRad), Mathf.Tan(vRad), 1) *   maxDist);
        v.Add(new Vector3(-Mathf.Tan(hRad), Mathf.Tan(vRad), 1) *  maxDist);
        v.Add(new Vector3(-Mathf.Tan(hRad), -Mathf.Tan(vRad), 1) * maxDist);
        v.Add(new Vector3(Mathf.Tan(hRad), -Mathf.Tan(vRad), 1) *  maxDist);
    }

    void GenerateTris(ref List<int> t) {
        t.Clear();

        //Near plane
        t.Add(2);
        t.Add(1);
        t.Add(0);
        t.Add(3);
        t.Add(2);
        t.Add(0);
        // Sides
        t.Add(1);
        t.Add(2);
        t.Add(6);
        t.Add(6);
        t.Add(5);
        t.Add(1);

        t.Add(3);
        t.Add(0);
        t.Add(7);
        t.Add(7);
        t.Add(0);
        t.Add(4);

        t.Add(0);
        t.Add(1);
        t.Add(5);
        t.Add(0);
        t.Add(5);
        t.Add(4);

        t.Add(2);
        t.Add(3);
        t.Add(6);
        t.Add(3);
        t.Add(7);
        t.Add(6);


        // Far Plane
        t.Add(5);
        t.Add(6);
        t.Add(4);
        t.Add(6);
        t.Add(7);
        t.Add(4);
    }

    void GenerateTris(ref List<List<int>> f) {
        f.Clear();

        List<int> face = new List<int>();
        //Near plane
        face.Add(1);
        face.Add(2);
        face.Add(0);
        face.Add(3);
        face.Add(2);
        face.Add(0);

        f.Add(face);

        face = new List<int>();
        //Far plane
        face.Add(5);
        face.Add(6);
        face.Add(4);
        face.Add(7);
        face.Add(6);
        face.Add(4);

        f.Add(face);

        Debug.Log(f);
    }
}
