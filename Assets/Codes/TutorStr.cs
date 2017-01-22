using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class TutorStr : MonoBehaviour
{

    int CurrentPage = 0;
    public string[] PageStrs;
    public Text Context;

    public void NextPage()
    {
        if (CurrentPage + 1 < PageStrs.Length)
            CurrentPage++;

        Context.text = PageStrs[CurrentPage];
    }

    public void LastPage()
    {
        if (CurrentPage -1 >=0)
            CurrentPage--;

        Context.text = PageStrs[CurrentPage];
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameObject.SetActive(false);
        }

    }
}
