using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int maxHealth { get; private set; } = 3;
    public int currentHealth { get; private set; }
    public int score { get; private set; } = 0;
    private float interval = 1f;
    [SerializeField] private PlayerMovement movement;
    [SerializeField] private Color flashColor;
    [SerializeField] private GameObject GameOverPanel;
    private Animator animator;
    private bool isInvincible = false;  
    private float invincibilityDuration =0.5f;

    private SpriteRenderer spriteRenderer;
    [SerializeField] private Database db;
    private Database.Contestant currentPlayer;
    private void Awake()
    {
        currentHealth = maxHealth;
        StartCoroutine(AddScoreRoutine());
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public bool AddHealth(int amount = 1)
    {
        int currentHealth = this.currentHealth;
        this.currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        return currentHealth != this.currentHealth ? true : false;
    }
    public bool RemoveHealth(int amount = 1)
    {
        if (isInvincible || currentHealth <= 0)
            return false;

        currentHealth = Mathf.Clamp(currentHealth - amount, 0, maxHealth);
        StartCoroutine(Camera.main.GetComponent<CameraShake>().Shake());
        if (currentHealth <= 0)
        {
            GameOver();
        }
        else
        {
            StartCoroutine(InvincibilityCooldown());
        }

        return true;
    }
    private IEnumerator InvincibilityCooldown()
    {
        isInvincible = true;
        Color originalColor=spriteRenderer.color;
        float elapsedTime = 0f;
        while (elapsedTime < invincibilityDuration)
        {
            spriteRenderer.color = (elapsedTime % 0.05f < 0.025f) ? flashColor : originalColor;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        spriteRenderer.color = originalColor;
        isInvincible = false;
    }
    private void GameOver()
    {
        currentPlayer=StartPlay.player;
        try
        {
            string jsonBody = "{\"score\": \"" + score + "\"}";
            Debug.Log(currentPlayer.email+" "+currentPlayer.idmonth);
            StartCoroutine(db.PutContestant(currentPlayer, jsonBody));
        }
        catch (Exception error)
        {
            Debug.Log(" ok"+error);
        }
        ItemFall[] items = GameObject.FindObjectsByType<ItemFall>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        foreach (ItemFall item in items)
        {
            item.fallSpeed = 0;
            item.enabled = false;
        }
        movement.enabled = false;
        animator.SetBool("isDead", true);
        GameOverPanel.SetActive(true);
    }
    
    private IEnumerator AddScoreRoutine()
    {
        while (currentHealth > 0)
        {
            yield return new WaitForSeconds(interval);
            if(currentHealth >0 ) score++;
        }
    }
    public void AddScore(int amount)
    {
        score += amount;
    }
    /*public void ReceiveData(Database.Contestant contestant) { 
        currentPlayer=contestant;
        Debug.Log(contestant.email+" "+contestant.idmonth);
    }*/
}
