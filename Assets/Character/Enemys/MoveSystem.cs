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
    [SerializeField]
    private int loopTimes = 999;

    [SerializeField]
    private bool IsInitialyMove;

    private int nextDestinatePointIndex;
    private float radiusGizmosSphere = 0.2f;

    // isGoBack = 0 when not. and 1 when go back
    private int isGoBack = 0;

    private void Start()
    {
        // Insert saw 
        nextDestinatePointIndex = 1;
        //path.Insert(0, this.transform);

        // AutoMove Mode
        if (IsInitialyMove)
        {
            Activate();
            return;
        }

        // Activate Mode
        if (activator!= null)
        {
            activator.onActivated += Activate;
        }
       
    }

    private void OnDrawGizmos()
    {
        // Draw Path
        if (path.Count > 1)
        {
            //iTween.DrawLineGizmos(new Transform[] { this.transform, path[0] }, Color.yellow);
            iTween.DrawLineGizmos(path.ToArray(), Color.yellow);

            for (int i = 0; i < path.Count; i++)
            {
                Gizmos.DrawSphere(path[i].position, radiusGizmosSphere);
            }
        }
       
    }
    
    public void Activate()
    {
        if(path.Count < 2)
        {
            return;
        }
        if (IsLoop && nextDestinatePointIndex == path.Count && loopTimes > 0)
        {
            nextDestinatePointIndex = path.Count - 2;
            isGoBack = 1;
            loopTimes--;
        }
        if(nextDestinatePointIndex == -1 && loopTimes > 0)
        {
            nextDestinatePointIndex = 1;
            isGoBack = 0;
            loopTimes--;
        }
        MoveToNextPosition();
    }

    private void MoveToNextPosition()
    {
        if (nextDestinatePointIndex >= path.Count || nextDestinatePointIndex < 0) return;

        var hash = new Hashtable()
        {
            {"position", path[nextDestinatePointIndex]},
            {"speed", speeds[nextDestinatePointIndex - (1 - isGoBack)] },
            {"easetype", iTween.EaseType.linear },
            {"oncomplete", "Activate"},
            {"ignoretimescale", true },
        };

        if (isGoBack == 1)
        {
            nextDestinatePointIndex--;
        }
        else
        {
            nextDestinatePointIndex++;
        }

        iTween.MoveTo(gameObject, hash);
    }

}
