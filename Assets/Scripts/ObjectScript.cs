using NUnit.Framework;
using UnityEngine;

public class ObjectScript : MonoBehaviour
{
    public bool isTrigger = false; // est ce que l'objet touche un autre ?bool isTrigger = false; // est ce que l'objet touche un autre ?
    public void OnTriggerStay(Collider other)
    {
        isTrigger = true;        
    }

    public void OnTriggerExit(Collider other)
    {
        isTrigger = false;       
    }
}
