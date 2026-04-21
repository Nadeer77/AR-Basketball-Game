using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;
using TMPro;

public class PlaceHoop : MonoBehaviour
{
    public GameObject hoopPrefab;
    public BallFlickThrow throwScript;
    public TextMeshProUGUI startMessageText;
    public TextMeshProUGUI missionText;

    private ARRaycastManager raycastManager;
    private bool isPlaced = false;

    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    void Start()
    {
        raycastManager = GetComponent<ARRaycastManager>();

        // 🔴 Disable throwing until hoop is placed
        throwScript.enabled = false;
        missionText.gameObject.SetActive(false);
        startMessageText.gameObject.SetActive(true);
    }

    void Update()
    {
        if (isPlaced) return;

        if (Input.GetMouseButtonDown(0))
        {
            if (raycastManager.Raycast(Input.mousePosition, hits, TrackableType.PlaneWithinPolygon))
            {
                Pose pose = hits[0].pose;

                startMessageText.gameObject.SetActive(false);


                // 🟢 Instantiate hoop
                GameObject spawnedHoop = Instantiate(hoopPrefab, pose.position, Quaternion.identity);

                // 🎯 Make hoop face camera (player)
                Vector3 direction = Camera.main.transform.position - spawnedHoop.transform.position;
                direction.y = 0; // keep upright
                spawnedHoop.transform.rotation = Quaternion.LookRotation(direction);

                // 🎯 Assign hoop target for throwing
                throwScript.hoopTarget = spawnedHoop.transform;

                // 🟢 Mark placed
                isPlaced = true;

                // 🟢 Enable throwing
                
                throwScript.enabled = true;
                missionText.gameObject.SetActive(true);

                // ❌ Disable this script (no more placement)
                enabled = false;

            }
        }
    }
}