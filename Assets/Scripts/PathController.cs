using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathController : MonoBehaviour
{
    [SerializeField]
    public PathManager pathManager;

    List<Waypoint> path;
    Waypoint target;

    public float MoveSpeed;
    public float RotateSpeed;

    bool isWalking;

    Animator animator;



    // Start is called before the first frame update
    void Start()
    {
        path = pathManager.GetPath();
        if (path != null && path.Count > 0) 
        {
            target = path[0];
        }
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey && isWalking == false)
        {
            isWalking = true;
            animator.SetBool("isWalking", true);

        }

        if (isWalking) 
        {
            rotateTowardsTarget();
            moveForward();
        }

    }

    void rotateTowardsTarget() 
    {
        float stepSize = RotateSpeed * Time.deltaTime;

        Vector3 targetDir = target.pos - transform.position;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, stepSize, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDir);
    }

    void moveForward() 
    {
        float stepSize = Time.deltaTime * MoveSpeed;
        float distanceToTarget = Vector3.Distance(transform.position, target.pos);

        if (distanceToTarget < stepSize) 
        {
            return;
        }
        Vector3 moveDir = Vector3.forward;
        transform.Translate(moveDir * stepSize);
    
    }
    private void OnTriggerEnter(Collider other)
    {
        target = pathManager.GetNextTarget();

    }

}
