using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideOnClick : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            gameObject.SetActive(false);
        }
    }
}
