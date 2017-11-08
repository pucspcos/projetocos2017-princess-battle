using UnityEngine;
using UnityEngine.UI;

namespace PrincessBattle
{
    public class InterfaceControl : MonoBehaviour
    {
        [SerializeField]
        RectTransform m_Radar;
        [SerializeField]
        GameObject m_Crown;
        [SerializeField]
        GameObject m_Throne;
        [SerializeField]
        Text m_CrownOwner;

        RectTransform m_Transform;
        GameObject m_ThroneObjective;
        Vector2 m_ScreenSize = new Vector2(1280f, 720f);

        void Start()
        {
            m_Transform = GetComponent<RectTransform>();

            FindObjective();
        }

        void Update()
        {
            FindObjective();

            if (m_ThroneObjective == null) return;

            Camera cam = Camera.main;

            Vector3 radarPos = Vector3.zero;

            if (CrownControl.Instance.Owner.CompareTag("Player"))
            {
                m_Crown.SetActive(false);
                m_Throne.SetActive(true);

                radarPos = cam.WorldToViewportPoint(m_ThroneObjective.transform.position);

                m_CrownOwner.text = "You have the crown! Run to the throne!";
            }
            else
            {
                m_Crown.SetActive(true);
                m_Throne.SetActive(false);

                radarPos = cam.WorldToViewportPoint(CrownControl.Instance.transform.position);

                m_CrownOwner.text = "Oh no! " + CrownControl.Instance.Owner.CharName + " has the crown! Get it!";
            }

            Vector2 screenPos = new Vector2(radarPos.x * m_ScreenSize.x, radarPos.y * m_ScreenSize.y);

            screenPos.x = Mathf.Clamp(screenPos.x, 0, m_ScreenSize.x);
            screenPos.y = Mathf.Clamp(screenPos.y, 0, m_ScreenSize.y);

            m_Radar.anchoredPosition = screenPos;
        }

        public void FindObjective()
        {
            if (m_ThroneObjective != null) return;

            m_ThroneObjective = GameObject.FindWithTag("Objective");
        }
    }
}
