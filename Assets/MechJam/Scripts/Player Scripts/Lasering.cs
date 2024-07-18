using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class Lasering : MonoBehaviour
{
    [SerializeField] private float laserDamage;
    [SerializeField] private float laserRange;
    [SerializeField] private float laserICD;

    [SerializeField] private LayerMask hitSomething;

    [SerializeField] private NanoBotInputs nanoBotInput;

    [SerializeField] private LineRenderer lineRenderer;

    [SerializeField] private Transform laserFirePoint;

    [SerializeField] private Transform armRotation;

    [SerializeField] private GameObject laserEffect;

    private Vector3 mousePos;

    Rigidbody2D rb;

    private InputAction fire;
    private float lastLaserTickTime;

    //private void OnEnable()
    //{
    //    fire = nanoBotInput.Player.Fire;
    //    fire.Enable();
    //    fire.performed += Fire;
    //}

    //private void OnDisable()
    //{
    //    fire.Disable();
    //}

    //private void Awake()
    //{
    //    nanoBotInput = new NanoBotInputs();
    //}
    void Start()
    {
        lastLaserTickTime = Time.time;
        lineRenderer = GetComponent<LineRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale != 0)
        {
            Vector2 laserLookDir = (mousePos - laserFirePoint.position);
            Debug.DrawRay(laserFirePoint.position, laserLookDir, Color.red);
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);



            Vector2 dirVec = (mousePos - transform.position).normalized;
            transform.localScale = new Vector3(
                Mathf.Sign(dirVec.x) * Mathf.Abs(transform.localScale.x),
                transform.localScale.y,
                transform.localScale.z);

            Vector2 rotation = (mousePos - armRotation.position).normalized;

            float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;

            if (Input.GetMouseButton(0))
            {

                //armRotation.rotation = Quaternion.Euler(0f, 0f, rotZ);

                lineRenderer.enabled = true;

                RaycastHit2D hit = Physics2D.Raycast(laserFirePoint.position, laserLookDir, hitSomething);

                //LaserUpdate();
                lineRenderer.SetPosition(0, laserFirePoint.position);

                if (hit.collider != null)
                {
                    if (Time.time - lastLaserTickTime > laserICD)
                    {
                        if (hit.collider.gameObject.CompareTag("CellBlock") || hit.collider.gameObject.CompareTag("CholesterolBlock"))
                        {
                            DestructableTiles tiles = hit.collider.GetComponent<DestructableTiles>();
                            tiles.DamageTileFromRaycast(hit.point, hit.normal, laserDamage);
                        }
                        else
                        {
                            Health healthComponent;

                            if (hit.collider.gameObject.TryGetComponent<Health>(out healthComponent))
                            {
                                healthComponent.TakeDamage(laserDamage);
                            }
                        }
                        lastLaserTickTime = Time.time;
                    }

                    //do damage to blocks
                    lineRenderer.SetPosition(1, hit.point);
                    GameObject effectImpact = Instantiate(laserEffect, hit.point, Quaternion.Euler(0f, 0f, rotZ));
                    Destroy(effectImpact, 0.3f);
                }
                else
                {
                    lineRenderer.SetPosition(1, mousePos);
                    GameObject effectImpact = Instantiate(laserEffect, mousePos, Quaternion.Euler(0f, 0f, rotZ));
                    Destroy(effectImpact, 0.3f);
                }

            }
            else if (Input.GetMouseButtonUp(0))
            {
                lineRenderer.enabled = false;
                armRotation.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
        }
    }

    //private void Fire(InputAction.CallbackContext context)
    //{
    //    if (context.performed)
    //    {
    //        var mousPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //        RaycastHit2D hit = Physics2D.Raycast(transform.position, mousPos, hitSomething);

    //        lineRenderer.SetPosition(0, firePoint.position);
    //        lineRenderer.SetPosition(1, mousPos);


    //        if (hit.collider != null)
    //        {
    //            //do damage to blocks
    //        }
    //    }
    //}

    void LaserUpdate()
    {
        var mousPos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);

        lineRenderer.SetPosition(0, laserFirePoint.position);
        
        Vector2 direction = mousePos - laserFirePoint.position;

        RaycastHit2D laserHit = Physics2D.Raycast(laserFirePoint.position, direction.normalized, laserRange);

        if (laserHit)
        {
            lineRenderer.SetPosition(1, laserHit.point);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Vector2 direction = mousePos - laserFirePoint.position;
        Gizmos.DrawLine(laserFirePoint.position, direction.normalized * laserRange);
    }
}
