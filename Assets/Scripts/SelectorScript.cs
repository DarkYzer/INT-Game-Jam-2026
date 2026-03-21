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

    [SerializeField] List<ObjectScript> spawnableObjects; // List of objects that could be selectable
    ObjectScript[] myObjects; // List of available objects

    [SerializeField] List<TextMeshProUGUI> weightTexts; // List of weights

    private bool empty = true; // Whether the selector is empty or not


    // Timers for the shake period of to-be-dropped objects
    [SerializeField] float slowTimer = 3f;
    [SerializeField] float fastTimer = 2f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sizeOf = spawnPoints.Length;
        myObjects = new ObjectScript[sizeOf];
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
                    myObjects[i] = (ObjectScript)Instantiate(spawnableObjects[explorer], spawnPoints[i].transform.position, Quaternion.identity);
                    myObjects[i].transform.parent = gameObject.transform;
                    myObjects[i].gameObject.SetActive(true);
                    weightTexts[i].text = myObjects[i].GetComponent<Rigidbody>().mass.ToString() + " kg";
                }
            }
        }
        empty = false;
    }

    // Returns a random ObjectScript among the ones left
    ObjectScript selectRandom()
    {
        while (!empty)
        {
            int explorer = UnityEngine.Random.Range(0, sizeOf);
            if ( myObjects[explorer] != null )
            {

                return myObjects[explorer];
            }
        }
        return null;
    }

    // Drops an ObjectScript
    void drop(ObjectScript droppedObject)
    {
        return;
    }

    // Removes an object from the selector
    void removeObject(GameObject selectedObject)
    {
        StartCoroutine(removeObjectCoroutine(selectedObject));
        //StopCoroutine(timerToDrop());
        //StartCoroutine(timerToDrop());
    }

    // Countdown to when you want to drop an object from the selector due to afk
    /*
    IEnumerator timerToDrop()
    {
        yield return new WaitForSeconds(15); // The first 15s
        ObjectScript droppedObject = selectRandom();
        if ( droppedObject != null)
        {
            droppedObject.shakeSlow(slowTimer); // First warning phase, slow shake on the object
            yield return new WaitForSeconds(slowTimer); 
            if (droppedObject.transform.parent != null)
            {
                droppedObject.shakeFast(fastTimer); // Second warning phase, fast shake on the object
                yield return new WaitForSeconds(fastTimer);
                if (droppedObject.transform.parent != null)
                {
                    drop(droppedObject); // Drops the object
                }
            }
        }
    }
    */


    // Removes an object from the selector. Refills it if there are no objects left
    IEnumerator removeObjectCoroutine(GameObject selectedObject)
    {
        int size = sizeOf;
        for (int i = 0; i < sizeOf;  ++i)
        {
            if (myObjects[i] == selectedObject)
            {
                myObjects[i].transform.parent = null;
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
            empty = true;
            yield return new WaitForSeconds(2);
            refill();
        }
        yield return null;
    }
}
