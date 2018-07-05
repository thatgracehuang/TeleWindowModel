using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CamAdjust : MonoBehaviour {
    [Header("Screen Parameters")]

    // Assuming aspect ratio of 16:9
    [Range(20, 50)]
    [Tooltip("Diagonal of the screen in inches")]
    public float screenSize = 40;
    const float DIAGONAL_ANGLE = 0.51238946f;

    public enum SetupType {
        Corners,
        Edges
    };
    public SetupType setup = SetupType.Corners;

    [Header("Camera Parameters")]

    public GameObject realSenseCam;
    GameObject[] cams = new GameObject[4];

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

    // Use this for initialization
    void Start() {
        Vector3[] positions = CalculatePositions();

        GenerateCams(positions);
    }

    // Update is called once per frame
    void Update() {
        Vector3[] p = CalculatePositions();

        ApplyPositions(p);
        ApplyParams();
    }

    Vector3[] CalculatePositions() {
        switch (setup) {
            case SetupType.Corners:
                return CalculateCornerPositions();
            case SetupType.Edges:
                return CalculateEdgePositions();
            default:
                return CalculateCornerPositions();
        }
    }

    void ApplyParams() {
        CamFulcrum cf;
        foreach (GameObject c in cams) {
            cf = c.GetComponent<CamFulcrum>();
            cf.hFOV = hFOV;
            cf.vFOV = vFOV;
            cf.minDist = minDist;
            cf.maxDist = maxDist;
            cf.frustrumMaterial = frustrumMaterial;
            cf.displayFrustrum = displayFrustrum;
        }
    }

    Vector3[] CalculateCornerPositions() {
        // Figure out the camera's positions in the corners. We want the center of the screen to be the origin.
        // Position in unit circle * half screen size * inches to meters
        Vector3[] p = {
            new Vector3( Mathf.Cos(DIAGONAL_ANGLE),  Mathf.Sin(DIAGONAL_ANGLE))*screenSize*0.0254f*0.5f,
            new Vector3(-Mathf.Cos(DIAGONAL_ANGLE),  Mathf.Sin(DIAGONAL_ANGLE))*screenSize*0.0254f*0.5f,
            new Vector3(-Mathf.Cos(DIAGONAL_ANGLE), -Mathf.Sin(DIAGONAL_ANGLE))*screenSize*0.0254f*0.5f,
            new Vector3( Mathf.Cos(DIAGONAL_ANGLE), -Mathf.Sin(DIAGONAL_ANGLE))*screenSize*0.0254f*0.5f
        };
        return p;
    }

    Vector3[] CalculateEdgePositions() {
        Vector3[] p = {
            new Vector3( 0.0f,  Mathf.Sin(DIAGONAL_ANGLE))*screenSize*0.0254f*0.5f,
            new Vector3(-Mathf.Cos(DIAGONAL_ANGLE),  0.0f)*screenSize*0.0254f*0.5f,
            new Vector3(0.0f, -Mathf.Sin(DIAGONAL_ANGLE))*screenSize*0.0254f*0.5f,
            new Vector3( Mathf.Cos(DIAGONAL_ANGLE), 0.0f)*screenSize*0.0254f*0.5f
        };
        return p;
    }

    void ApplyPositions(Vector3[] pos) {
        for (int i = 0; i < pos.Length; ++i) {
            if (i > cams.Length) {
                break;
            }

            cams[i].transform.position = transform.position + pos[i];
        }
    }

    void GenerateCams(Vector3[] positions) {
        for (int i = transform.childCount - 1; i >= 0; --i) {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }

        GameObject cam;
        for (int i = 0; i < 4; ++i) {
            cam = Instantiate(realSenseCam, transform.position + positions[i], transform.rotation);
            cam.transform.SetParent(transform);
            cams[i] = cam;
        }

    }
}
