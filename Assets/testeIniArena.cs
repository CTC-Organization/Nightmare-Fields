using UnityEngine;

public class testeIniArena : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   public Animator anim;
    private bool HasGun = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HasGun = true;
        anim.SetBool("HasGun", HasGun);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
