using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class Unit : MonoBehaviour
{
    
    Vector3[] path;
    int targetIndex;
    Coroutine updatePathRoutine;

    [Header("Path Update Settings")]
    public float pathUpdateMoveThreshold;
    public float minPathUpdateTime;

    [Header("Logging")]
    [SerializeField] public Logger Logger;

    [Header("[READONLY] Pathfinding Settings")]
    private Transform target;
    public Vector2 lookDir;
    private bool followPath;

    protected virtual void Awake()
    {
        followPath = false;
    }

    protected virtual void Start()
    {
        updatePathRoutine =  StartCoroutine(UpdatePath());
        //PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
    }

    protected virtual void Update()
    {
        //if (followPath && target != null)
        //{
        //    if (updatePathRoutine == null)
        //    {
        //        updatePathRoutine = StartCoroutine(UpdatePath());
        //    }
        //    //PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
        //}
        //else
        //{
        //    StopAllCoroutines();
        //}
        //PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
    }

    public void SetConditions(Transform _target, bool _followPath)
    {
        target = _target;
        followPath = _followPath;
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful && followPath)
        {
            path = newPath;
            targetIndex = 0;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator UpdatePath()
    {
        if (Time.timeSinceLevelLoad < 0.3f)
        {
            yield return new WaitForSeconds(0.3f);
        }

        while (target == null)
        {
            yield return null;
        }

        PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);

        float sqrMoveThreshold = pathUpdateMoveThreshold * pathUpdateMoveThreshold;
        Vector3 targetPosOld = target.position;

        while (true)
        {
            yield return new WaitForSeconds(minPathUpdateTime);
            if (target != null && (target.position - targetPosOld).sqrMagnitude > sqrMoveThreshold)
            {
                PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
                targetPosOld = target.position;
            }
        }
    }

    IEnumerator FollowPath()
    {
        if (path.Length > 0)
        {
            Vector3 currentWaypoint = path[0];

            while (true)
            {
                if (Vector3.Distance(transform.position, currentWaypoint) < 0.1f)
                {
                    targetIndex++;
                    if (targetIndex >= path.Length)
                    {
                        lookDir = Vector3.zero;
                        yield break;
                    }
                    currentWaypoint = path[targetIndex];
                }

                lookDir = (currentWaypoint - transform.position).normalized;

                yield return null;
            }
        }
    }

    protected virtual void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i], Vector3.one*0.2f);

                if (i == targetIndex)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }
}