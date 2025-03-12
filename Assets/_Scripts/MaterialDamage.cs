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

        Debug.Log(normal.ToString());
    }

    Vector2 GetUVFromLocalPosition(Vector3 normal)
    {
        // Define the UV mapping for each face of the cube
        if (normal.x > 0.5f) {  // Right face
            return new Vector2((normal.z + 0.5f) / 1f, (normal.y + 0.5f) / 1f);
        } else if (normal.x < -0.5f) {  // Left face
            return new Vector2((-normal.z + 0.5f) / 1f, (normal.y + 0.5f) / 1f);
        } else if (normal.y > 0.5f) {  // Top face
            return new Vector2((normal.x + 0.5f) / 1f, (-normal.z + 0.5f) / 1f);
        } else if (normal.y < -0.5f) {  // Bottom face
            return new Vector2((normal.x + 0.5f) / 1f, (normal.z + 0.5f) / 1f);
        } else if (normal.z > 0.5f) {  // Front face
            return new Vector2((normal.x + 0.5f) / 1f, (normal.y + 0.5f) / 1f);
        } else {  // Back face
            return new Vector2((-normal.x + 0.5f) / 1f, (normal.y + 0.5f) / 1f);
        }
    }
}  