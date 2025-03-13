using System.Threading.Tasks;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D body;
    private Animator animator;
    [SerializeField] private float speed;
    [SerializeField] private Parallax[] parallaxItems;
    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    void Start()
    {
        
    }
    void FixedUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        body.linearVelocity = new Vector2(horizontalInput * speed, body.linearVelocity.y);
    }
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        bool isMoving = Mathf.Abs(horizontalInput) > 0.01f;

        animator.SetBool("isMoving", isMoving);

        Vector3 localScale = transform.localScale;

        if (isMoving)
        {
            if (horizontalInput > 0 && transform.localScale.x < 0)
            {
                localScale.x = -localScale.x;
                transform.localScale = localScale;
            }
            else if (horizontalInput < 0 && transform.localScale.x > 0)
            {
                localScale.x = -localScale.x;
                transform.localScale = localScale;
            }

            // Faire bouger tous les éléments de parallax
            foreach (var parallax in parallaxItems)
            {
                parallax.Scroll(-horizontalInput); // Inverser pour donner l'effet de profondeur
            }
        }
    }
}
