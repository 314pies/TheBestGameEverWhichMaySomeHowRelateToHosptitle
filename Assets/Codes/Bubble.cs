using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PhotonView))]
public class Bubble : MonoBehaviour
{
    private PhotonView photonView;
    public void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }

    public float LefeTime=3.5f;
    public void Start()
    {
        if (photonView.isMine)
            StartCoroutine(WaitAndDestroy());
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && collision.gameObject.GetComponent<PhotonView>().isMine)
        {
            collision.gameObject.SendMessage("TriggedTrap", true);
            photonView.RPC("Remove", photonView.owner);
            gameObject.SetActive(false);
        }
    }
    [PunRPC]
    public void Remove()
    {
        PhotonNetwork.Destroy(gameObject);
    }

    IEnumerator WaitAndDestroy()
    {
        yield return new WaitForSeconds(LefeTime);
        PhotonNetwork.Destroy(gameObject);
    }

}
