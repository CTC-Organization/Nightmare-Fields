using UnityEngine;

public class testeiNI : MonoBehaviour
{

    public Animator anim;
    private bool HasGun = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HasGun = false;
        anim.SetBool("HasGun", HasGun);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
