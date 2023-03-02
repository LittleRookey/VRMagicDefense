using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Litkey.Utility;

public enum HealthSpawnPos
{
    Up,
    Down
}
public class HealthUI : MonoBehaviour
{
    public Image healthBar; // actual red healthbar
    public Image backHealthBar; // healthbar that moves white
    [SerializeField] private Health health;
    [SerializeField] private bool isOverlay;
    private HealthSpawnPos currentSpawnPos = HealthSpawnPos.Down;
    public bool lookAtPlayer;
    Vector3 upVector;
    Vector3 downVector;
    GameObject player;
    [SerializeField] bool useCustomPos;

    Vector3 minusOne = new Vector3(-1f, 1f, 1f);

    private void Awake()
    {
        transform.localScale = minusOne;
        if (health == null)
            transform.parent.TryGetComponent<Health>(out health);
        player = GameObject.FindGameObjectWithTag("Player");
        upVector = new Vector3(0f, 1.7f, 0f);
        downVector = new Vector3(0f, 0f, 0f);
    }
    private void OnEnable()
    {
        health.OnHit += UpdateHealth; 

        if (!useCustomPos)
            SetHealthBarPos(currentSpawnPos, Vector3.zero);
    }

    private void OnDisable()
    {
        health.OnHit -= UpdateHealth;
    }

    public void UpdateHealth(float fillAmount)
    {
        //Debug.Log("H");
        healthBar.fillAmount = fillAmount;
        DOTween.To(() => backHealthBar.fillAmount, x => backHealthBar.fillAmount = x, fillAmount, .3f);
    }

    public void SetHealthBarPos(HealthSpawnPos hsp, Vector3 add)
    {
        if (isOverlay) return;
        switch (hsp)
        {
            case HealthSpawnPos.Up:
                currentSpawnPos = HealthSpawnPos.Up;
                transform.localPosition = upVector + add;
                break;
            case HealthSpawnPos.Down:
                currentSpawnPos = HealthSpawnPos.Down;
                transform.localPosition = downVector + add;
                break;
        }
    }
    void LookAtPlayer()
    {
        transform.LookAt(player.transform);
        
    }
    private void Update()
    {
        if (lookAtPlayer)
        {
            LookAtPlayer();
        }
    }
}