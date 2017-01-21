using UnityEngine;
using System.Collections;

public class ExpTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<Rigidbody>().AddExplosionForce(500, transform.position, 500);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
