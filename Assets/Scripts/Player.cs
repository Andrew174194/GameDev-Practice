using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

//This code is originaly from Bandit accest

public class Player : MonoBehaviour {

    [SerializeField] float m_speed = 1.0f;
    [SerializeField] float m_jumpForce = 2.0f;
    [SerializeField] GameObject door;
    [SerializeField] TextMeshProUGUI countText;
    [SerializeField] TextMeshProUGUI winText;
    [SerializeField] TextMeshProUGUI deadText;

    private Animator m_animator;
    private SpriteRenderer sprite;
    private Rigidbody2D m_body2d;
    private Sensor_Bandit m_groundSensor;
    private bool m_grounded = false;
    private bool doubleJump = true;
    private bool m_combatIdle = false;
    private bool m_isDead = false;
    private bool key = false;
    private bool finished = false;
    private int coins = 0;
    


    // Use this for initialization
    void Start () {
        winText.gameObject.SetActive(false);
        deadText.gameObject.SetActive(false);
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_Bandit>();
        sprite = GetComponent<SpriteRenderer>();
        countText.text = "Money: " + (coins * 100).ToString();
    }

    private void OnTriggerEnter2D(Collider2D obj)
    {
        if (obj.tag == "Death")
        {
            m_isDead = true;
            m_animator.SetTrigger("Death");
            deadText.gameObject.SetActive(true);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            
        }

        if (obj.tag == "Coin")
        {
            obj.gameObject.SetActive(false);
            coins++;
            countText.text = "Money: " + (coins * 100).ToString();
        }
            
        if (obj.tag == "Key")
        {
            key = true;
            obj.gameObject.SetActive(false);
            door.gameObject.SetActive(false);
        }

        if (obj.tag == "Finish")
            if (key)
            {
                winText.gameObject.SetActive(true);
                finished = true;
                m_animator.SetInteger("AnimState", 0);
                m_body2d.velocity = Vector3.zero;
            }
            else Debug.Log("You need a key!");
    }

    void Update () {
        if (!finished)
        {
            if (!m_grounded && m_groundSensor.State())
            {
                m_grounded = true;
                doubleJump = true;
                m_animator.SetBool("Grounded", m_grounded);
            }

            if (m_grounded && !m_groundSensor.State())
            {
                m_grounded = false;
                m_animator.SetTrigger("Jump");
                m_animator.SetBool("Grounded", m_grounded);
            }

            // -- Handle input and movement --
            float inputX = Input.GetAxis("Horizontal");
            sprite.flipX = inputX > 0;

            // Move
            m_body2d.velocity = new Vector2(inputX * m_speed, m_body2d.velocity.y);

            //Jump
            if (Input.GetKeyDown("space") && (doubleJump))
            {
                if (m_grounded)
                {
                    m_grounded = false;
                    m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
                    m_animator.SetTrigger("Jump");
                    m_animator.SetBool("Grounded", m_grounded);
                    m_groundSensor.Disable(0.5F);
                }
                else
                {
                    doubleJump = false;
                    m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce / 2);
                }
            }

            //Run
            else if (Mathf.Abs(inputX) > 0.0F)
                m_animator.SetInteger("AnimState", 2);

            //Idle
            else
                m_animator.SetInteger("AnimState", 0);
        }
    }
}
