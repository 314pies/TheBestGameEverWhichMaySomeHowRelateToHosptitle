using UnityEngine;
using System.Collections;

public class WaveCollector : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public int WaveHolded=0;
    void OnCollisionEnter(Collision collision)
    { 
        if(collision.collider.tag == "wave")
        {
            if(WaveHolded == 0)
            {
                Debug.Log("Collllll");
                WaveHolded++;
                collision.gameObject.SendMessage("DestroySelf");
                //Destroy wave
            }
        }
    }
}
