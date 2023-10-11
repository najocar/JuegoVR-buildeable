using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;
using UnityEngine.InputSystem;

public class SliceObject : MonoBehaviour
{

    [SerializeField]
    private Transform startSlidePoint;
    [SerializeField]
    private Transform endSlicePoint;
    [SerializeField]
    private VelocityEstimator velocityEstimator;
    [SerializeField]
    private LayerMask sliceableLayer;


    [SerializeField]
    private float intervaloEliminacion = 6.0f;


    [SerializeField]
    private Material crossSectionMaterial;
    [SerializeField]
    private float cutForce = 2000;

    [SerializeField]
    private AudioClip sound;
    private AudioSource audioController;

    // [SerializeField]
    // private ParticleSystem particulas;



    void Start()
    {
        
    }

    void FixedUpdate()
    {
        audioController = GetComponent<AudioSource>();
        bool hasHit = Physics.Linecast(startSlidePoint.position, endSlicePoint.position, out RaycastHit hit, sliceableLayer);
        if(hasHit){
            audioController.PlayOneShot(sound);
            GameObject target = hit.transform.gameObject;
            Slice(target);

            // particulas.Play();
        }
    }

    public void Slice(GameObject target){

        Vector3 velocity = velocityEstimator.GetVelocityEstimate();
        Vector3 planeNormal = Vector3.Cross(endSlicePoint.position - startSlidePoint.position, velocity);
        planeNormal.Normalize();

        SlicedHull hull = target.Slice(endSlicePoint.position, planeNormal);

        if(hull != null){
            GameObject upperHull = hull.CreateUpperHull(target, crossSectionMaterial);
            SetupSlicedComponent(upperHull);
            Destroy(upperHull, intervaloEliminacion);

            GameObject lowerHull = hull.CreateLowerHull(target, crossSectionMaterial);
            SetupSlicedComponent(lowerHull);
            Destroy(lowerHull, intervaloEliminacion);

            Destroy(target);
        }
    }  

    public void SetupSlicedComponent(GameObject sliceObject){
        Rigidbody rb = sliceObject.AddComponent<Rigidbody>();
        rb.useGravity = false;
        MeshCollider collider = sliceObject.AddComponent<MeshCollider>();
        collider.convex = true;
        rb.AddExplosionForce(cutForce, sliceObject.transform.position, 1);
        // sliceObject.layer = LayerMask.NameToLayer("Sliceable");
    }
    
}
