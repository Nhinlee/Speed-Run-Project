using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lazer : MonoBehaviour
{
    [SerializeField]
    private float maxDistanceOfLazer = 100f;

    [SerializeField]
    private List<LineRenderer> lineRenderers;

    [SerializeField]
    [Tooltip("Collide with layers")]
    private LayerMask colliderLayer;

    void Update()
    {
        ShootLazer();
    }

    private void ShootLazer()
    {
        // Giai Phuong trinh de co dc 3 vector cach nhau 120 do
        var a = transform.up.x;
        var b = transform.up.y;
        
        float x, y, x2, y2;
        if (a == 0)
        {
            y = x2 = (-1.0f / 2) / b;
            x = y2 = Mathf.Sqrt(1 - y * y);
        }
        else if (b == 0)
        {
            x = x2 = (-1 / 2) / a;
            y = y2 = Mathf.Sqrt(1 - x * x);
        }
        else
        {
            x = (-a + Mathf.Sqrt(4 * Mathf.Pow(b, 4) + 4 * a * a * b * b - b * b)) / (2 * (a * a + b * b));
            x2 = (-a - Mathf.Sqrt(4 * Mathf.Pow(b, 4) + 4 * a * a * b * b - b * b)) / (2 * (a * a + b * b));
            y = (-1.0f / 2 - a * x) / b;
            y2 = (-1.0f / 2 - a * x2) / b;
        }

        Shoot(transform.up, 0);
        Shoot(new Vector2(x, y), 1);
        Shoot(new Vector2(x2, y2), 2);
    }

    private void Shoot(Vector2 direction, int lineIndex)
    {
        var hit = Physics2D.Raycast(transform.position, direction, maxDistanceOfLazer, colliderLayer);
        if (hit)
        {
            var o = hit.collider.gameObject;
            if (o.CompareTag("Player"))
            {
                o.GetComponent<SpeedBoyController>().Die();
            }
            DrawLazerLine(lineRenderers[lineIndex], transform.position, hit.point);
        }
        else
        {
            DrawLazerLine(lineRenderers[lineIndex], transform.position, direction * maxDistanceOfLazer);
        }
    }

    private void DrawLazerLine(LineRenderer line, Vector2 startPos, Vector2 endPos)
    {
        line.SetPosition(0, startPos);
        line.SetPosition(1, endPos);
    }
}
