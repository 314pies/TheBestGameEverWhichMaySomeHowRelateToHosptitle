using UnityEngine;
using System.Collections;

public class ModeManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GoodPlayerSpawn();
    }
	
	// Update is called once per frame
	void Update () {
	
	}


    public void GoodPlayerSpawn()
    {
        PhotonNetwork.Instantiate("GoodPlayer", Vector3.zero, Quaternion.identity, 0);
    }
}
