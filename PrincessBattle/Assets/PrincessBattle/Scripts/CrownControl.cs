using UnityEngine;

namespace PrincessBattle
{
    public class CrownControl : MonoBehaviour
    {
        CharControl m_Owner;

        public CharControl Owner
        {
            get { return m_Owner; }
            set
            {
                gameObject.transform.parent = m_Owner.CrownSpot;

                m_Owner = value;
            }
        }
    }
}
