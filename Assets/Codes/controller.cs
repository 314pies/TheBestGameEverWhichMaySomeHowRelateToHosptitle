using UnityEngine;
using System.Collections;

public class controller : MonoBehaviour
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
}
