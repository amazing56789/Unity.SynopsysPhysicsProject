using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MaterialDamage))]
public class CollideWithDust : MonoBehaviour {
    [SerializeField] private ParticleSystem dustSystem;

    // @#TODO: add units to var names
    [Header("Material & Lunar Dust Properties")]
    [SerializeField] private float materialYieldStrength;
    public float materialYoungModulus;
    public float materialPoissonRatio;
    [SerializeField] private float dustYoungModulus;
    [SerializeField] private float dustPoissonRatio;


    private List<ParticleCollisionEvent> collisionEvents;
    private MaterialDamage materialDamage;

    void Start()
    {
        collisionEvents = new List<ParticleCollisionEvent>();
        pMaxConst = Mathf.Pow(4f, 11/30) * Mathf.Pow(3f, 8/5)
            * Mathf.Pow(
                1/(((1 - materialPoissonRatio*materialPoissonRatio)/materialYoungModulus) + ((1 - dustPoissonRatio*dustPoissonRatio)/dustYoungModulus))
            , 6/5) / Mathf.PI;
        materialDamage = gameObject.GetComponent<MaterialDamage>();
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
    
        for (int i = 0; i < numEvents; i++) {
            maxStress = pMaxConst * Mathf.Pow(collisionEvents[i].velocity.magnitude, 8/5);
            
            if (maxStress > materialYieldStrength) {
                materialDamage.ApplyDamage(maxStress, collisionEvents[i].intersection);
            }
        }
    }
}
