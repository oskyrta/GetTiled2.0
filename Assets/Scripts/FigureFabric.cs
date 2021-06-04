using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FigureFabric : MonoBehaviour
{
    readonly static List<Vector2[]> figureShapes = new List<Vector2[]>() {
        new Vector2[] { new Vector2(-2, 0), new Vector2(-1, 0), new Vector2(0, 0), new Vector2(1, 0), new Vector2(2, 0) }, // Horizontal 5 stick
        new Vector2[] { new Vector2(0, -2), new Vector2(0, -1), new Vector2(0, 0), new Vector2(0, 1), new Vector2(0, 2) }, // Vertical 5 stick
                   
        new Vector2[] { new Vector2(-1.5f, 0), new Vector2(-0.5f, 0), new Vector2(0.5f, 0), new Vector2(1.5f, 0) }, // Horizontal 4 stick
        new Vector2[] { new Vector2(0, -1.5f), new Vector2(0, -0.5f), new Vector2(0, 0.5f), new Vector2(0, 1.5f) }, // Vertical 4 stick
                   
        new Vector2[] { new Vector2(-1, 0), new Vector2(0, 0), new Vector2(1, 0) }, // Horizontal 3 stick
        new Vector2[] { new Vector2(0, -1), new Vector2(0, 0), new Vector2(0, 1) }, // Vertical 3 stick
                  
        new Vector2[] { new Vector2(-0.5f, 0), new Vector2(0.5f, 0) }, // Horizontal 2 stick
        new Vector2[] { new Vector2(0, -0.5f), new Vector2(0, 0.5f) }, // Vertical 2 stick
                   
        new Vector2[] { new Vector2(0, 0) }, // Point

        new Vector2[] {
            new Vector2(-1, 1), new Vector2(0, 1), new Vector2(1, 1),
            new Vector2(-1, 0), new Vector2(0, 0), new Vector2(1, 0),       // Cube
            new Vector2(-1, -1), new Vector2(0, -1), new Vector2(1, -1),
        },

        new Vector2[] {
            new Vector2(-1, 1),
            new Vector2(-1, 0),                                            // bottom left big corner
            new Vector2(-1, -1), new Vector2(0, -1), new Vector2(1, -1)
        },

        new Vector2[] {
                                                     new Vector2(1, 1),
                                                     new Vector2(1, 0),    // bottom right big corner
            new Vector2(-1, -1), new Vector2(0, -1), new Vector2(1, -1)
        },

        new Vector2[] {
            new Vector2(-1, 1), new Vector2(0, 1), new Vector2(1, 1),
            new Vector2(-1, 0),                                            // top left big corner
            new Vector2(-1, -1),
        },

        new Vector2[] {
            new Vector2(-1, 1), new Vector2(0, 1), new Vector2(1, 1),
                                                   new Vector2(1, 0),
                                                   new Vector2(1, -1)      // top right big corner
        },

        new Vector2[] {
            new Vector2(-0.5f, 0.5f),
            new Vector2(-0.5f, -0.5f), new Vector2(0.5f, -0.5f)    // bottom left small corner
        },

        new Vector2[] {
                                       new Vector2(0.5f, 0.5f),
            new Vector2(-0.5f, -0.5f), new Vector2(0.5f, -0.5f)    // bottom right small corner
        },

        new Vector2[] {
           new Vector2(-0.5f, 0.5f), new Vector2(0.5f, 0.5f),
           new Vector2(-0.5f, -0.5f)                               // top left small corner
        },

        new Vector2[] {
            new Vector2(-0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                                      new Vector2(0.5f, -0.5f)     // top right small corner
        },
    };

    public static int ShapesCount = figureShapes.Count;

    public GameObject figure_prefab;
    public List<Color> colors;
    public int[] colors_indices = new int[] { 0, 0, 1, 1, 2, 2, 3, 3, 4, 5, 6, 6, 6, 6, 7, 7, 7, 7 };

    Color GetColor(int idx) { return colors[colors_indices[idx]]; }

    public Figure CreateFigure(int index)
    {
        Figure figure = Instantiate(figure_prefab).GetComponent<Figure>();
        figure.Init(figureShapes[index], colors[ colors_indices[index] ]);

        return figure;
    }
}
