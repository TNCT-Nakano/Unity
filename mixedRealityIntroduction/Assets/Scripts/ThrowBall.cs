using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowBall : MonoBehaviour
{
	public UH unlimitedhand;
    public float speed,angle;

    private Rigidbody rb;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        Vector3 movement = new Vector3(0.0f, angle, -speed);
        rb.AddForce(movement);

        Destroy(rb,10);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            
        }
    }

	void OnCollisionEnter(Collision collision){
		unlimitedhand.stimulate (4);
	}
}
