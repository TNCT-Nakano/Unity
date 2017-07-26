using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using System;
using HoloToolkit.Unity.SpatialMapping;

public class testScript : MonoBehaviour, IInputClickHandler
{
    public GameObject placementObject;
    public float placementDistance;
    public float velocity;
    private GameObject[] balls;

    public void OnInputClicked(InputClickedEventData eventData)
    {
        GameObject tmp = (GameObject)Instantiate(placementObject);
        Vector3 headPos = Camera.main.transform.position;
        Vector3 gazeForward = Camera.main.transform.forward;
        Vector3 placementPos = headPos + placementDistance * gazeForward;
        Rigidbody rb = tmp.GetComponent<Rigidbody>();

        rb.position = placementPos;
        rb.velocity = velocity * Camera.main.transform.forward;
        rb.maxAngularVelocity = 30;
        rb.angularVelocity = new Vector3(-30f,0f,0f);
    }

    // Use this for initialization
    void Start()
    {
        InputManager.Instance.AddGlobalListener(gameObject);
        SurfaceMeshesToPlanes.Instance.MakePlanes();
        SurfaceMeshesToPlanes.Instance.MakePlanesComplete += DeleteMeshes;
    }

    void DeleteMeshes(object s, EventArgs e)
    {
        SpatialMappingManager.Instance.StopObserver();
        foreach (Transform t in SpatialMappingManager.Instance.gameObject.transform)
            Destroy(t.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        //nop
    }

    public void DeleteAllBalls()
    {
        Debug.Log("All balls is destroied by voice command.");
        balls = GameObject.FindGameObjectsWithTag("Ball");
        foreach(GameObject ball in balls)
        {
            Destroy(ball);
        }
    }
}