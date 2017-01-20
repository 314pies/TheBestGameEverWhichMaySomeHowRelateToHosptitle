using UnityEngine;
using System.Collections;

public class ModeManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
        NetworkCreateTest();
    }
	
	// Update is called once per frame
	void Update () {
	
	}


    public void NetworkCreateTest()
    {
        PhotonNetwork.Instantiate("TestPrefab", Vector3.zero, Quaternion.identity, 0);
    }
}
