using System;
using System.Collections.Generic;
using System.Collections;
using System.Xml.Schema;
using UnityEngine;

public class CardBoardScript : ObjectScript
{
    [SerializeField] int maxHits = 5;
    [SerializeField] ParticleSystem explosion;
    [SerializeField] GameObject colis;
    RaycastHit[] hits;
    [SerializeField] float explosionRange = 1f;
    [SerializeField] float explosionStrength = 10f;

    void Awake()
    {
        explosion.gameObject.SetActive(false);
    }

    public override void fakeUpdate()
    {
        hits = Physics.RaycastAll(transform.position, Vector3.up, Mathf.Infinity);
        if (hits.Length/2 > maxHits)
            StartCoroutine(exploding());
    }

    private IEnumerator exploding()
    {
        // mouvement d'expolsion
        RaycastHit[] hitsRange;
        hitsRange = Physics.SphereCastAll(transform.position, explosionRange, transform.up);
        foreach (RaycastHit hit in hitsRange)
        {
            if (hit.transform.CompareTag("MovingObject"))
            {
                Debug.Log(hit.transform.name);
                Vector3 center = (hit.transform.position + transform.position) / 2;
                Vector3 speed = hit.transform.position - center;
                Debug.Log(speed);
                Rigidbody rb = hit.transform.GetComponent<Rigidbody>();
                rb.AddForce(speed * explosionStrength, ForceMode.Impulse);
            }
        };
        explosion.gameObject.SetActive(true);
        explosion.Play();
        colis.SetActive(false);
        yield return new WaitForSeconds(.5f);
        Destroy(gameObject);
        DragAndDropController.Instance.hasBeenPlaced.Remove(transform);
    }

}
