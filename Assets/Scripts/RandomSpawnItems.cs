
using System.Collections;
using UnityEngine;

public class RandomSpawnItems : MonoBehaviour
{
    [SerializeField] private Player player;
    private float maxInterval=1f;
    private float minInterval=0.08f;
    private float currentInterval;
    private BoxCollider2D box;

    private void Awake()
    {
        box = GetComponent<BoxCollider2D>();
    }
    private void Start()
    {
        currentInterval = maxInterval;
        StartGame();
    }
    public void StartGame()
    {
        StartCoroutine(RandomSpawn());
        StartCoroutine(ChangeInterval());
    }
    public IEnumerator RandomSpawn()
    {
        GameObject cobalt= Resources.Load<GameObject>("Prefabs/Cobalt");
        GameObject bomb=   Resources.Load<GameObject>("Prefabs/Bomb");
        GameObject health= Resources.Load<GameObject>("Prefabs/Health");
        GameObject bonusScore= Resources.Load<GameObject>("Prefabs/BonusScore");
        while (player.currentHealth > 0)
        {
            ItemFall newItem = null;
            float randomPosX = 0f;
            float pos = transform.position.x - box.bounds.size.x / 2;
            float size = box.bounds.size.x;

            try
            {
                int random = Random.Range(1, 201);
                GameObject objToSpawn;
                if (random <= 190) objToSpawn = cobalt;
                else if (random <= 195) objToSpawn = bonusScore;
                else if (random <= 199) objToSpawn = bomb;
                else objToSpawn = health;

                randomPosX = Random.Range(pos, pos + size);
                newItem = Instantiate(objToSpawn, new Vector2(randomPosX, transform.position.y), Quaternion.identity).GetComponent<ItemFall>();
                Vector3 localScale = newItem.transform.localScale;
                float randomScale = Random.Range(-15f, 15.01f);
                localScale.x += ((localScale.x / 100) * randomScale);
                localScale.y += ((localScale.y / 100) * randomScale);
                newItem.transform.localScale = localScale;
            }
            catch (System.Exception ex)
            {
                Debug.LogWarning("Spawn error: " + ex.Message);
            }

            yield return null;

            if (newItem != null)
            {
                float itemWidth = newItem.GetSize().x;
                randomPosX = Mathf.Clamp(randomPosX, pos + itemWidth / 2, pos + size - itemWidth / 2);
                newItem.transform.position = new Vector2(randomPosX, newItem.transform.position.y);

                float targetFallSpeed = 1f / currentInterval * 1.2f;
                newItem.fallSpeed = Mathf.Lerp(newItem.fallSpeed, targetFallSpeed, Time.deltaTime * 1.5f);
            }
            yield return new WaitForSeconds(currentInterval);
        }

    }
    private IEnumerator ChangeInterval()
    {
        while (player.currentHealth > 0 && currentInterval > minInterval)
        {
            yield return new WaitForSeconds(0.1f);
            currentInterval = Mathf.Clamp(currentInterval - 0.0017f, minInterval, maxInterval);
        } 
    }
}
