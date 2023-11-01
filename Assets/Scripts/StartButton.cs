using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Transform transform = this.transform;

        while (transform.parent != null)
        {
            if (transform.parent.tag == "StartMenu")
            {
                transform.parent.gameObject.SetActive(false);
            }
            transform = transform.parent;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
