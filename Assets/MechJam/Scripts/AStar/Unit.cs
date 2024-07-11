using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class Unit : MonoBehaviour
{
    private Transform target;
    Vector3[] path;
    int targetIndex;
    Coroutine updatePathRoutine;
    public Vector2 lookDir;
    public bool followPath;

    [Header("Unit Parameters")]
    public float searchRadius;
    public float pathUpdateMoveThreshold;
    public float minPathUpdateTime;

    [Header("Logging")]
    [SerializeField] public Logger Logger;

    protected virtual void Awake()
    {
        followPath = false;
    }

    protected virtual void Start()
    {
        //StartCoroutine(UpdatePath());
        //PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
    }

    protected virtual void Update()
    {
        if (followPath && target != null)
        {
            if (updatePathRoutine == null)
            {
                updatePathRoutine = StartCoroutine(UpdatePath());
            }
            //PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
        }
        else
        {
            StopAllCoroutines();
        }
        //PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
    }

    public void SetTarget(Transform _target)
    {
        target = _target;
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            path = newPath;
            targetIndex = 0;
            StopCoroutine("FollowPath");
            if (followPath)
            {
                StartCoroutine("FollowPath");
            }
        }
    }

    IEnumerator UpdatePath()
    {
        if (Time.timeSinceLevelLoad < 0.3f)
        {
            yield return new WaitForSeconds(0.3f);
        }
        PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);

        float sqrMoveThreshold = pathUpdateMoveThreshold * pathUpdateMoveThreshold;
        Vector3 targetPosOld = target.position;

        while (true)
        {
            yield return new WaitForSeconds(minPathUpdateTime);
            if ((target.position - targetPosOld).sqrMagnitude > sqrMoveThreshold)
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