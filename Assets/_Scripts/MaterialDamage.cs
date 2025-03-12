using System;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

class MaterialDamage : MonoBehaviour {
    public NonDeformableContactObject simulator;
    public int HeatmapResolution = 1024;
    public float maxDamage;
    private RenderTexture Heatmap;
    //Caching bc used a lot
    private float[][] damages;

    void Start()
    {
        Heatmap = new(HeatmapResolution, HeatmapResolution, 0, RenderTextureFormat.RGFloat)
        {
            enableRandomWrite = true
        };
        // worldToLocalMatrix = transform.worldToLocalMatrix;
        // damages = new float[HeatmapResolution * HeatmapResolution];
    }
    public void ApplyDamage(float damage, Vector3 position, Vector3 normal)
    {
        // localPosition = transform.InverseTransformPoint(position);

        // localPosition.x /= transform.localScale.x;
        // localPosition.y /= transform.localScale.y;
        // localPosition.z /= transform.localScale.z;

        // uv = GetUVFromLocalPosition(localPosition);

        // pixelCoordinates.Set(uv.x * HeatmapResolution, uv.y * HeatmapResolution);
        // damage[pixelCoordinates.x][pixelCoordinates.y] = damage[pixelCoordinates.x][pixelCoordinates.y] + damage;

        // damageTexture.SetPixel((int)pixelCoordinates.x, (int)pixelCoordinates.y, Color.Lerp(Color.green, Color.red, damage[pixelCoordinates.x]));
        // damageTexture.Apply();

        Debug.Log(transform.InverseTransformPoint(position).ToString());
    }

    Vector2 GetUVFromLocalPosition(Vector3 position, Vector3 normal)
    {
        // Define the UV mapping for each face of the cube
        if (position.x > 0.49f) {  // 5
            return new Vector2(-position.z + 0.5f, position.y + 0.5f);
        } else if (position.x < -0.49f) {  // 3
            return new Vector2(position.y + 0.5f, -position.z + 0.5f);
        } else if (position.y > 0.49f) {  // 4
            return new Vector2(position.x + 0.5f, -position.z + 0.5f);
        } else if (position.z > 0.49f) {  // 2
            return new Vector2(position.x + 0.5f, position.y + 0.5f);
        } else if (position.z < 0.49f) {  // B
            return new Vector2(-position.x + 0.5f, position.y + 0.5f);
        } else {  // Bottom face
            return new Vector2(position.x + 0.5f, position.z + 0.5f);
        }
    }
}  