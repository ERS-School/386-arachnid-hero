using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpringJoint))]
[RequireComponent(typeof(Rigidbody))]
public class GrappleHook : MonoBehaviour
{
    public float GrappleRange = 15;
    public float springMax = 70;
    public float dampeningMax = 2;
    public float detectionSphereRadius = 0.1f;
    [Range(-1f, 1f)]
    public float offsetGrapple = -0.2f;
    public LayerMask layermask = ~0;
    public Material material;
    public List<Vector3> GrappleHitPositions = new List<Vector3>();
    public List<float> segmentDistances = new List<float>();
    bool mouseDown;
    float GrappleDist;
    SpringJoint spring;
    Vector3 GrappleHitPos;
    Rigidbody GrabbedRB;

    void Start()
    {
        //assign variables
        spring = GetComponent<SpringJoint>();
        spring.spring = 0;
        spring.damper = 0;
        if (material)
        {
            GetComponent<LineRenderer>().material = material;
        }
    }

    public void UnGrapple()
    {
        // Lets go of grapple and resets all information
        mouseDown = false;
        spring.spring = 0;
        spring.damper = 0;
        GrappleHitPositions.Clear();
        segmentDistances.Clear();
        GrabbedRB = null;
    }

    public void GrappleTo(RaycastHit hit)
    {
        // Sets all spring values to 0 in case was not 0
        spring.spring = 0;
        spring.damper = 0;
        if (hit.rigidbody && !hit.rigidbody.isKinematic) // Checks if the object that was hit will be able to move
        {
            // Makes the grapple connect to the object so it can be moved
            GrabbedRB = hit.rigidbody;
            GrappleHitPositions.Add(GrabbedRB.transform.position);
            segmentDistances.Add(0);
            spring.autoConfigureConnectedAnchor = true;
            spring.connectedBody = hit.rigidbody;
        }
        else
        {
            // if not a movable object then just add the hit point to the grapple positions
            spring.connectedBody = null;
            spring.autoConfigureConnectedAnchor = false;
            GrappleHitPos = hit.point;
            GrappleHitPositions.Add(hit.point);
            segmentDistances.Add(0);
            spring.connectedAnchor = GrappleHitPos;
        }

        spring.maxDistance = GrappleDist = hit.distance * 0.98f; // The max distance is the actual distance multiplied by a smaller number to give some speed boost
        if (material)
        {
            material.mainTextureOffset = new Vector2(-GrappleDist, material.mainTextureScale.y); // Adjusts texture to not look like it is flying away if there is a material
        }
        mouseDown = true;
    }

    void UpdateGrapple()
    {
        if (material)
        {
            material.mainTextureOffset = new Vector2(-Vector3.Distance(transform.position, GrappleHitPositions[0]), material.mainTextureScale.y);
        }
        if (GrabbedRB == null) // If the grabbed object can't be moved
        {
            RaycastHit hit;

            int hitAmount = segmentDistances.Count;
            Vector3 grappleShootPos = transform.position + transform.right * offsetGrapple;
            // Checks if there is something between the player and the current grapple position
            if (Physics.Raycast(grappleShootPos, (GrappleHitPos - grappleShootPos), out hit, GrappleRange, layermask))
            {
                if (Vector3.Distance(GrappleHitPos, hit.point) > detectionSphereRadius) // If there is and it isn't the current hit position
                {
                    // Then shorten the grapple accordingly and set the new point to the grapple position
                    float shortenedBy = Vector3.Distance(GrappleHitPos, hit.point);
                    spring.maxDistance -= shortenedBy;
                    segmentDistances.Add(shortenedBy);
                    GrappleDist = spring.maxDistance;
                    GrappleHitPos = spring.connectedAnchor = hit.collider.ClosestPointOnBounds(hit.point);
                    GrappleHitPositions.Add(GrappleHitPos);
                }
                else if (hitAmount > 1)
                {
                    float rayRange = Vector3.Distance(GrappleHitPos, grappleShootPos)*2; // make sure the ray doesn't go further than the length of the grapple
                    Vector3 directionMul = GrappleHitPositions[hitAmount - 2] - grappleShootPos;
                    bool rayHit = Physics.SphereCast(grappleShootPos, 0.01f, directionMul, out hit, rayRange, layermask);
                    Debug.DrawLine(grappleShootPos, hit.point, Color.green);
                    if (!rayHit || Vector3.Distance(GrappleHitPositions[hitAmount - 2], hit.point) < detectionSphereRadius) // Checks if there is anything between the player and the second last grapple hit
                    {
                        // If there isn't anything between then can remove the second last grapple and lengthen the grapple
                        spring.maxDistance += segmentDistances[segmentDistances.Count - 1];
                        segmentDistances.RemoveAt(hitAmount - 1);
                        GrappleDist = spring.maxDistance;
                        GrappleHitPos = spring.connectedAnchor = GrappleHitPositions[hitAmount - 2];
                        GrappleHitPositions.RemoveAt(hitAmount - 1);

                    }
                }
            }
        }
        else
        {
            // If the object can be moved then just update the grapple position with the objects transform
            GrappleHitPositions.RemoveAt(0);
            GrappleHitPositions.Insert(0, GrabbedRB.transform.position);
        }
        spring.spring = springMax;
        spring.damper = dampeningMax;
    }

    void FixedUpdate()
    {
        // If mouse down then update grapple
        if (mouseDown)
        {
            UpdateGrapple();
        }
        else // If not then set everything to 0
        {
            spring.spring = 0;
            spring.damper = 0;
        }
        // Draw the grapple positions after finished checking them all
        DrawGrapples(GrappleHitPositions);
    }

    public void DrawGrapples(List<Vector3> GrapplePositions)
    {
        GetComponent<LineRenderer>().positionCount = GrapplePositions.Count + 1; // Sets the amount of positions to draw the grapple
        List<Vector3> pos_temp = new List<Vector3>(); // Temporary list to store the positions
        pos_temp.AddRange(GrapplePositions); // Add the grapple positions
        pos_temp.Add(transform.position + transform.right * offsetGrapple); // Add the player position and offset a bit
        GetComponent<LineRenderer>().SetPositions(pos_temp.ToArray()); // Adds the positions to line renderer
    }
}
