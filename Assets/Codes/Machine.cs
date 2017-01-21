using UnityEngine;
using System.Collections;

public class Machine : MonoBehaviour {
    private PhotonView photonView;
    public void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update () {
	
	}
}
