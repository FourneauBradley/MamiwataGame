using UnityEngine;
[RequireComponent (typeof(Rigidbody2D))]
[RequireComponent (typeof(BoxCollider2D))]
public class ItemFall : MonoBehaviour
{
    public float fallSpeed = 5f;
    private Rigidbody2D rb;
    private BoxCollider2D box;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        box=GetComponent<BoxCollider2D>();
        box.isTrigger = true;
    }
    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x,-fallSpeed);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (tag == "Health")
            {
                Player player=GameObject.FindAnyObjectByType<Player>();
                if (!player.AddHealth())
                {
                    player.AddScore(10);
                }
            }
            else if(tag == "Cobalt")
            {
                Player player = GameObject.FindAnyObjectByType<Player>();
                if (!player.RemoveHealth())
                {
                    return;
                }
            }
            else if(tag == "Bomb")
            {
                ItemFall[] items = GameObject.FindObjectsByType<ItemFall>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
                int scoreAmount = 0;
                foreach (ItemFall item in items) {
                    if (item.tag == "Cobalt")
                    {
                        scoreAmount++;
                        GameObject prefabToSpawn = Resources.Load<GameObject>("Prefabs/Explosion");
                        Instantiate(prefabToSpawn, item.transform.position, Quaternion.identity, null);
                        Destroy(item.gameObject);
                    }
                }
                Player player = GameObject.FindAnyObjectByType<Player>();
                player.AddScore(scoreAmount);
            }
            Destroy(this.gameObject);
        }
        if(collision.tag == "Destroyer")
        {
            Destroy(this.gameObject);
        }
    }
    public Vector2 GetSize()
    {
        return box.bounds.size;
    }
}
