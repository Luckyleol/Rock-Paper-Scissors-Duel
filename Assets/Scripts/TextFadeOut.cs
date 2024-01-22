using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextFadeOut : MonoBehaviour
{
    [SerializeField]
    float fadeTime = 0.25f;
    float timer = -1;
    private void OnEnable()
    {
        timer = fadeTime;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            this.gameObject.SetActive(false);
        }

    }
}
