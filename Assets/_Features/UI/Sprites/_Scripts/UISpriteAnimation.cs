using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISpriteAnimation : MonoBehaviour
{
    public Image image;
    public List<Sprite> sprites;
    public float animSpeed = 1;
    private int index;
    private bool isDone;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartAnim());
    }

    IEnumerator StartAnim()
    {
        while (true)
        {
            yield return new WaitForSeconds(1/animSpeed);
            index++;
            if (index >= sprites.Count)
                index = 0;
            else
                image.sprite = sprites[index];
        }
    }
}
