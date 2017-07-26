using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;
using HoloToolkit.Unity.SpatialMapping;

public class SUTagViewer : MonoBehaviour {
    public TextMesh textMesh = null;
    public float kMinAreaForComplete = 50.0f;
    public float kMinHorizAreaForComplete = 25.0f;
    public float kMinWallAreaForComplete = 10.0f;
    // Use this for initialization
    void Start () {

    }
	// Update is called once per frame
	void Update () {
        Vector3 rayPos = Camera.main.transform.position;
        Vector3 rayVec = Camera.main.transform.forward * 10.0f;

        IntPtr raycastResultPtr = SpatialUnderstanding.Instance.UnderstandingDLL.GetStaticRaycastResultPtr();
        SpatialUnderstandingDll.Imports.PlayspaceRaycast(
            rayPos.x,
            rayPos.y,
            rayPos.z,
            rayVec.x,
            rayVec.y,
            rayVec.z,
            raycastResultPtr);
        
        SpatialUnderstandingDll.Imports.RaycastResult rayCastResult = SpatialUnderstanding.Instance.UnderstandingDLL.GetStaticRaycastResult();
        textMesh.text = rayCastResult.SurfaceType.ToString();
    }

    private bool DoesScanMeetMinBarForCompletion
    {
        get
        {
            if ((SpatialUnderstanding.Instance.ScanState != SpatialUnderstanding.ScanStates.Scanning) ||
                (!SpatialUnderstanding.Instance.AllowSpatialUnderstanding))
            {
                return false;
            }

            IntPtr statsPtr = SpatialUnderstanding.Instance.UnderstandingDLL.GetStaticPlayspaceStatsPtr();
            if (SpatialUnderstandingDll.Imports.QueryPlayspaceStats(statsPtr) == 0)
            {
                return false;
            }

            SpatialUnderstandingDll.Imports.PlayspaceStats stats = SpatialUnderstanding.Instance.UnderstandingDLL.GetStaticPlayspaceStats();
            if ((stats.TotalSurfaceArea > kMinAreaForComplete) ||
                (stats.HorizSurfaceArea > kMinHorizAreaForComplete) ||
                (stats.WallSurfaceArea > kMinWallAreaForComplete))
            {
                return true;
            }
            return false;
        }
    }
}
