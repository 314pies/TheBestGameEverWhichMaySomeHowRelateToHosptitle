using UnityEngine;
using System.Collections;

public class BoardWall : MonoBehaviour {

	public void OnCollisionEnter(Collision other)
    {
        if(other.collider.tag == "wave")
        {
            other.gameObject.SendMessage("DestroySelf");
        }

    }
}
