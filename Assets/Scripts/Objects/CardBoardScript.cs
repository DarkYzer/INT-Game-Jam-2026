using System;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;

public class CardBoardScript : ObjectScript
{
    [SerializeField] int maxHits = 5;
    [SerializeField] ParticleSystem explosion;
    RaycastHit[] hits;
    [SerializeField] float explosionRange = 1f;
    [SerializeField] float explosionStrength = 10f;
    public override void fakeUpdate()
    {
        Debug.Log($"fakeUpdate: {hits.Length/2}");
        hits = Physics.RaycastAll(transform.position, Vector3.up, Mathf.Infinity);
        if (hits.Length/2 > maxHits)
            exploding();
    }

    private void exploding()
    {
        // mouvement d'expolsion
        RaycastHit[] hitsRange;
        hitsRange = Physics.SphereCastAll(transform.position, explosionRange, transform.up);
        foreach (RaycastHit hit in hitsRange)
        {
            if (hit.transform.CompareTag("MovingObject"))
            {
                Vector3 center = (hit.transform.position + transform.position) / 2;
                Vector3 speed = hit.transform.position - center;
                Debug.Log(speed);
                Rigidbody rb = hit.transform.GetComponent<Rigidbody>();
                rb.AddForce(speed * explosionStrength, ForceMode.Impulse);
            }
        };
        Debug.Log($"Marcon Explosion!!"); 
        explosion.Play();
        Destroy(gameObject);
        DragAndDropController.Instance.hasBeenPlaced.Remove(transform);
    }

}
