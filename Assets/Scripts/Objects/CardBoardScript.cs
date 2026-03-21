using System;
using System.Collections.Generic;
using UnityEngine;

public class CardBoardScript : ObjectScript
{
    [SerializeField] int maxHits = 5;
    RaycastHit[] hits;
    [SerializeField] float explosionRange = 1f;
    [SerializeField] float explosionStrength = 10f;
    public override void fakeUpdate()
    {
        hits = Physics.RaycastAll(transform.position, Vector3.up, Mathf.Infinity);
        if (hits.Length/2 > maxHits)
            exploding();
    }

    private void exploding()
    {
        Destroy(gameObject);
        // Créer particule
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
        }
    }

}
