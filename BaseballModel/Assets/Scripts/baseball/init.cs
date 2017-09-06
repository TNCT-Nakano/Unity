using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class init : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //下向きにrayを打つ
        RaycastHit hit;
        //(0,0,0)周辺から打つと正しい値が出ない
        Vector3 pos = new Vector3(0, 0, 3);
        Ray downRay = new Ray(pos, Vector3.down);
        if (Physics.Raycast(downRay, out hit, 10))
        {
            print("Ray Distance: " + hit.distance);
            print("transform.position" + transform.position);
            transform.position = new Vector3(0, -hit.distance, 0);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
