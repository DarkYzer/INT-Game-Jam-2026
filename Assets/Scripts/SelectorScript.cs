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

    [SerializeField] float[] weightThresholds;
    [SerializeField] string[] nameWeightCategories;

    [SerializeField] AnimationCurve timeDilationCurve; // Preferably, you just have to change this one, ideally similar to a decreasing exponential
    [SerializeField] List<AnimationCurve> shiftingProbaCurves; // Preferably, these are linear, from one value to another

    [SerializeField] List<float> baseProbaTable; // The values we want at the start for probabilities
    [SerializeField] List<float> endProbaTable; // The values we want in the end for probabilities
    // The end being the tangent of timeDiliationCurve or something similar

    float waveCount = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sizeOf = spawnPoints.Length;
        myObjects = new GameObject[sizeOf];
        dragNdropController.OnPick.AddListener(removeObject);
        setStartProba();

        refill();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void setStartProba()
    {
        for (int i = 0; i < baseProbaTable.Count; i++)
        {
            shiftingProbaCurves.Add(AnimationCurve.Linear(0f, baseProbaTable[i], 1f, endProbaTable[i]));
        }
    }

    // Returns the index of a picked element in spawnableObjects according to probatable
    private int randomElement()
    {
        float time = timeDilationCurve.Evaluate(waveCount);
        float probaSum = 0f;
        for (int i = 0; i < shiftingProbaCurves.Count; i++)
        {
            probaSum += shiftingProbaCurves[i].Evaluate(time);
        }
        float randomPick = UnityEngine.Random.Range(1, probaSum);
        probaSum = 0f;
        for (int i = 0; i < shiftingProbaCurves.Count; i++)
        {
            probaSum += shiftingProbaCurves[i].Evaluate(time);
            if (randomPick < probaSum)
            {
                return i;
            }
        }
        return shiftingProbaCurves.Count - 1; // N'est pas censé arriver
    }

    // Refills the selector, picking random non exclusive objects from spawnableObjects
    void refill()
    {
        for (int i = 0; i < sizeOf; i++)
        {
            int explorer = randomElement();
            Vector3 spawnPosition = spawnPoints[i].transform.position; // Hard coding the z position, kinda useless
            spawnPosition.z = 0;
            myObjects[i] = Instantiate(spawnableObjects[explorer], spawnPosition, Quaternion.identity);
            myObjects[i].transform.parent = gameObject.transform; // Parenting the objects to the selector so it moves along the camera
            myObjects[i].transform.SetPositionAndRotation(spawnPosition, Quaternion.identity); // Sets the z axis to 0, still useless
            myObjects[i].SetActive(true);
            bool isWithinWeight = false;
            for (int j = 0; j < weightThresholds.Length; j++)
            {
                if (myObjects[i].GetComponent<Rigidbody>().mass > weightThresholds[j] - .1f)
                {
                    weightTexts[i].text = nameWeightCategories[j]; // Shows the weight
                    isWithinWeight = true;
                }
            }
            if (!isWithinWeight)
            {
                weightTexts[i].text = nameWeightCategories[weightThresholds.Length];
            }

        }
        waveCount += 1f;
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
