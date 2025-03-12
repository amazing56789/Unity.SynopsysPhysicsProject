using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(MaterialDamage))]
public class CollideWithDust : MonoBehaviour {
    [SerializeField] private ParticleSystem dustSystem;

    [SerializeField] private DeformableContactObject simulator;
    [SerializeField] private NonDeformableContactObject dustParticle;


    private List<ParticleCollisionEvent> collisionEvents;
    private MaterialDamage materialDamage;

    void Start()
    {
        collisionEvents = new List<ParticleCollisionEvent>();
        CalibrateMaterialProperties();
        materialDamage = gameObject.GetComponent<MaterialDamage>();
    }

    public void CalibrateMaterialProperties(DeformableContactObject newSimulator, NonDeformableContactObject newDust)
    {
        pMaxConst = Mathf.Pow(4f, 11/30) * Mathf.Pow(3f, 8/5)
            * Mathf.Pow(
                1/(((1 - simulator.poissonRatio*simulator.poissonRatio)/simulator.youngModulusGPa) + ((1 - dustParticle.poissonRatio*dustParticle.poissonRatio)/dustParticle.youngModulusGPa))
            , 6/5) / Mathf.PI;
    }
    public void CalibrateMaterialProperties(DeformableContactObject newSimulator)
    {
        CalibrateMaterialProperties(newSimulator, dustParticle);
    }
    public void CalibrateMaterialProperties()
    {
        CalibrateMaterialProperties(simulator, dustParticle);
    }

    // https://wp.optics.arizona.edu/optomech/wp-content/uploads/sites/53/2016/10/OPTI-521-Tutorial-on-Hertz-contact-stress-Xiaoyin-Zhu.pdf
    // F = (4(E_k^(1/3))(E_*)R); impact depth = 2E_k/F = (2(F^2)/(E^2_*))^(1/3); Pmax = 3F/(2pi(a^2))
    // 1/E_* = (1-v^2_1)/E_1 + (1-v^2_2)/E_2
    // below is (4^(11/3) * (3^(8/5)) * E^(6/5))/pi (Pmax divided by KE^(8/5))
    private float pMaxConst;
    private float maxStress;

    void OnParticleCollision()
    {
        int numEvents = dustSystem.GetCollisionEvents(gameObject, collisionEvents);
    
        foreach (ParticleCollisionEvent i in collisionEvents) {
            maxStress = pMaxConst * Mathf.Pow(i.velocity.magnitude, 8/5);
            
            if (true /*maxStress > simulator.yieldStrengthGPa*/) {
                materialDamage.ApplyDamage(maxStress, i.intersection, i.normal);
            }
        }
    }
}
