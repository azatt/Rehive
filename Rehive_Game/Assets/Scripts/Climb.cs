using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climb : MonoBehaviour
{
    public bool isClimbing;

    bool inPosition;
    bool isLerping;
    float posT;
    Vector3 startPos;
    Vector3 targetPos;
    Quaternion startRot;
    Quaternion targetRot;
    public float positionOffset;
    public float OffsetFromWall = 0.3f;
    public float speed_multiplier = 0.2f;
    public float climbSpeed = 3;
    public float rotateSpeed = 5;

    public float delta;

    Transform helper;

    public void Start()
    {
        helper = new GameObject().transform;
        helper.name = "climb helper";

        CheckForClimb();
    }
    public void CheckForClimb()
    {
        Vector3 origin = transform.position;
        Vector3 dir = transform.forward;
        RaycastHit hit;
        if (Physics.Raycast(origin, dir, out hit, 10))
        {
            helper.position = PosWithOffset(origin, hit.point);
            InitForClimb(hit);
        }
    }
    void InitForClimb(RaycastHit hit)
    {
        isClimbing = true;
        helper.transform.rotation = Quaternion.LookRotation(-hit.normal);
        startPos = transform.position;
        targetPos = hit.point + (hit.normal * OffsetFromWall);
        Debug.Log(startPos + ", " + targetPos);
        posT = 0;
        inPosition = false;
        // animation things
    }
    private void Update()
    {
        delta = Time.deltaTime;

        Tick();
    }

    public void Tick()
    {
        if (!inPosition)
        {
            GetInPosition();
            return;
        }

        if (!isLerping)
        {
            float hor = Input.GetAxis("Horizontal");
            float vert = Input.GetAxis("Vertical");
            float m = Mathf.Abs(hor) + Mathf.Abs(vert);

            Vector3 h = helper.right * hor;
            Vector3 v = helper.up * vert;
            Vector3 moveDir = (h + v).normalized;

            bool canMove = CanMove(moveDir);
            if (!canMove || moveDir == Vector3.zero)
            {
                return;
            }

            posT = 0;
            isLerping = true;
            startPos = transform.position;
            Vector3 tp = helper.position - transform.position;
            targetPos = helper.position;
        }
        else
        {
            posT += delta * climbSpeed;
            if (posT > 1)
            {
                posT = 1;
                isLerping = false;
            }

            Vector3 cp = Vector3.Lerp(startPos, targetPos, posT);
            transform.position = cp;
            transform.rotation = Quaternion.Slerp(transform.rotation, helper.rotation, delta * rotateSpeed);
        }
    }

    bool CanMove(Vector3 moveDir)
    {
        Vector3 origin = transform.position;
        float dis = positionOffset;
        Vector3 dir = moveDir;
        Debug.DrawRay(origin, dir * dis, Color.red);
        RaycastHit hit;

        if (Physics.Raycast(origin, dir, out hit, dis))
        {
            return false;
        }

        origin += moveDir * dis;
        dir = helper.forward;
        float dis2 = 0.5f;

        Debug.DrawRay(origin, dir * dis2, Color.blue);
        if (Physics.Raycast(origin, dir, out hit, dis))
        {
            helper.position = PosWithOffset(origin, hit.point);
            helper.rotation = Quaternion.LookRotation(-hit.normal);
            return true;
        }

        origin += dir * dis2;
        dir = -Vector3.up;

        Debug.DrawRay(origin, dir, Color.yellow);
        if (Physics.Raycast(origin, dir, out hit, dis2))
        {
            float angle = Vector3.Angle(helper.up, hit.normal);
            if (angle < 40)
            {
                helper.position = PosWithOffset(origin, hit.point);
                helper.rotation = Quaternion.LookRotation(-hit.normal);
                return true;
            }
        }

        return false;
    }

    void GetInPosition()
    {
        posT += delta;
        if (posT > 1)
        {
            posT = 1;
            inPosition = true;

            // enable the ik
        }

        Vector3 tp = Vector3.Lerp(startPos, targetPos, posT);
        transform.position = tp;
        transform.rotation = Quaternion.Slerp(transform.rotation, helper.rotation, delta * rotateSpeed);
    }

    Vector3 PosWithOffset(Vector3 origin, Vector3 target)
    {
        Vector3 direction = origin - target;
        direction.Normalize();
        Vector3 offset = direction * OffsetFromWall;
        return target + offset;
    }
}
