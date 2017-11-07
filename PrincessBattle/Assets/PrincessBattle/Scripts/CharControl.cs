using UnityEngine;

namespace PrincessBattle
{
    public class CharControl : MonoBehaviour
    {
        [SerializeField]
        protected string m_CharName;
        [SerializeField]
        protected LayerMask m_GroundLayer;
        [SerializeField]
        protected Transform m_CrownSpot;
        [SerializeField]
        protected MeshRenderer m_MeshRenderer;

        protected Material m_Material;
        protected float m_GroundedDistance = .2f;
        protected bool m_Grounded;
        protected Transform m_Ground;
        protected float m_MoveSpeed = 5f;
        protected float m_MovementAdjustmentWhenCrowned = .9f;
        protected bool m_Crowned;
        protected Vector3 m_Velocity = Vector3.zero;

        public Transform CrownSpot
        {
            get { return m_CrownSpot; }
        }

        public float MoveSpeed
        {
            get
            {
                float velocity = m_MoveSpeed;

                if (m_Crowned)
                {
                    velocity *= m_MovementAdjustmentWhenCrowned;
                }

                return velocity;
            }
        }

        public Transform Ground
        {
            get { return m_Ground; }
        }

        virtual protected void Awake()
        {
            m_Material = m_MeshRenderer.material;
        }

        virtual protected void FixedUpdate()
        {
            m_Velocity = Vector3.zero;

            VerifyGround();

            Forces();
        }

        virtual protected void Update()
        {
            transform.position += m_Velocity;
        }

        bool VerifyGround()
        {
            RaycastHit hit;

            m_Grounded = Physics.Raycast(transform.position, Vector3.down, out hit, m_GroundedDistance, m_GroundLayer);

            if (m_Grounded)
            {
                m_Ground = hit.transform;
            }

            return m_Grounded;
        }

        void Forces()
        {
            // Gravity
            if (!m_Grounded)
            {
                m_Velocity += Physics.gravity * Time.fixedDeltaTime;
            }
            else
            {

            }
        }
    }
}
