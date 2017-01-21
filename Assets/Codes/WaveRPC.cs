using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PhotonView))]
public class WaveRPC : MonoBehaviour
{

    private PhotonView photonView;
    public void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }

    public void DestroySelf()
    {
        photonView.RPC("DeleteWave", PhotonTargets.MasterClient);
    }

    [PunRPC]
    public void DeleteWave()
    {
        if (PhotonNetwork.isMasterClient)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
