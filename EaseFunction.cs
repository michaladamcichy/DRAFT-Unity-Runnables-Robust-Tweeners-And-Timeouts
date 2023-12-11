using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class EaseFunction
{
    public static float EaseInSine(float x)
    {
        return 1f - Mathf.Cos((x * Mathf.PI) / 2f);
    }

    public static float EaseInOutSine(float x)
    {
        return -1f * (Mathf.Cos(Mathf.PI * x) - 1f) / 2f;
    }

    public static float EaseInQuint(float x)
    {
        return Mathf.Pow(x, 5);
    }

    public static float EaseOutQuint(float x)
    {
        return 1 - Mathf.Pow(1f - x, 5);
    }

    public static float EaseInOutQuint(float x)
    {
        return x < 0.5 ? 16 * Mathf.Pow(x, 5) : 1 - Mathf.Pow(-2 * x + 2, 5) / 2;
    }
}
