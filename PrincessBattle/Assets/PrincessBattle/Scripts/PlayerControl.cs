using UnityEngine;

namespace PrincessBattle
{
    public class PlayerControl : CharControl
    {
        [SerializeField]
        float m_FireCooldown = 1f; // Seconds until next shoot
        [SerializeField]
        GameObject m_FirePrefab;

        bool m_CanMove;
        float m_CanFire;

        public bool CanMove
        {
            get { return m_CanMove; }
            set { m_CanMove = value; }
        }

        public float FireCooldown
        {
            get { return Mathf.Clamp(m_CanFire, 0f, m_FireCooldown); }
        }

        override protected void Update()
        {
            base.Update();

            if (m_CanMove)
            {
                Move();

                Fire();
            }
        }

        void Move()
        {

        }

        void Fire()
        {
            if (m_CanFire <= 0f)
            {
                if (Input.GetKeyDown("Fire"))
                {
                    m_CanFire = m_FireCooldown;
                }
            }
            else
            {
                m_CanFire -= Time.deltaTime;
            }
        }
    }
}
