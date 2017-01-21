using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioClip))]
[RequireComponent(typeof(PhotonView))]
public class Bombs : MonoBehaviour
{
    private PhotonView photonView;
    void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }

    public float ExplosionPower = 10.0f;
    public float Explosionradius = 1.5f;
    public GameObject ExplosionEffect;

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && other.GetComponent<PhotonView>().isMine)
        {

            Collider[] colliders = Physics.OverlapSphere(transform.position, Explosionradius);
            foreach (Collider hit in colliders)
            {
                if (hit.tag == "Player")
                {
                    photonView.RPC("ExplodeAndDestroy", PhotonTargets.All);
                    Vector3 Dir = hit.transform.position - transform.position;
                    Dir.Normalize();
                    hit.GetComponent<controller>().Explode(ExplosionPower, Dir * ExplosionPower);
                    
                }
            }
        }
    }

    [PunRPC]
    public void ExplodeAndDestroy()
    {
        photonView.RPC("PlaySoundEffect", PhotonTargets.All);
        Instantiate(ExplosionEffect, transform.position, Quaternion.identity);
        gameObject.SetActive(false);
        if (photonView.isMine)
            PhotonNetwork.Destroy(gameObject);
    }

    public AudioClip Bomb;
    [PunRPC]
    public void PlaySoundEffect()
    {
        Debug.Log("A______________A");
        GetComponent<AudioSource>().PlayOneShot(Bomb, 1.0f);
    }

}
