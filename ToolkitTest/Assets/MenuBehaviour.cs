using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBehaviour : MonoBehaviour {

    public void Destroy()
    {
        Destroy(gameObject);
    }

    private void Start()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
    }
}
