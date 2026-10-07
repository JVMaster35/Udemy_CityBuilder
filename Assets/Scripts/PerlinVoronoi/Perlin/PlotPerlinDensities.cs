using UnityEngine;

[ExecuteInEditMode]
public class PlotPerlinDensities : MonoBehaviour
{
    [Range(1, 8)]
    [SerializeField]
    private int octaves = 2;

    [Range(0, 1000)]
    [SerializeField]
    private int xOffset = 0;
    [Range(0, 1000)]
    [SerializeField]
    private int yOffset = 0;

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

    [Range(0.0f, 1.0f)]
    [SerializeField]
    private float lowCutoff = 0;
    [Range(0.0f, 1.0f)]
    [SerializeField]
    private float mediumlowCutoff = 0;
    [Range(0.0f, 1.0f)]
    [SerializeField]
    private float mediumCutoff = 0;
    [Range(0.0f, 1.0f)]
    [SerializeField] 
    private float mediumhighCutoff = 0;
    [Range(0.0f, 1.0f)]
    [SerializeField] 
    private float highCutoff = 0;

    void OnValidate()
    {
        Texture2D texture = new Texture2D(1024, 1024);
        GetComponent<Renderer>().sharedMaterial.mainTexture = texture;

        float perlin;
        Color colour = Color.white;

        for (int y = 0; y < texture.height; y++)
        {
            for (int x = 0; x < texture.width; x++)
            {
                perlin = fBM((x + xOffset) * xScale, (y + yOffset) * yScale, octaves);

                colour = Color.black;
                if (perlin < lowCutoff) colour = new Color(0, 0, 0);
                else if (perlin < mediumlowCutoff) colour = new Color(0.4f, 0.4f, 0.4f);
                else if (perlin < mediumCutoff) colour = new Color(0.6f, 0.6f, 0.6f);
                else if (perlin < mediumhighCutoff) colour = new Color(0.8f, 0.8f, 0.8f);
                else if (perlin < highCutoff) colour = new Color(1, 1, 1);

                texture.SetPixel(x, y, colour);
            }
        }
        texture.Apply();
    }

    void OnValidate2()
    {
        Texture2D texture = new Texture2D(1024, 1024);
        GetComponent<Renderer>().sharedMaterial.mainTexture = texture;

        float perlinr;
        float perlinc;
        float perlini;
        Color colour = Color.white;

        for (int y = 0; y < texture.height; y++)
        {
            for (int x = 0; x < texture.width; x++)
            {
                perlinr = fBM((x + xOffset) * xScale, (y + yOffset) * yScale, octaves);
                perlinc = fBM((x + xOffset + 100) * xScale, (y + yOffset + 100) * yScale, octaves);
                perlini = fBM((x + xOffset + 5000) * xScale, (y + yOffset + 5000) * yScale, octaves);

                colour = Color.black;
                if (perlinr < greenCutOff) colour = Color.green;
                if (perlinc < blueCutOff) colour = Color.blue;
                if (perlini < yellowCutOff) colour = Color.yellow;

                texture.SetPixel(x, y, colour);
            }
        }
        texture.Apply();
    }


    void OnValidate3()
    {
        Texture2D texture = new Texture2D(1024, 1024);
        GetComponent<Renderer>().sharedMaterial.mainTexture = texture;

        float perlin;
        Color colour = Color.white;

        for (int y = 0; y < texture.height; y++)
        {
            for (int x = 0; x < texture.width; x++)
            {
                perlin = fBM((x + xOffset) * xScale, (y + yOffset) * yScale, octaves);

                colour = Color.black;
                if (perlin < greenCutOff) colour = Color.green;
                else if (perlin < blueCutOff) colour = Color.blue;
                else if (perlin < yellowCutOff) colour = Color.yellow;

                texture.SetPixel(x, y, colour);
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

        return total / (float)octaves;
    }
}
