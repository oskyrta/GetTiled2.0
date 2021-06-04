using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Figure : MonoBehaviour
{
    List<Tile> tiles;
    public Vector2[] Shape { get; set; }
    public Color CurrentColor { get; set; }

    public void Init(Vector2[] figure_shape, Color color)
    {
        LevelController levelController = GameObject.Find("LevelController").GetComponent<LevelController>();
        tiles = new List<Tile>();
        Shape = figure_shape;
        CurrentColor = color;
        
        foreach (Vector2 pos in figure_shape)
        {
            Tile tile = Instantiate(levelController.tile_prefab);
            Vector3 position = pos * levelController.tile_size;
            position.z = 0;
            
            tile.transform.parent = gameObject.transform;
            tile.transform.position = position;
            tile.Enable(color);

            tiles.Add(tile);
        }
    }
}
