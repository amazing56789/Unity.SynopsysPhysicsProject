using UnityEngine;

class MaterialDamage : MonoBehaviour {
    public void ApplyDamage(float damage, Vector3 position) {
        Debug.Log(position.ToString() + ": " + damage.ToString());
    }
}  