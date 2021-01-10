using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSystem : MonoBehaviour
{
    [SerializeField]
    private List<Transform> path;

    [SerializeField]
    [Tooltip("nums of speed element = nums of point in path above")]
    private List<float> speeds;

    [SerializeField]
    [Tooltip("This stand for player touch and activate saw system")]
    private SawActivator activator;

    [SerializeField]
    private bool IsLoop;

    private int nextDestinatePointIndex;
    private float radiusGizmosSphere = 0.2f;

    private void Start()
    {
        // Insert saw 
        nextDestinatePointIndex = 0;
        // Event Handler
        if(activator!= null)
        {
            activator.onActivated += Activate;
        }
        // is Loop mode
        if(IsLoop)
        {
            Activate();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SpeedBoyController playerController = collision.gameObject.GetComponent<SpeedBoyController>();
            playerController.Die();
        }
    }

    private void OnDrawGizmos()
    {
        // Draw Path
        if (path.Count > 0)
        {
            iTween.DrawLineGizmos(new Transform[] { this.transform, path[0] }, Color.yellow);
            iTween.DrawLineGizmos(path.ToArray(), Color.yellow);

            for (int i = 0; i < path.Count; i++)
            {
                Gizmos.DrawSphere(path[i].position, radiusGizmosSphere);
            }
        }
       
    }
    
    public void Activate()
    {
        if (IsLoop && nextDestinatePointIndex == path.Count)
        {
            nextDestinatePointIndex = 0;
        }
        MoveToNextPosition();
    }

    private void MoveToNextPosition()
    {
        if (nextDestinatePointIndex >= path.Count) return;

        var hash = new Hashtable()
        {
            {"position", path[nextDestinatePointIndex]},
            {"speed", speeds[nextDestinatePointIndex] },
            {"easetype", iTween.EaseType.linear },
            {"oncomplete", "Activate"},
            {"ignoretimescale", true },
        };
        nextDestinatePointIndex++;

        iTween.MoveTo(gameObject, hash);
    }

}
