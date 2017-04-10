using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class gaosimohu : MonoBehaviour 
{

    //模糊半径
    public float BlurRadius = 1.0f;
    //降分辨率
    public int downSample = 2;
    //迭代次数
    public int iteration = 1;

    private Material material;

    void Start()
    {
        LoadShaders();
    }

    private void LoadShaders()
    {
        if (!material)
        {
            string name = "Custom/GaussianBlur";
            Shader shader = Shader.Find(name);
            if (shader == null)
            {
                Debug.LogError("Load shader failed, filename=" + name);
            }

            material = new Material(shader);
        }
    }

	// Use this for initialization
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (material != null)
        {
            //申请RenderTexture，RT的分辨率按照downSample降低
            RenderTexture rt1 = RenderTexture.GetTemporary(source.width >> downSample, source.height >> downSample, 0, source.format);
            RenderTexture rt2 = RenderTexture.GetTemporary(source.width >> downSample, source.height >> downSample, 0, source.format);

            Graphics.Blit(source, rt1);

            for (int i = 0; i < iteration; i++)
            {
                //第一次高斯模糊，设置offsets，竖向模糊
                material.SetVector("_offsets", new Vector4(0, BlurRadius, 0, 0));
                Graphics.Blit(rt1, rt2, material);
                //第二次高斯模糊，设置offsets,横向模糊
                material.SetVector("_offsets", new Vector4(BlurRadius, 0, 0, 0));
                Graphics.Blit(rt2, rt1, material);
            }

            //将结果输出
            Graphics.Blit(rt1, destination);
            RenderTexture.ReleaseTemporary(rt1);
            RenderTexture.ReleaseTemporary(rt2);
        }
    }
}
