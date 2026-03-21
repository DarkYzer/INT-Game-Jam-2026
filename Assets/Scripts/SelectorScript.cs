using NUnit.Framework.Constraints;
using System;
using System.Xml.Schema;
using Unity.Mathematics;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using TMPro.Examples;

public class SelectorScript : MonoBehaviour
{
    [SerializeField] DragAndDropController dragNdropController; // The drag n drop controller

    // Number of items selectable
    int sizeOf;
    [SerializeField] GameObject[] spawnPoints; // Spawn points of the objects

    [SerializeField] List<GameObject> spawnableObjects; // List of objects that could be selectable
    GameObject[] myObjects; // List of available objects

    [SerializeField] List<TextMeshProUGUI> weightTexts; // List of weights

    private bool readyToDrop = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sizeOf = spawnPoints.Length;
        myObjects = new GameObject[sizeOf];
        dragNdropController.OnPick.AddListener(removeObject);

        refill();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void refill()
    {
        // Besoin de spawnableObjects.Count >= sizeOf
        int[] exploredObjects = new int[spawnableObjects.Count];
        for (int i = 0;  i < sizeOf; i++)
        {
            while (myObjects[i] == null) // Récupère un objet non récupéré dans myObjects depuis spawnObjects
            {
                int explorer = UnityEngine.Random.Range(0, spawnableObjects.Count);
                if (exploredObjects[explorer] != 1) // Pour l'instant ça évite d'avoir des réplicas
                {
                    exploredObjects[explorer] = 1;
                    myObjects[i] = Instantiate(spawnableObjects[explorer], spawnPoints[i].transform.position, Quaternion.identity);
                    myObjects[i].transform.parent = gameObject.transform;
                    myObjects[i].SetActive(true);
                    weightTexts[i].text = myObjects[i].GetComponent<Rigidbody>().mass.ToString() + " kg";
                }
            }
        }
    }

    void dropRandom()
    {
        while (true)
        {
            break;
        }
    }

    void drop(GameObject droppedObject)
    {
        return;
    }

    void removeObject(GameObject selectedObject)
    {
        StartCoroutine(removeObjectCoroutine(selectedObject));
        StopCoroutine(timerToDrop());
        StartCoroutine(timerToDrop());
    }
    IEnumerator timerToDrop()
    {
        yield return new WaitForSeconds(15);
        yield return new WaitForSeconds(3);
        yield return new WaitForSeconds(2);
        dropRandom();
    }


    IEnumerator removeObjectCoroutine(GameObject selectedObject)
    {
        int size = sizeOf;
        for (int i = 0; i < sizeOf;  ++i)
        {
            if (myObjects[i] == selectedObject)
            {
                myObjects[i] = null;
                weightTexts[i].text = "";
            }
            if (myObjects[i] == null)
            {
                size--;
            }
        }
        if (size <= 0)
        {
            yield return new WaitForSeconds(2);
            refill();
        }
        yield return null;
    }
}
