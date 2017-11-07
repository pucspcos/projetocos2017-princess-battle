using UnityEngine;

namespace PrincessBattle
{
    public class CameraEffect : MonoBehaviour
    {
        [SerializeField]
        Material m_Material;

        Material m_MaterialInstance;

        void Awake()
        {
            m_MaterialInstance = Instantiate<Material>(m_Material);

            m_MaterialInstance.SetFloat("_OffsetPosY", 0f);
            m_MaterialInstance.SetFloat("_OffsetColor", 0.01f);
            m_MaterialInstance.SetFloat("_OffsetDistortion", 480f);
            m_MaterialInstance.SetFloat("_Intensity", 0.64f);
        }

        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            // TV noise
            float offsetNoise = m_MaterialInstance.GetFloat("_OffsetNoiseY");

            m_MaterialInstance.SetFloat("_OffsetNoiseX", Random.Range(0f, 0.6f));
            m_MaterialInstance.SetFloat("_OffsetNoiseY", offsetNoise + Random.Range(-0.03f, 0.03f));

            // Vertical shift
            float offsetPosY = m_MaterialInstance.GetFloat("_OffsetPosY");

            if (offsetPosY > 0.0f)
            {
                m_MaterialInstance.SetFloat("_OffsetPosY", offsetPosY - Random.Range(0f, offsetPosY));
            }
            else if (offsetPosY < 0.0f)
            {
                m_MaterialInstance.SetFloat("_OffsetPosY", offsetPosY + Random.Range(0f, -offsetPosY));
            }
            else if (Random.Range(0, 150) == 1)
            {
                m_MaterialInstance.SetFloat("_OffsetPosY", Random.Range(-0.5f, 0.5f));
            }

            // Channel color shift
            //float offsetColor = m_MaterialInstance.GetFloat("_OffsetColor");

            //if (offsetColor > 0.003f)
            //{
            //    m_MaterialInstance.SetFloat("_OffsetColor", offsetColor - 0.001f);
            //}
            //else if (Random.Range(0, 400) == 1)
            //{
            //    m_MaterialInstance.SetFloat("_OffsetColor", Random.Range(0.003f, 0.1f));
            //}

            // Distortion
            if (Random.Range(0, 15) == 1)
            {
                m_MaterialInstance.SetFloat("_OffsetDistortion", Random.Range(1f, 480f));
            }
            else
            {
                m_MaterialInstance.SetFloat("_OffsetDistortion", 480f);
            }

            Graphics.Blit(source, destination, m_MaterialInstance);
        }
    }
}
