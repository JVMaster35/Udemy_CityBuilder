using UnityEngine;

[ExecuteInEditMode]
public class PlotPerlin : MonoBehaviour
{
    [Range(1, 8)]
    [SerializeField]
    private int octaves = 2;

    [Range(0,1000)]
    [SerializeField]
    private int xOffest = 0;
    [Range(0, 1000)]
    [SerializeField]
    private int yOffest = 0;

    [Range(0.001f, 0.01f)]
    [SerializeField]
    private float xScale = 0.003f;
    [Range(0.001f, 0.01f)]
    [SerializeField]
    private float yScale = 0.003f;

    [Range(0.0f, 1.0f)]
    [SerializeField]
    private float greenCutOff = 0f;
    [Range(0.0f, 1.0f)]
    [SerializeField]
    private float blueCutOff = 0f;
    [Range(0.0f, 1.0f)]
    [SerializeField]
    private float yellowCutOff = 0f;

    void OnValidate()
    {
        Texture2D texture = new Texture2D(1024,1024);
        GetComponent<Renderer>().sharedMaterial.mainTexture = texture;

        float perlinR;
        float perlinC;
        float perlinI;

        Color color = Color.white;

        for(int y = 0; y < texture.height; y++)
        {
            for (int x = 0; x < texture.width; x++)
            {
                perlinR = fBM((x + xOffest) * xScale, (y + yOffest) * yScale, octaves);
                perlinC = fBM((x + xOffest + 100) * xScale, (y + yOffest + 100) * yScale, octaves);
                perlinI = fBM((x + xOffest + 5000) * xScale, (y + yOffest + 5000) * yScale, octaves);

                color = Color.purple;
                if (perlinR < greenCutOff) color = Color.green;
                if (perlinC < blueCutOff) color = Color.blue;
                if (perlinI < yellowCutOff) color = Color.yellow;

                texture.SetPixel(x, y, color);
            }
        }

        texture.Apply();
    }

    private float fBM(float x, float y, int octaves)
    {
        float total = 0;
        float frequency = 1;

        for (int i = 0; i < octaves; i++)
        {
            total += Mathf.PerlinNoise(x * frequency, y * frequency);
            frequency *= 2;
        }

        return total/(float)octaves;
    }
}
