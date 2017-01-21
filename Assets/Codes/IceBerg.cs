using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioClip))]
[RequireComponent(typeof(PhotonView))]
public class IceBerg : MonoBehaviour
{

    private PhotonView photonView;
    public float FreezeTime = 5.0f;
    public void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && other.GetComponent<PhotonView>().isMine)
        {
            other.SendMessage("Freeze", FreezeTime);
            photonView.RPC("PlaySoundEffect", PhotonTargets.All);
            StartCoroutine(WaitAndRemove());
        }
    }

    IEnumerator WaitAndRemove()
    {
        yield return new WaitForSeconds(FreezeTime);
        photonView.RPC("Remove", photonView.owner);
    }

    [PunRPC]
    public void Remove()
    {
        if (photonView.isMine)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }

    public AudioClip StepOnIceBerg;
    [PunRPC]
    public void PlaySoundEffect()
    {
        GetComponent<AudioSource>().PlayOneShot(StepOnIceBerg);
    }
}
