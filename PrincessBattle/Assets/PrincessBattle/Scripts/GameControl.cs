using UnityEngine;
using UnityEngine.UI;

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
        GameObject m_Menu;
        [SerializeField]
        GameObject m_GameOver;
        [SerializeField]
        InterfaceControl m_Interface;

        [Space]

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
        Canvas m_Canvas;

        [Space]

        [SerializeField]
        GameObject m_PlayerPrefab;
        [SerializeField]
        GameObject m_EnemyPrefab;
        [SerializeField]
        GameObject m_CrownPrefab;
        [SerializeField]
        GameObject m_CountdownPrefab;

        [Space]

        [SerializeField]
        CameraControl m_CameraControl;
        [SerializeField]
        GameObject[] m_Blocks;

        CrownControl m_Crown;
        CharControl m_CrownOwner;
        GameObject m_Level;
        PlayerControl m_Player;
        EnemyControl[] m_Enemies;

        float m_SpawnInterval = 3f;
        float m_SpawnCooldown;
        float m_StartCountdown;

        bool m_GameInit;
        bool m_GameStarted;

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
            m_Menu.SetActive(true);
            m_GameOver.SetActive(false);
        }

        void Update()
        {
            if (m_GameInit)
            {
                if (m_GameStarted)
                {
                    SpawnEnemies();
                }
                else
                {
                    if (m_StartCountdown <= 0)
                    {
                        m_GameStarted = true;
                        m_Player.CanMove = true;

                        CameraControl.Instance.m_Targets = new Transform[] { m_Player.transform };
                    }

                    m_StartCountdown -= Time.deltaTime;
                }
            }
        }

        void LateUpdate()
        {
            UpdateScore();
        }

        // Instance methods:

        public void New()
        {
            if (m_Level != null)
            {
                Destroy(m_Level);
            }

            SetupLevel();

            m_GameInit = true;
            m_GameStarted = false;
            m_StartCountdown = m_SpawnInterval;

            m_Menu.SetActive(false);
            m_GameOver.SetActive(false);

            DisplayCountdown();
        }

        public void Quit()
        {
            Application.Quit();
        }

        void DisplayCountdown()
        {
            GameObject obj = Instantiate(m_CountdownPrefab, m_Canvas.transform);

            Text countdown = obj.GetComponent<Text>();

            if (m_StartCountdown > 0)
            {
                string count = Mathf.RoundToInt(m_StartCountdown) + "";

                countdown.text = count;

                Invoke("DisplayCountdown", 1f);
            }
            else
            {
                countdown.text = "Go!";
            }
        }

        void SetupLevel()
        {
            m_Level = new GameObject("Level");

            // Blocks:
            // -- Initial
            GameObject blockIni = Instantiate(m_Blocks[0], Vector3.zero, Quaternion.identity);

            blockIni.transform.parent = m_Level.transform;

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

                block.transform.parent = m_Level.transform;

                lastBlock = block;
            }

            // -- Final
            GameObject blockEnd = Instantiate(m_Blocks[1]);

            blockEnd.transform.position = lastBlock.transform.position + new Vector3(blockDistance, 0, 0);

            blockEnd.transform.parent = m_Level.transform;

            // Player:

            GameObject player = Instantiate(m_PlayerPrefab, new Vector3(0, 0.1f, 0), Quaternion.identity);

            m_Player = player.GetComponent<PlayerControl>();

            m_Player.transform.parent = m_Level.transform;

            // Crown:

            GameObject crown = Instantiate(m_CrownPrefab, Vector3.zero, Quaternion.identity);

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

                    float distance = 50f;
                    float minDistance = 20f;
                    bool valid = false;
                    int maxEntropy = 300;
                    int cycle = 0;

                    position.y = 0.1f;

                    do
                    {
                        if (cycle >= maxEntropy) break;

                        Vector2 point = Random.insideUnitCircle;

                        position.x += point.x * distance;
                        position.z += point.y * distance;

                        if (Vector3.Distance(m_Player.transform.position, position) >= minDistance)
                        {
                            RaycastHit hit;

                            valid = Physics.Raycast(position, Vector3.down, out hit, Mathf.Infinity, m_GroundLayer);
                        }

                        cycle++;
                    } while (!valid);

                    Quaternion direction = Quaternion.LookRotation(m_Player.transform.position - position, Vector3.up);

                    GameObject enemy = Instantiate(m_EnemyPrefab, position, direction);

                    enemy.transform.parent = m_Level.transform;

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
