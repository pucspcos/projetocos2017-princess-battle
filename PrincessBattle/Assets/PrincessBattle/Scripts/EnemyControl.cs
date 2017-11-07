using UnityEngine;

namespace PrincessBattle
{
    public class EnemyControl : CharControl
    {
        static readonly string[] m_FirstNames = { "Stegofino", "Marsupan", "Givanildo" };
        static readonly string[] m_SurNames = { "Arrombagildo", "Estrobiarus", "Dubiaurus" };
        static readonly Color[] m_Colors = {
            Color.magenta,
            Color.yellow,
            Color.cyan
        };

        [SerializeField]
        float m_ViewRange = 100f;

        bool m_PlayerSeen;

        float m_RunningCountdown = 5f;
        float m_RunningTimeout;

        override protected void Awake()
        {
            base.Awake();

            m_CharName = m_FirstNames[Random.Range(0, m_FirstNames.Length)] + " " + m_SurNames[Random.Range(0, m_SurNames.Length)];
            m_Material.color = m_Colors[Random.Range(0, m_Colors.Length)];
        }

        override protected void Update()
        {
            Quaternion direction = Quaternion.LookRotation(m_Target - transform.position, Vector3.up);

            transform.rotation = direction;

            if (!m_Crowned)
            {
                Move();
            }
            else
            {
                Run();
            }

            base.Update();
        }

        void Move()
        {
            LookForTheCrown();
        }

        void Run()
        {
            RunFromOthers();
        }

        void LookForTheCrown()
        {
            float distance = Vector3.Distance(CrownControl.Instance.transform.position, transform.position);

            if (distance <= m_ViewRange)
            {
                m_PlayerSeen = true;

                m_Target = CrownControl.Instance.transform.position;
                m_Target.y = m_GroundedDistance;
            }
        }

        void RunFromOthers()
        {
            if (m_RunningTimeout <= 0)
            {
                float distance = (Random.value * 20f) + 10f;

                m_Target = new Vector3(Random.insideUnitCircle.x * distance, m_GroundedDistance, Random.insideUnitCircle.y * distance);

                m_RunningTimeout = m_RunningCountdown;
            }
            else
            {
                m_RunningTimeout -= Time.deltaTime;
            }
        }
    }
}
