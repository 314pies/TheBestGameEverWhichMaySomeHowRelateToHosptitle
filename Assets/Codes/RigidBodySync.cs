using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PhotonView))]
public class RigidBodySync : MonoBehaviour
{
    [SerializeField]
    private float SynRate = 5.0f;

    public PhotonView photonView;
    public Rigidbody rigidBody;
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        photonView = GetComponent<PhotonView>();
        InvokeRepeating("CheckPos", 1.0f, SynRate);
        if (PhotonNetwork.isMasterClient)
        {
            StartCoroutine(InitialSyn());
        }
    }

    void OnValidate()
    {
        rigidBody = GetComponent<Rigidbody>();
        photonView = GetComponent<PhotonView>();
    }

    IEnumerator InitialSyn()
    {
        yield return new WaitForSeconds(0.05f);
        photonView.RPC("InitialSynPos", PhotonTargets.Others, transform.position, rigidBody.velocity);
    }


    [PunRPC]
    public void InitialSynPos(Vector3 Pos, Vector3 Velocity)
    {
        Debug.Log("Initial Sync");
        transform.position = Pos;
        rigidBody.velocity = Velocity;
    }

    public void CheckPos()
    {
        if (PhotonNetwork.isMasterClient)
            photonView.RPC("SynPos", PhotonTargets.Others, transform.position, rigidBody.velocity);
    }

    [PunRPC]
    public void SynPos(Vector3 Pos, Vector3 Velocity)
    {
        Debug.Log("Sync Invoked: " + Velocity);
        if (Vector3.Distance(transform.position, Pos) > 0.5f)
            transform.position = Pos;

        if (Vector3.Distance(Velocity, rigidBody.velocity) > 0.5f)
            rigidBody.velocity = Velocity;
    }
}
