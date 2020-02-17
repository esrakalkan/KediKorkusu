using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatBehaviourScript : MonoBehaviour
{
    static Animator anim;
    public float dist;
    public GameObject other;
    private float counter;


    void Start()
    {
        anim = GetComponent<Animator>();
        counter = 0;
    }

    // Update is called once per frame
    void Update()
    {
       
        dist = Vector3.Distance(other.transform.position, transform.position);
        
        
        if(dist > 5.0f)
        {
            transform.Translate(0, 0, 2 * Time.deltaTime);
            anim.SetBool("IsWalking", true);
        }
        else
        {
            anim.SetBool("IsWalking", false);
        }

        
    }
}
