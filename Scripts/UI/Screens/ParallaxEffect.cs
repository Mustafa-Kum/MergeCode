using System.Collections.Generic;
using UnityEngine;

public class ParallaxController : MonoBehaviour
{
    public List<GameObject> sprites;
    public float moveSpeed = 5f;

    private Camera mainCamera;
    private Dictionary<GameObject, float> spriteWidths;
    private float rightMostXPosition;

    void Start()
    {
        mainCamera = Camera.main;
        InitializeSpriteWidths();
        PositionSpritesAtStart();
    }

    void Update()
    {
        MoveSprites();
        CheckAndRespawnSprites();
    }

    void InitializeSpriteWidths()
    {
        spriteWidths = new Dictionary<GameObject, float>();
        foreach (var sprite in sprites)
        {
            float width = sprite.GetComponent<SpriteRenderer>().bounds.size.x;
            spriteWidths[sprite] = width;
        }
    }

    void PositionSpritesAtStart()
    {
        float currentX = 0f;
        for (int i = 0; i < sprites.Count; i++)
        {
            if (i == 0)
            {
                Vector3 startPosition = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0));
                startPosition.x += spriteWidths[sprites[i]] / 2; // Center the sprite
                sprites[i].transform.position = new Vector3(startPosition.x, sprites[i].transform.position.y, sprites[i].transform.position.z);
                currentX = startPosition.x;
            }
            else
            {
                currentX += (spriteWidths[sprites[i - 1]] / 2) + (spriteWidths[sprites[i]] / 2);
                sprites[i].transform.position = new Vector3(currentX, sprites[i].transform.position.y, sprites[i].transform.position.z);
            }
            // Update rightMostXPosition after positioning each sprite
            rightMostXPosition = Mathf.Max(rightMostXPosition, sprites[i].transform.position.x + (spriteWidths[sprites[i]] / 2));
        }
    }

    void MoveSprites()
    {
        foreach (var sprite in sprites)
        {
            sprite.transform.Translate(Vector3.left * moveSpeed * Time.deltaTime, Space.World);
        }
    }

    void CheckAndRespawnSprites()
    {
        List<GameObject> toRespawn = new List<GameObject>();

        foreach (var sprite in sprites)
        {
            Vector3 viewportPosition = mainCamera.WorldToViewportPoint(sprite.transform.position);
            if (viewportPosition.x < 0)
            {
                toRespawn.Add(sprite);
            }
        }

        foreach (var sprite in toRespawn)
        {
            float spriteWidth = spriteWidths[sprite];
            float newPositionX = rightMostXPosition + spriteWidth / 2;
            sprite.transform.position = new Vector3(newPositionX, sprite.transform.position.y, sprite.transform.position.z);

            // Update rightMostXPosition with the new position of this sprite
            rightMostXPosition = newPositionX + spriteWidth / 2;
        }
    }
}