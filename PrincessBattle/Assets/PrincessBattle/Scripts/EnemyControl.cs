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
        float m_ViewRange = 10f;

        PlayerControl m_Player;
        bool m_PlayerSeen;

        Vector3 m_TargetPosition;

        override protected void Awake()
        {
            base.Awake();

            m_CharName = m_FirstNames[Random.Range(0, m_FirstNames.Length)] + " " + m_SurNames[Random.Range(0, m_SurNames.Length)];
            m_Player = GameControl.Instance.Player;
            m_Material.color = m_Colors[Random.Range(0, m_Colors.Length)];
        }

        override protected void Update()
        {
            base.Update();

            if (!m_Crowned)
            {
                if (m_PlayerSeen)
                {

                }
                else
                {
                    VerifyForPlayer();
                }

                Move();
            }
            else
            {
                Run();
            }
        }

        void Move()
        {

        }

        void Run()
        {

        }

        void VerifyForPlayer()
        {
            float distance = Vector3.Distance(m_Player.transform.position, transform.position);

            if (distance <= m_ViewRange)
            {
                m_PlayerSeen = true;
            }
        }
    }
}
