using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NutrientGrenadeSpawner : ThrowableSpawner
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Use();
        }
    }
}