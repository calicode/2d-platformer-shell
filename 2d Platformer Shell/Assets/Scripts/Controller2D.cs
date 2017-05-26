using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(BoxCollider2D))]
public class Controller2D : MonoBehaviour
{

    const float SKINWIDTH = .015f;
    public int horizontalRayCount = 4;
    public int verticalRayCount = 4;
    public CollisionInfo collisions;
    float horizontalRaySpacing;
    float verticalRaySpacing;

    RaycastOrigins raycastOrigins;
    BoxCollider2D collider;
    public LayerMask collisionMask;


    // Use this for initialization
    void Start()
    {
        collider = GetComponent<BoxCollider2D>();

        CalculateRaySpacing();
    }

    void Update()
    {

    }


    void UpdateRayCastOrigins()
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(SKINWIDTH * -2);
        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    void HorizontalCollisions(ref Vector3 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(velocity.x) + SKINWIDTH;

        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);
            if (hit)
            {


                velocity.x = (hit.distance - SKINWIDTH) * directionX;
                rayLength = hit.distance;

                collisions.left = directionX == -1;
                collisions.right = directionX == 1;

            }
            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);

        }
    }

    void VerticalCollisions(ref Vector3 velocity)
    {
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + SKINWIDTH;

        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);
            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);
            if (hit)
            {
                velocity.y = (hit.distance - SKINWIDTH) * directionY;
                rayLength = hit.distance;
                collisions.below = directionY == -1;
                collisions.above = directionY == 1;



            }


        }


    }

    void CalculateRaySpacing()
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(SKINWIDTH * -2);
        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
    }
    struct RaycastOrigins
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }
    public struct CollisionInfo
    {
        public bool above, below;
        public bool left, right;
        public void Reset()
        {
            above = below = false;
            left = right = false;

        }

    }
    public void Move(Vector3 velocity)
    {
        collisions.Reset();

        //collision handling
        UpdateRayCastOrigins();
        if (velocity.x != 0) { HorizontalCollisions(ref velocity); }
        if (velocity.y != 0) { VerticalCollisions(ref velocity); }
        transform.Translate(velocity);

    }


}
