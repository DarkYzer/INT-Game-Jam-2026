using NUnit.Framework.Constraints;
using System;
using System.Xml.Schema;
using Unity.Mathematics;
using UnityEngine;
using System.Collections.Generic;

public class SelectorScript : MonoBehaviour
{
    // Number of items selectable
    int sizeOf;
    [SerializeField] GameObject[] spawnPoints; // Spawn points of the objects

    [SerializeField] List<GameObject> spawnableObjects; // List of objects that could be selectable
    GameObject[] myObjects; // List of available objects


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sizeOf = spawnPoints.Length;
        myObjects = new GameObject[sizeOf];
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
                    myObjects[i].SetActive(true);
                }
            }
        }
    }
}
