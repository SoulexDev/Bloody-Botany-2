using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TimeExtensions
{
    public static float NormalizedDeltaTime()
    {
        return Time.deltaTime * 60;
    }
}