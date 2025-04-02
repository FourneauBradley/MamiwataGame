using System.Collections.Generic;
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
        try
        {
            if (collision.tag == "Player")
            {
                if (tag == "Health")
                {
                    Player player = GameObject.FindAnyObjectByType<Player>();
                    if (!player.AddHealth())
                    {
                        player.AddScore(5);
                    }
                }
                else if (tag == "BonusScore")
                {
                    Player player = GameObject.FindAnyObjectByType<Player>();
                    player.AddScore(10);
                }
                else if (tag == "Cobalt")
                {
                    Player player = GameObject.FindAnyObjectByType<Player>();
                    if (!player.RemoveHealth())
                    {
                        return;
                    }
                }
                else if (tag == "Bomb")
                {
                    ItemFall[] items = GameObject.FindObjectsByType<ItemFall>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
                    int scoreAmount = 0;
                    List<ItemFall> toDestroy = new List<ItemFall>();

                    foreach (ItemFall item in items)
                    {
                        if (item.tag == "Cobalt")
                        {
                            toDestroy.Add(item);
                        }
                    }
                    GameObject prefabToSpawn = Resources.Load<GameObject>("Prefabs/Explosion");
                    foreach (ItemFall item in toDestroy)
                    {
                        scoreAmount++;
                        if (prefabToSpawn == null) continue;

                        GameObject explosion = Instantiate(
                            prefabToSpawn,
                            item.transform.position,
                            Quaternion.Euler(0, 0, Random.Range(0, 361)),
                            null);

                        Vector3 localScale = explosion.transform.localScale;
                        float randomScale = Random.Range(-15f, 15.01f);
                        localScale.x += ((localScale.x / 100) * randomScale);
                        localScale.y += ((localScale.y / 100) * randomScale);
                        explosion.transform.localScale = localScale;
                        explosion.GetComponent<Animator>().speed = Random.Range(0.5f, 4f);

                        Destroy(item.gameObject);
                    }
                    Player player = GameObject.FindAnyObjectByType<Player>();
                    player.AddScore(scoreAmount%8);
                }
                Destroy(this.gameObject);
            }else if (collision.tag == "Destroyer")
                {
                    Destroy(this.gameObject);
                }
        }catch(System.Exception e)
        {
            Debug.LogException(e);
        }
        
    }
    public Vector2 GetSize()
    {
        return box.bounds.size;
    }
}
