using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErrorScreeHandler : MonoBehaviour
{
    public GameObject screen;
    public void Colse()
    {
        screen.SetActive(false);
    }
}
