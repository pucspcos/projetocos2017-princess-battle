using UnityEngine;

namespace PrincessBattle
{
    public class CrownControl : MonoBehaviour
    {
        static CrownControl m_Instance;

        public static CrownControl Instance
        {
            get { return m_Instance; }
        }

        CharControl m_Owner;

        public CharControl Owner
        {
            get { return m_Owner; }
            set
            {
                m_Owner = value;

                gameObject.transform.position = m_Owner.CrownSpot.position;
                gameObject.transform.parent = m_Owner.CrownSpot;
            }
        }

        void Awake()
        {
            m_Instance = this;
        }
    }
}
