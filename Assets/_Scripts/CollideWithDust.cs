using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MaterialDamage))]
public class CollideWithDust : MonoBehaviour {
    [SerializeField] private ParticleSystem dustSystem;

    [SerializeField] private DeformableContactObject simulator;
    [SerializeField] private NonDeformableContactObject dustParticle;


    private List<ParticleCollisionEvent> collisionEvents = new();
    private MaterialDamage materialDamage;

    void Start()
    {
        CalibrateMaterialProperties();
        materialDamage = GetComponent<MaterialDamage>();
    }
    public void CalibrateMaterialProperties(DeformableContactObject newSimulator, NonDeformableContactObject newDustParticle)
    {
        simulator = newSimulator;
        dustParticle = newDustParticle;
        dTotalConst = simulator.calibrationConstant * dustParticle.mass / (2 * simulator.hardness);
    }
    public void CalibrateMaterialProperties(DeformableContactObject newSimulator)
    {
        CalibrateMaterialProperties(newSimulator, dustParticle);
    }
    public void CalibrateMaterialProperties()
    {
        CalibrateMaterialProperties(simulator, dustParticle);
    }

    private float dTotalConst;
    private float volumeLoss;
    void OnParticleCollision()
    {
        int numEvents = dustSystem.GetCollisionEvents(gameObject, collisionEvents);
    
        for (int i = 0; i < numEvents; i++) {
            volumeLoss = dTotalConst * collisionEvents[i].velocity.sqrMagnitude * Random.Range(dustParticle.minSlide, dustParticle.maxSlide);    
            materialDamage.ApplyDamage(volumeLoss, collisionEvents[i].intersection);
        }
    }
}
