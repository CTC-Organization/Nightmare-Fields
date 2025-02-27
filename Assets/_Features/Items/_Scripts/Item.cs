using UnityEngine;

public class Item : MonoBehaviour
{
    public enum ItemType { PowerUp, Seed, Oil }
    public ItemType type;
    public Sprite image;
}
