using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using System;

public class testScript : MonoBehaviour, IInputClickHandler
{
    public GameObject placementObject;
    public float placementDistance;
    public float velocity;
    private List<GameObject> balls = new List<GameObject>();

    public void OnInputClicked(InputClickedEventData eventData)
    {
        RaycastHit hit;
        Vector3 headPos = Camera.main.transform.position;
        Vector3 gazeForward = Camera.main.transform.forward;
        if (Physics.Raycast(headPos, gazeForward, out hit))
            return;
        GameObject tmp = (GameObject)Instantiate(placementObject);
        balls.Add(tmp);
        
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
    }

    // Update is called once per frame
    void Update()
    {
        //nop
    }

    public void DeleteAllBalls()
    {
        Debug.Log("All balls is destroied by voice command.");
        foreach(GameObject ball in balls)
        {
            Destroy(ball);
        }
    }
}