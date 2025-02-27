using UnityEngine;

public class Item : MonoBehaviour
{
    public enum ItemType { PowerUp, Seed }
    public ItemType type;
    public Sprite image;
}
