using UnityEngine;
using System.Collections.Generic;

// Written by Steve Streeting 2017
// License: CC0 Public Domain http://creativecommons.org/publicdomain/zero/1.0/

/// <summary>
/// Component which will flicker a linked light while active by changing its
/// intensity between the min and max values given. The flickering can be
/// sharp or smoothed depending on the value of the smoothing parameter.
///
/// Just activate / deactivate this component as usual to pause / resume flicker
/// </summary>
public class LightFlickerEffect : MonoBehaviour
{
    //[Tooltip("External light to flicker; you can leave this null if you attach script to a light")]
    //public new Light light;
    private Light light;

    [Tooltip("Minimum random light range")]
    public float minRange = 0f;
    [Tooltip("Maximum random light range")]
    public float maxRange = 1f;

    
    //[Tooltip("Minimum random light intensity")]
    //public float minIntensity = 0f;
    //[Tooltip("Maximum random light intensity")]
    //public float maxIntensity = 1f;

    [Tooltip("How much to smooth out the randomness; lower values = sparks, higher = lantern")]
    [Range(1, 100)]
    public int smoothing = 5;

    // Continuous average calculation via FIFO queue
    // Saves us iterating every time we update, we just change by the delta
    Queue<float> smoothQueueIntensity;
    Queue<float> smoothQueueRange;
    float lastSum = 0;


    /// <summary>
    /// Reset the randomness and start again. You usually don't need to call
    /// this, deactivating/reactivating is usually fine but if you want a strict
    /// restart you can do.
    /// </summary>
    public void Reset()
    {
        smoothQueueIntensity.Clear();
        smoothQueueRange.Clear();
        lastSum = 0;
    }

    void Start()
    {
        smoothQueueIntensity = new Queue<float>(smoothing);
        smoothQueueRange = new Queue<float>(smoothing);
        // External or internal light?
        if (light == null)
        {
            light = this.GetComponent<Light>();
        }
    }

    void Update()
    {
        if (light == null)
            return;

        // pop off an item if too big
        while (smoothQueueIntensity.Count >= smoothing)
        {
            lastSum -= smoothQueueIntensity.Dequeue();
        }

        // pop off an item if too big
        while (smoothQueueRange.Count >= smoothing)
        {
            lastSum -= smoothQueueRange.Dequeue();
        }

        //// Generate random new item, calculate new average
        //float newVal1 = Random.Range(minIntensity, maxIntensity);
        //smoothQueueIntensity.Enqueue(newVal1);
        //lastSum += newVal1;

        //// Calculate new smoothed average
        //light.intensity = lastSum / (float)smoothQueueIntensity.Count;

        // Generate random new item, calculate new average
        float newVal2 = Random.Range(minRange, maxRange);
        smoothQueueRange.Enqueue(newVal2);
        lastSum += newVal2;

        // Calculate new smoothed average
        light.range = lastSum / (float)smoothQueueRange.Count;
    }

}