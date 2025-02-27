using UnityEngine;

public class Plant : MonoBehaviour
{
    public Item seedItem;
    public int growthStage = 0;
    public int maxGrowthStage = 3;
    public Sprite[] growthSprites;
    private SpriteRenderer spriteRenderer;
    private bool isWatered = false;
    private int lastDayChecked = -1;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateSprite();
    }

    public void Water()
    {
        if (!isWatered && growthStage < maxGrowthStage)
        {
            isWatered = true;
        }
    }

    private void Update()
    {
        if (DayManager.dm != null && DayManager.dm.days != lastDayChecked)
        {
            lastDayChecked = DayManager.dm.days;
            OnNewDay();
        }
    }

    private void OnNewDay()
    {
        if (isWatered && growthStage < maxGrowthStage)
        {
            growthStage++;
            UpdateSprite();
            isWatered = false;
        }
    }

    private void UpdateSprite()
    {
        if (spriteRenderer != null && growthStage < growthSprites.Length)
        {
            spriteRenderer.sprite = growthSprites[growthStage];
        }
    }
}