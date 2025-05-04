using System.Collections;
using UnityEngine;

public class BridgeController : MonoBehaviour
{
    public Transform movingBlock;       // The block that moves (1x3 width group)
    public Transform centerBlock;       // The block player steps on to trigger
    public float radius = 3f;           // Arc radius for rotation (adjust to fit spacing)
    public float rotationDuration = 1f;

    private Transform player;
    private bool isRotating = false;
    private bool playerOnCenter = false;

    private Coroutine moveCoroutine;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        // Check if player is on the center block
        if (!isRotating && IsPlayerOnBlock(centerBlock))
        {
            if (!playerOnCenter)
            {
                playerOnCenter = true;
                moveCoroutine = StartCoroutine(RotateMovingBlock(true));
            }
        }
        else if (playerOnCenter && !IsPlayerOnBlock(centerBlock))
        {
            playerOnCenter = false;
            if (moveCoroutine != null)
                StopCoroutine(moveCoroutine);

            moveCoroutine = StartCoroutine(RotateMovingBlock(false));
        }
    }

    bool IsPlayerOnBlock(Transform block)
    {
        Bounds blockBounds = new Bounds(block.position, new Vector3(3f, 1f, 1f)); // 3x1x1 block
        return blockBounds.Contains(new Vector3(player.position.x, block.position.y, player.position.z));
    }

    IEnumerator RotateMovingBlock(bool forward)
    {
        isRotating = true;

        float startAngle = forward ? 0f : -180f;
        float endAngle = forward ? -180f : 0f;

        float elapsed = 0f;
        Vector3 pivot = centerBlock.position;
        Vector3 offsetDirection = new Vector3(0f, 0f, -radius); // Start behind the pivot

        while (elapsed < rotationDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / rotationDuration);
            float angle = Mathf.Lerp(startAngle, endAngle, t);

            Vector3 newOffset = Quaternion.Euler(angle, 0f, 0f) * offsetDirection;
            movingBlock.position = pivot + newOffset;
            movingBlock.rotation = Quaternion.identity; // Keep the block level

            yield return null;
        }

        float finalAngle = forward ? -180f : 0f;
        Vector3 finalOffset = Quaternion.Euler(finalAngle, 0f, 0f) * offsetDirection;
        movingBlock.position = pivot + finalOffset;
        movingBlock.rotation = Quaternion.identity;

        isRotating = false;
    }
}