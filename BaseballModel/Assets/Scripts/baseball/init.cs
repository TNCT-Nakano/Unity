using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class init : MonoBehaviour {

	public GameObject bases;

	// Use this for initialization
	void Start () {
        Debug.Log("Bases init");
        //下向きにrayを打つ
        RaycastHit hit;
        //(0,0,0)周辺から打つと正しい値が出ない
        Vector3 pos = new Vector3(0, 0, 0);
        Ray downRay = new Ray(pos, Vector3.down);
        if (Physics.Raycast(downRay, out hit, 10))
        {
			/* 地面との距離でプレハブを移動
            print("Ray Distance: " + hit.distance);
            print("transform.position" + transform.position);
            transform.position = new Vector3(0, -hit.distance, 0);
            */

			/* o地点から下に向けて当たった座標にプレハブ配置　*/
			Instantiate (bases, hit.point, Quaternion.identity);

            Debug.Log("Bases set");
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
