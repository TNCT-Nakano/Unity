using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class infoBehaviour : MonoBehaviour {
    public bool autoDestroing=true;
	// Use this for initialization
	void Start () {
        if(autoDestroing)
            StartCoroutine(autoDestroy());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator autoDestroy()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}
