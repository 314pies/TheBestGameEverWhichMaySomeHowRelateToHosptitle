using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PhotonView))]
public class ReleasedTrappedPlayer : MonoBehaviour {

    private PhotonView photonView;
    public void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && collision.gameObject.GetComponent<PhotonView>().isMine)
        {
            collision.gameObject.SendMessage("TriggedTrap", false);
        }
    }
}
