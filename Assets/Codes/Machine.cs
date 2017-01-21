using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PhotonView))]
public class Machine : MonoBehaviour
{
    private PhotonView photonView;
    public void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }

    public int AvailableBubbles = 10;
    public float Frequency = 1.0f;
    public float BubbleSpeed = 10.0f;

    public Vector3 SpawnPosAdj = Vector3.up;
    public void Start()
    {
        InvokeRepeating("CreateBubble", 1.0f, Frequency);
    }

    public void CreateBubble()
    {
        if (PhotonNetwork.isMasterClient && AvailableBubbles > 0)
        {

            GameObject NewWave = PhotonNetwork.Instantiate("Bubble", transform.position + SpawnPosAdj, Quaternion.identity, 0);
            Vector3 Speed = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
            Speed.Normalize();
            NewWave.GetComponent<Rigidbody>().velocity = Speed * BubbleSpeed;
            AvailableBubbles--;
        }
        else if (AvailableBubbles <= 0)
        {
            photonView.RPC("DestroySelf", photonView.owner);
        }
    }

    [PunRPC]
    public void DestroySelf()
    {
        if (photonView.isMine)
            PhotonNetwork.Destroy(gameObject);
    }
}
