using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcess : MonoBehaviour
{
    private PostProcessVolume postProcess;
    public static PostProcess instance;
    private bool activated = false;
    private void Update()
    {
        if (activated && postProcess.weight != 1)
        {
            postProcess.weight = Mathf.Lerp(postProcess.weight, 1, 10 * Time.deltaTime);
        }
        else if (!activated && postProcess.weight != 0)
        {
            postProcess.weight = Mathf.Lerp(postProcess.weight, 0, 10 * Time.deltaTime);
            if (postProcess.weight < 0.01f)
                postProcess.enabled = false;
        }
    }

    private void Awake()
    {
        instance = this;
        postProcess = gameObject.GetComponent<PostProcessVolume>();
    }

    public void Activate()
    {
        postProcess.weight = 0;
        activated = true;
        postProcess.enabled = true;
    }

    public void Deactivate() 
    {
        activated = false;
    }
}
