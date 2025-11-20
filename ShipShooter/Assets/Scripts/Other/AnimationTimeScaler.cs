using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTimeScaler : MonoBehaviour
{
    public float animationTimeScale = 1f;

    private void Update()
    {
        Animator animator = GetComponent<Animator>();
        animator.speed = animationTimeScale;
    }
}
