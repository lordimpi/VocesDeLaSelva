using System;
using UnityEngine;

public class PlayerEvents : MonoBehaviour
{
    public static Action<float, float> HealthUpdate;
    public static Action Death;
    public static Action Revive;
}