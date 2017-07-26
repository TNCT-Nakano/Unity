using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HoloToolkit.Unity.InputModule;
using System;

public class HandObserver : MonoBehaviour, ISourceStateHandler
{
    public GameObject mahoujin;
    public GameObject buster;
    public float maxDistance;
    private List<GameObject> fields = new List<GameObject>();
    private GameObject busterInstance;

    public void OnSourceDetected(SourceStateEventData eventData)
    {
        Debug.Log("source:" + eventData.InputSource);
        Vector3 headPos = Camera.main.transform.position;
        Vector3 gazeForward = Camera.main.transform.forward;
        ExpansionField(headPos, gazeForward);
    }

    public void OnSourceLost(SourceStateEventData eventData)
    {
        foreach (GameObject field in fields)
        {
            //field.GetComponent<MahoujinBehaviour>().destroy = true;
        }
        fields.Clear();
        Destroy(busterInstance);
    }

    void ExpansionField(Vector3 headPos, Vector3 gazeForward)
    {
        GameObject parentField = Instantiate(mahoujin, headPos + gazeForward.normalized * maxDistance, Quaternion.FromToRotation(transform.forward, gazeForward));
        fields.Add(parentField);

        busterInstance = Instantiate(buster, headPos + gazeForward.normalized * (maxDistance + 0.25f), Quaternion.FromToRotation(transform.forward, gazeForward), parentField.transform);

        RaycastHit floor;
        if (Physics.Raycast(headPos, -transform.up, out floor, 3f, HoloToolkit.Unity.SpatialMapping.SpatialMappingManager.Instance.LayerMask))
            fields.Add(Instantiate(mahoujin, (new Vector3(0, 0.01f, 0)) + floor.point, Quaternion.FromToRotation(transform.forward, transform.up)));
    }

    // Use this for initialization
    void Start()
    {
        InputManager.Instance.AddGlobalListener(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
