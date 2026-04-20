using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

public class PlaceHoop : MonoBehaviour
{
    public GameObject hoopPrefab;
    public BallFlickThrow throwScript;

    private ARRaycastManager raycastManager;
    private bool isPlaced = false;

    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    void Start()
    {
        raycastManager = GetComponent<ARRaycastManager>();

        // Disable throwing at start
        throwScript.enabled = false;
    }

    void Update()
    {
        if (isPlaced) return;

        if (Input.GetMouseButtonDown(0))
        {
            if (raycastManager.Raycast(Input.mousePosition, hits, TrackableType.PlaneWithinPolygon))
            {
                Pose pose = hits[0].pose;

                Instantiate(hoopPrefab, pose.position, pose.rotation);

                isPlaced = true;

                // Enable throwing AFTER placement
                throwScript.enabled = true;

                enabled = false;
            }
        }
    }
}