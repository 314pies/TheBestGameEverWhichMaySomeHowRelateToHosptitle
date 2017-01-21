using UnityEngine;
using System.Collections;

public class Patient : MonoBehaviour
{

    public PhotonView photonView;

    // Use this for initialization
    void Start()
    {
        InvokeRepeating("CreateWave", 2.0f, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public float WaveSpeed = 10.0f;

    public void CreateWave()
    {
        if (PhotonNetwork.isMasterClient)
        {
            GameObject NewWave = PhotonNetwork.Instantiate("Wave", transform.position, Quaternion.identity, 0);
            Vector3 Speed = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
            Speed.Normalize();
            NewWave.GetComponent<Rigidbody>().velocity = Speed* WaveSpeed;
        }
    }


}
