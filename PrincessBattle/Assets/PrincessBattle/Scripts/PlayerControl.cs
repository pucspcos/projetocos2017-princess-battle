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
            if (m_CanMove)
            {
                Move();

                Fire();
            }

            base.Update();
        }

        void Move()
        {
            float xInput = Input.GetAxis("Horizontal1");
            float yInput = Input.GetAxis("Vertical1");

            transform.Rotate(Vector3.up, xInput * m_AngularVelocity * Time.deltaTime);

            m_Target = transform.position + (transform.forward * yInput * MoveSpeed);
            m_Target.y = m_GroundedDistance;
        }

        void Fire()
        {
            if (m_CanFire <= 0f)
            {
                if (Input.GetAxis("Fire1") > 0f)
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
