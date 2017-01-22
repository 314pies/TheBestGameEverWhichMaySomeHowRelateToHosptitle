using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class GetName : MonoBehaviour
{

    public void OnInputNameUpdate(string _Name)
    {
        PlayerPrefs.SetString("Name", _Name);
        Debug.Log(_Name);
    }


    public void Start()
    {
        if (PlayerPrefs.HasKey("Name"))
        {
            GetComponent<InputField>().text = PlayerPrefs.GetString("Name");
        }
    }
}
