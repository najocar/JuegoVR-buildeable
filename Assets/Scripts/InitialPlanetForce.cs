using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialPlanetForce : MonoBehaviour {
    // Start is called before the first frame update

    [SerializeField]
    private GameObject objectToAddForce;
    [SerializeField]
    private Vector3 forceDirectionVector;
    [SerializeField]
    private float minForce;
    [SerializeField]
    private float maxForce;

    void Start()
    {
        float forceMagnitude = Random.Range(minForce, maxForce);

        Vector3 force = Vector3.Normalize(forceDirectionVector) * forceMagnitude;

        objectToAddForce.GetComponent<Rigidbody>().AddForce(force);

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
