using UnityEngine;
using System.Collections;

public class controller : Photon.MonoBehaviour
{
    public Rigidbody CharacterController;
    public int LeftVelocity = -10, DownVelocity = -10, UpVelocity = 10, RightVelocity = 10;
    public int JumpVelocity = 5;
    void Start()
    {
        CharacterController = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.isMine)
        {
            if (Input.GetKey(KeyCode.S))
            {
                //Debug.Log("aksdjhajhfdsg");
                CharacterController.velocity = new Vector3(
                    CharacterController.velocity.x,
                    CharacterController.velocity.y,
                    DownVelocity
                    );
            }
            if (Input.GetKey(KeyCode.A))
            {
                //Debug.Log("aksdjhajhfdsg");
                CharacterController.velocity = new Vector3(
                    LeftVelocity,
                    CharacterController.velocity.y,
                    CharacterController.velocity.z
                    );
            }
            if (Input.GetKey(KeyCode.D))
            {
                //Debug.Log("aksdjhajhfdsg");
                CharacterController.velocity = new Vector3(
                    RightVelocity,
                    CharacterController.velocity.y,
                    CharacterController.velocity.z
                    );
            }
            if (Input.GetKey(KeyCode.W))
            {
                //Debug.Log("aksdjhajhfdsg");
                CharacterController.velocity = new Vector3(
                    CharacterController.velocity.x,
                    CharacterController.velocity.y,
                    UpVelocity
                    );
            }
            if (Input.GetKey("space"))
            {
                //Debug.Log("aksdjhajhfdsg");
                CharacterController.velocity = new Vector3(
                    CharacterController.velocity.x,
                    JumpVelocity,
                    CharacterController.velocity.z
                    );
            }
        }
        else
        {

            transform.position = Vector3.Lerp(transform.position, this.correctPlayerPos, Time.deltaTime * 5);
            transform.rotation = Quaternion.Lerp(transform.rotation, this.correctPlayerRot, Time.deltaTime * 5);
        }
    }


    private Vector3 correctPlayerPos;
    private Quaternion correctPlayerRot;

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);

        }
        else
        {
            // Network player, receive data
            this.correctPlayerPos = (Vector3)stream.ReceiveNext();
            this.correctPlayerRot = (Quaternion)stream.ReceiveNext();
        }
    }
}
