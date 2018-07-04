using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CamAdjust : MonoBehaviour {

    public GameObject realSenseCam;
    GameObject[] cams = new GameObject[4];

    // Assuming aspect ratio of 16:9

    [Range(20, 50)]
    [Tooltip("Diagonal of the screen in inches")]
    public float screenSize = 40;
    const float DIAGONAL_ANGLE = 0.51238946f;

    // Use this for initialization
    void Start() {
        // Figure out the camera's positions in the corners. We want the center of the screen to be the origin.
        // Position in unit circle * half screen size * inches to meters
        Vector3[] positions = CalculatePositions();

        GenerateCams(positions);
    }

    // Update is called once per frame
    void Update() {
        Vector3[] p = CalculatePositions();
        ApplyPositions(p);
    }

    Vector3[] CalculatePositions() {
        Vector3[] p = {
            new Vector3( Mathf.Cos(DIAGONAL_ANGLE),  Mathf.Sin(DIAGONAL_ANGLE))*screenSize*0.0254f*0.5f,
            new Vector3(-Mathf.Cos(DIAGONAL_ANGLE),  Mathf.Sin(DIAGONAL_ANGLE))*screenSize*0.0254f*0.5f,
            new Vector3(-Mathf.Cos(DIAGONAL_ANGLE), -Mathf.Sin(DIAGONAL_ANGLE))*screenSize*0.0254f*0.5f,
            new Vector3( Mathf.Cos(DIAGONAL_ANGLE), -Mathf.Sin(DIAGONAL_ANGLE))*screenSize*0.0254f*0.5f
        };
        return p;
    }

    void ApplyPositions(Vector3[] pos) {
        for (int i = 0; i < pos.Length; ++i) {
            if (i > cams.Length) {
                break;
            }

            cams[i].transform.position = pos[i];
        }
    }

    void GenerateCams(Vector3[] positions) {
        for (int i = transform.childCount - 1; i >= 0; --i) {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }

        GameObject cam;
        for (int i = 0; i < 4; ++i) {
            cam = Instantiate(realSenseCam, transform.position + positions[i], Quaternion.identity);
            cam.transform.SetParent(transform);
            cams[i] = cam;
        }

    }
}
