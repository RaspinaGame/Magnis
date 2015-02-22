using UnityEngine;
using System.Collections;

public class LightningController : MonoBehaviour 
{
    public float frameRate = 0.04f;
    public Texture[] lightningTextures;

    LineRenderer lineRenderer;
    float lastTimeTextureUpdated;
    public int texturesIndex = 0;

	// Use this for initialization
	void Start () 
    {
        lineRenderer = GetComponent<LineRenderer>();
        texturesIndex = Random.Range(0, lightningTextures.Length);
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (Time.time - lastTimeTextureUpdated > frameRate)
        {
            texturesIndex++;
            texturesIndex %= lightningTextures.Length;
            //  lineRenderer.materials[0].SetTexture("lightningTexture", lightningTextures[texturesIndex]);
          //  lineRenderer.materials[0].mainTexture = lightningTextures[texturesIndex];
            lineRenderer.sharedMaterials[0].mainTexture = lightningTextures[texturesIndex];
            lastTimeTextureUpdated = Time.time;
        }
	}
}
