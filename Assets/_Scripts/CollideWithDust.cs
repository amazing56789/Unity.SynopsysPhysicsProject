using System.Collections.Generic;
using UnityEngine;

public class CollideWithDust : MonoBehaviour {
    [SerializeField] private ParticleSystem system;
    private List<ParticleCollisionEvent> collisionEvents;

    void Start()
    {
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    void OnParticleCollision(GameObject other)
    {
        int numEvents = other.GetComponent<ParticleSystem>().GetCollisionEvents(gameObject, collisionEvents);
    
        for (int i = 0; i < numEvents; i++) {
            // @#Debug
            // @#TODO: this sh
            Debug.Log(collisionEvents[i].velocity.magnitude);
        }
    }
}
