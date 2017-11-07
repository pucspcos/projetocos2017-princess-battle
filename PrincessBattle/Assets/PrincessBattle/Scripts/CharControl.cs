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
        [SerializeField]
        protected Rigidbody m_Rigidbody;

        protected Material m_Material;
        protected Color m_MaterialColor;
        protected float m_GroundedDistance = .1f;
        protected bool m_Grounded;
        protected Transform m_Ground;
        protected float m_MoveSpeed = 10f;
        protected float m_MovementAdjustmentWhenCrowned = 1.1f;
        protected bool m_Crowned;
        protected Vector3 m_Velocity = Vector3.zero;
        protected float m_AngularVelocity = 90f;

        protected Vector3 m_Target;

        protected float m_FreezeCountdown = 2f;
        protected float m_FreezeTimeout;

        public string CharName
        {
            get { return m_CharName; }
        }

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

        public bool Crowned
        {
            get { return m_Crowned; }
            set { m_Crowned = value; }
        }

        virtual protected void Awake()
        {
            m_Material = m_MeshRenderer.material;
            m_Target = transform.position;
        }

        virtual protected void Start()
        {
            m_MaterialColor = m_Material.color;
        }

        virtual protected void FixedUpdate()
        {
            VerifyGround();
        }

        virtual protected void Update()
        {
            if (m_FreezeTimeout <= 0)
            {
                m_Material.color = m_MaterialColor;

                transform.position = Vector3.MoveTowards(transform.position, m_Target, (MoveSpeed * Time.deltaTime));
                transform.position += m_Velocity;
            }
            else
            {
                m_Material.color = Color.Lerp(m_MaterialColor, Color.red, Mathf.PingPong(Time.time, 1));

                m_FreezeTimeout -= Time.deltaTime;
            }
        }

        bool VerifyGround()
        {
            RaycastHit hit;

            m_Grounded = Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, m_GroundLayer);

            if (m_Grounded)
            {
                m_Ground = hit.transform;
            }

            return m_Grounded;
        }

        void OnCollisionEnter(Collision collision)
        {
            if (m_Crowned && m_FreezeTimeout <= 0)
            {
                CharControl charControl = collision.gameObject.GetComponent<CharControl>();

                if (charControl != null)
                {
                    Debug.Log("Collided with " + charControl.m_CharName);

                    CrownControl.Instance.Owner = charControl;

                    charControl.Crowned = true;

                    m_FreezeTimeout = m_FreezeCountdown;
                }
            }
        }
    }
}
