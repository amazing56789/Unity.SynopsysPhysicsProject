using UnityEngine;

class MaterialDamage : MonoBehaviour {
    public int HeatmapWidth = 1000;
    public float maxDamage = 1000f;
    private Texture2D Heatmap;
    // 
    //Caching bc used a lot
    private Color32[] pixels;
    private float[] damages;
    private Matrix4x4 worldToLocalMatrix;
    private bool updateTextureFlag;
    private Color
        GREEN,
        RED;
    void Start()
    {
        GREEN = new Color(0, 0, 0, 0);
        RED = new Color(255, 0, 0, 0);
        Heatmap = new(HeatmapWidth, (int) (1.5f * HeatmapWidth), TextureFormat.RGBA32, false);
        pixels = Heatmap.GetPixels32();
        for (int i = 0; i < pixels.Length; i++)
            pixels[i] = GREEN;
        Heatmap.SetPixels32(pixels);
        Heatmap.Apply();
        worldToLocalMatrix = transform.worldToLocalMatrix;
        damages = new float[(int) (1.5f * HeatmapWidth * HeatmapWidth)];
        GetComponent<Renderer>().material.SetTexture("_MainTex", Heatmap);
    }
    void Update()
    {
        if (updateTextureFlag) {
            Heatmap.SetPixels32(pixels);
            Heatmap.Apply();
            GetComponent<Renderer>().material.SetTexture("_MainTex", Heatmap);
            updateTextureFlag = false;
        }
    }
    int x, y;
    public void ApplyDamage(float damage, Vector3 worldPosition)
    {
        Debug.Log(x.ToString() + ", " + y.ToString() + ": " + Heatmap.GetPixel(x, y));
        GetPixelFromLocalPosition(worldToLocalMatrix.MultiplyPoint3x4(worldPosition), out x, out y);
        // damages[x * HeatmapWidth + y] += 100 * damage / maxDamage;
        damages[x * HeatmapWidth + y] = 1f;
        pixels[x * HeatmapWidth + y] = RED;//Color32.Lerp(GREEN, RED, damages[x * HeatmapWidth + y]);
        updateTextureFlag = true;
    }

    private const float oneSixth = 1/6;
    void GetPixelFromLocalPosition(Vector3 localPosition, out int x, out int y)
    {
        // Define the UV mapping for each face of the cube
        if (localPosition.x > 0.49f) {  // 5
            x = (int) (HeatmapWidth * (localPosition.z/2f + 0.75f));
            y = (int) (1.5f * HeatmapWidth * (localPosition.y/3f + 5*oneSixth));
        } else if (localPosition.x < -0.49f) {  // 3
            x = (int) (HeatmapWidth * (-localPosition.z/2f + 0.75f));
            y = (int) (1.5f * HeatmapWidth * (localPosition.y/3f + 0.5f));
        } else if (localPosition.y > 0.49f) {  // 4
            x = (int) (HeatmapWidth * (-localPosition.z/2f + 0.25f));
            y = (int) (1.5f * HeatmapWidth * (localPosition.x/3f + 5*oneSixth));
        } else if (localPosition.z > 0.49f) {  // 2
            x = (int) (HeatmapWidth * (-localPosition.x/2f + 0.25f));
            y = (int) (1.5f * HeatmapWidth * (localPosition.y/3f + 0.5f));
        } else if (localPosition.z < 0.49f) {  // B
            x = (int) (HeatmapWidth * (localPosition.x/2 + 0.25f));
            y = (int) (1.5f * HeatmapWidth * (localPosition.y/3f + oneSixth));
        } else {  // Bottom face
            Debug.LogError("Hit on bottom");
            x = (int) (HeatmapWidth * (-localPosition.x/2f + 0.75f));
            y = (int) (1.5f * HeatmapWidth * (localPosition.z/3f + oneSixth));
        }
    }

    // #@Debug:
    void OnGUI()
    {
        if(GUI.Button(new Rect(0, 0, 100, 100), "Click me")) {
            Heatmap.SetPixel(500, 500, Color.red);
            Heatmap.Apply();
            GetComponent<Renderer>().material.mainTexture = Heatmap;
        }
    }
}  