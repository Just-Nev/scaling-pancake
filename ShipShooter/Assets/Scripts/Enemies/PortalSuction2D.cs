using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PortalSuction2D : MonoBehaviour
{
    [Header("Suction")]
    [SerializeField] private float pullForce = 25f;
    [SerializeField] private float maxPullSpeed = 15f;

    [Header("Radii")]
    [SerializeField] private float coreRadius = 0.25f;     // reach center threshold
    [SerializeField] private float holdRadius = 0.05f;     // how tightly we snap/hold at center

    [Header("Targeting")]
    [SerializeField] private LayerMask affectedLayers = ~0;

    [Header("Occupied Behavior")]
    [SerializeField] private bool stopPullingWhenOccupied = true;

    private readonly HashSet<Rigidbody2D> bodies = new();
    private readonly HashSet<Rigidbody2D> pendingAdd = new();
    private readonly HashSet<Rigidbody2D> pendingRemove = new();

    private Rigidbody2D occupyingBody; // the object sitting in the center

    private void Reset()
    {
        var col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    private bool IsOccupied => occupyingBody != null;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // If occupied and we want to stop pulling, ignore new entrants
        if (stopPullingWhenOccupied && IsOccupied) return;

        if (((1 << other.gameObject.layer) & affectedLayers) == 0)
            return;

        var rb = other.attachedRigidbody;
        if (rb == null) return;
        if (rb.gameObject == gameObject) return;

        pendingAdd.Add(rb);
        pendingRemove.Remove(rb);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var rb = other.attachedRigidbody;
        if (rb == null) return;

        pendingRemove.Add(rb);
        pendingAdd.Remove(rb);
    }

    private void FixedUpdate()
    {
        // If the occupying body got destroyed externally, clear occupancy
        if (occupyingBody == null) occupyingBody = null;

        // Apply buffered adds/removes
        if (pendingAdd.Count > 0)
        {
            foreach (var rb in pendingAdd)
                if (rb != null) bodies.Add(rb);
            pendingAdd.Clear();
        }

        if (pendingRemove.Count > 0)
        {
            foreach (var rb in pendingRemove)
                bodies.Remove(rb);
            pendingRemove.Clear();
        }

        // If occupied and we stop pulling, don't pull anything else
        if (stopPullingWhenOccupied && IsOccupied)
        {
            HoldAtCenter(occupyingBody);
            return;
        }

        if (bodies.Count == 0) return;

        Vector2 center = transform.position;
        var toRemove = ListPool<Rigidbody2D>.Get();

        foreach (var rb in bodies)
        {
            if (rb == null)
            {
                toRemove.Add(rb);
                continue;
            }

            // Don't pull the occupying body from the set (in case it is still tracked)
            if (rb == occupyingBody)
            {
                HoldAtCenter(rb);
                continue;
            }

            Vector2 pos = rb.position;
            Vector2 toCenter = center - pos;
            float dist = toCenter.magnitude;

            if (dist <= coreRadius)
            {
                // Decide: destroy or keep?
                var immune = rb.GetComponent<PortalImmune>();
                bool doNotDestroy = immune != null && immune.doNotDestroy;

                if (doNotDestroy)
                {
                    // This object stays and can occupy the portal
                    HoldAtCenter(rb);

                    bool occupy = immune.occupyPortal;
                    if (occupy)
                    {
                        occupyingBody = rb;

                        // Clear others so we stop pulling immediately (optional but clean)
                        bodies.Clear();
                        pendingAdd.Clear();
                        pendingRemove.Clear();
                        return;
                    }
                }
                else
                {
                    Destroy(rb.gameObject);
                }

                toRemove.Add(rb);
                continue;
            }

            // Pull
            Vector2 dir = toCenter / Mathf.Max(dist, 0.0001f);

            // distance-scaled pull (nice “black hole” feel)
            float scaledForce = pullForce * Mathf.Clamp01(1f / Mathf.Max(dist, 0.25f));
            rb.AddForce(dir * scaledForce, ForceMode2D.Force);

            // Clamp speed
            if (rb.linearVelocity.magnitude > maxPullSpeed)
                rb.linearVelocity = rb.linearVelocity.normalized * maxPullSpeed;
        }

        for (int i = 0; i < toRemove.Count; i++)
            bodies.Remove(toRemove[i]);

        ListPool<Rigidbody2D>.Release(toRemove);
    }

    private void HoldAtCenter(Rigidbody2D rb)
    {
        if (rb == null) return;

        Vector2 center = transform.position;

        // Snap if very close, otherwise gently pull into the exact center
        Vector2 delta = center - rb.position;
        if (delta.magnitude <= holdRadius)
        {
            rb.position = center;
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
        else
        {
            // strong pull into the exact center for the final tiny distance
            rb.AddForce(delta.normalized * pullForce * 2f, ForceMode2D.Force);
            if (rb.linearVelocity.magnitude > maxPullSpeed)
                rb.linearVelocity = rb.linearVelocity.normalized * maxPullSpeed;
        }
    }
}

public static class ListPool<T>
{
    private static readonly Stack<List<T>> pool = new();

    public static List<T> Get()
    {
        if (pool.Count > 0)
        {
            var list = pool.Pop();
            list.Clear();
            return list;
        }
        return new List<T>();
    }

    public static void Release(List<T> list) => pool.Push(list);
}



