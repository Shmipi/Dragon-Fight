using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeckCollider : MonoBehaviour
{
    public Player player;
    private PlayerBehaviour playerBehaviour;

    private EdgeCollider2D edgeCollider;
    private Vector2 startFirstPosition;
    private Vector2 worldSecondPoint;
    private AttackCollider attackCollider;

    public PlayerBehaviour PlayerBehaviour { get => playerBehaviour; set => playerBehaviour = value; }
    public AttackCollider AttackCollider { get => attackCollider; set => attackCollider = value; }

    private void Start()
    {
        player = playerBehaviour.player;
        edgeCollider = GetComponent<EdgeCollider2D>();

        if(edgeCollider != null)
        {
            startFirstPosition = edgeCollider.points[0];
            worldSecondPoint = transform.TransformPoint(edgeCollider.points[1]);
        }
    }

    private void Update()
    { 
        if (edgeCollider != null)
        {
            // Convert it back to local space to maintain world position
            Vector2 localSecondPoint = transform.InverseTransformPoint(worldSecondPoint);

            // Set the updated points
            Vector2[] newPoints = new Vector2[2];
            newPoints[0] = startFirstPosition; 
            newPoints[1] = localSecondPoint;  

            edgeCollider.points = newPoints;
        }
    }

    public void Hurt()
    {
        attackCollider.SetHasBeenHurt();
        playerBehaviour.Hurt();
    }

}
