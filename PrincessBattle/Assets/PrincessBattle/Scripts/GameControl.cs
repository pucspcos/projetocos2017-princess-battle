using UnityEngine;

namespace PrincessBattle
{
    public class GameControl : MonoBehaviour
    {
        static GameControl m_Instance;

        public static GameControl Instance
        {
            get { return m_Instance; }
        }

        [SerializeField]
        int m_LevelRooms = 10;
        [SerializeField]
        int m_MaxSimultaneouslyEnemies = 5;
        [SerializeField]
        int m_InitialWave = 1;
        [SerializeField]
        int m_Score;
        [SerializeField]
        LayerMask m_GroundLayer;

        [Space]

        [SerializeField]
        GameObject m_PlayerPrefab;
        [SerializeField]
        GameObject m_EnemyPrefab;
        [SerializeField]
        GameObject m_CrownPrefab;

        [Space]

        [SerializeField]
        GameObject[] m_Blocks;

        CrownControl m_Crown;
        CharControl m_CrownOwner;

        PlayerControl m_Player;
        EnemyControl[] m_Enemies;

        float m_SpawnInterval = 3f;
        float m_SpawnCooldown;

        public PlayerControl Player
        {
            get { return m_Player; }
        }

        // MonoBeahviour:

        void Awake()
        {
            if (m_Instance != null)
            {
                Destroy(gameObject);

                return;
            }

            m_Enemies = new EnemyControl[m_MaxSimultaneouslyEnemies];

            m_Instance = this;

            DontDestroyOnLoad(gameObject);
        }

        void Start()
        {
            SetupLevel();
        }

        void Update()
        {
            SpawnEnemies();
        }

        void LateUpdate()
        {
            UpdateScore();
        }

        // Instance methods:

        void SetupLevel()
        {
            // Blocks:
            // -- Initial
            GameObject blockIni = Instantiate(m_Blocks[0], Vector3.zero, Quaternion.identity);

            // -- Intermediares
            GameObject lastBlock = null;
            float blockDistance = 100f;

            for (int i = 0; i < m_LevelRooms; i++)
            {
                GameObject block;

                if (i % 2 == 0)
                {
                    block = Instantiate(m_Blocks[2]);
                }
                else
                {
                    block = Instantiate(m_Blocks[3]);
                }

                if (lastBlock != null)
                {
                    if (i % 2 == 0)
                    {
                        block.transform.position = lastBlock.transform.position + new Vector3(blockDistance * lastBlock.transform.localScale.x, 0, 0);

                        block.transform.localScale = new Vector3(1, 1, Random.value > 0.5f ? 1 : -1);
                    }
                    else
                    {
                        block.transform.position = lastBlock.transform.position + new Vector3(0, 0, blockDistance * lastBlock.transform.localScale.z);

                        block.transform.localScale = new Vector3(1, 1, lastBlock.transform.localScale.z);
                    }
                }
                else
                {
                    block.transform.position = new Vector3(blockDistance, 0, 0);

                    block.transform.localScale = new Vector3(1, 1, Random.value > 0.5f ? 1 : -1);
                }

                lastBlock = block;
            }

            // -- Final
            GameObject blockEnd = Instantiate(m_Blocks[1]);

            blockEnd.transform.position = lastBlock.transform.position + new Vector3(blockDistance, 0, 0);

            // Player:

            GameObject player = Instantiate(m_PlayerPrefab, new Vector3(0, 1, 0), Quaternion.identity);

            m_Player = player.GetComponent<PlayerControl>();

            // Crown:

            GameObject crown = Instantiate(m_CrownPrefab);

            m_Crown = crown.GetComponent<CrownControl>();

            m_Crown.Owner = m_Player;
        }

        void SpawnEnemies()
        {
            if (m_SpawnCooldown > 0)
            {
                m_SpawnCooldown -= Time.deltaTime;

                return;
            }

            for (int i = 0; i < m_MaxSimultaneouslyEnemies; i++)
            {
                if (m_Enemies[i] == null)
                {
                    Vector3 position = m_Player.transform.position;
                    Quaternion direction = Quaternion.LookRotation(position);

                    float distance = 30f;
                    bool valid;
                    int maxEntropy = 300;
                    int cycle = 0;

                    position.y = 1f;

                    do
                    {
                        if (cycle >= maxEntropy) break;

                        RaycastHit hit;

                        Vector2 point = Random.insideUnitCircle;

                        position.x += point.x * distance;
                        position.z += point.y * distance;

                        valid = Physics.Raycast(position, Vector3.down, out hit, Mathf.Infinity, m_GroundLayer);

                        cycle++;
                    } while (!valid);

                    GameObject enemy = Instantiate(m_EnemyPrefab, position, direction);

                    m_Enemies[i] = enemy.GetComponent<EnemyControl>();

                    break;
                }
            }

            m_SpawnCooldown = m_SpawnInterval;
        }

        void UpdateScore()
        {

        }
    }
}
