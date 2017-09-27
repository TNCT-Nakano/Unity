using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class init : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Vector3 headPos = Camera.main.transform.position;
        RaycastHit floor;
        if (Physics.Raycast(headPos, -transform.up, out floor, 3f, HoloToolkit.Unity.SpatialMapping.SpatialMappingManager.Instance.LayerMask))
            gameObject.transform.position = floor.point;

        GetComponent<HoloToolkit.Unity.SpatialMapping.SurfaceMeshesToPlanes>().MakePlanes();
        GameObject.Find("SpatialMapping").GetComponent<HoloToolkit.Unity.SpatialMapping.SpatialMappingObserver>().CleanupObserver();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
