using UnityEngine;

namespace PrincessBattle
{
    public class SelfDestructible : MonoBehaviour
    {
        [SerializeField]
        float m_RemoveAfter = 2f;

        bool m_Removing;

        void Update()
        {
            if (m_Removing == false)
            {
                m_Removing = true;

                Invoke("SelfDestruct", m_RemoveAfter);
            }
        }

        void SelfDestruct()
        {
            CancelInvoke();

            Destroy(gameObject);
        }
    }
}
