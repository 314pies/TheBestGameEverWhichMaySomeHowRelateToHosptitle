using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PhotonView))]
public class WaveCollector : MonoBehaviour
{

    private PhotonView photonView;
    public void Awake()
    {
        photonView = GetComponent<PhotonView>();
        WaveHoldSign.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public int WaveHolded = 0;
    public GameObject WaveHoldSign;
    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "wave")
        {
            if (WaveHolded == 0)
            {
                collision.gameObject.SendMessage("DestroySelf");
                WaveHolded++;
                photonView.RPC("UpdateWaveHoldStatus", PhotonTargets.AllBuffered, WaveHolded);
                //Destroy wave
            }
        }

        if (collision.collider.tag == "patient")
        {
            collision.gameObject.SendMessage("RecieveWave");
            WaveHolded = 0;
            photonView.RPC("UpdateWaveHoldStatus", PhotonTargets.AllBuffered, WaveHolded);
        }
    }

    [PunRPC]
    public void UpdateWaveHoldStatus(int WaveCount)
    {
        WaveHolded = WaveCount;
        //Not holding, normal
        if (WaveHolded == 0)
        {
            gameObject.layer = 0;
            WaveHoldSign.SetActive(false);
        }
        else
        {
            gameObject.layer = 10;
            WaveHoldSign.SetActive(true);
        }

    }
}
