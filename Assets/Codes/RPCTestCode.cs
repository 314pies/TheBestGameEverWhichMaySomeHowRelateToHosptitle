using UnityEngine;
using System.Collections;

public class RPCTestCode : MonoBehaviour {

    public PhotonView photonView;
    void Start()
    {
        photonView.RPC("TestTest", PhotonTargets.Others,1,2,"Godddddder");
    }



    [PunRPC]
    public void TestTest(int a,int b,string A_A)
    {
        Debug.Log(a+", "+b +": " + A_A);
    }
}
