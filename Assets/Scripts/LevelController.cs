using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    public TextController textController;
    public FigureFabric figureFabric;
    Tile[,] field = new Tile[10, 10];
    Figure[] figures = new Figure[3];
    int figuresCount = 0;

    public Tile tile_prefab;
    public GameObject field_image;
    public GameObject tiles_holder;

    public Vector2 tile_size;
    public Vector2 field_size;
    public Vector2 start_pos;

    bool carried = false;
    int carried_index = 0;

    int score;

    void Init()
    {
        for (int i = 0; i < 10; i++) {
            for (int j = 0; j < 10; j++) {
                field[i, j].Disable();
            }
        }
        for(int i = 0; i < 3; i++)
        {
            if (figures[i] != null) Destroy(figures[i].gameObject);
        }

        SpawnFigures();
        textController.SetToZero();
        score = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        tile_size.x = tile_prefab.transform.localScale.x / 2;
        tile_size.y = tile_prefab.transform.localScale.y / 2;

        Vector3 v3_field_size = field_image.GetComponent<MeshFilter>().mesh.bounds.size;
        field_size.x = v3_field_size.x * field_image.transform.localScale.x;
        field_size.y = v3_field_size.z * field_image.transform.localScale.z;

        start_pos = (tile_size - field_size) / 2;
       
        for(int i = 0; i < 10; i++)  {
            for(int j = 0; j < 10; j++)  {
                field[i, j] = Instantiate(tile_prefab);
                field[i, j].transform.position = start_pos + tile_size * new Vector2(i, j);
                field[i, j].transform.parent = tiles_holder.transform;
            }
        }
        Init();
    }

    void Update()
    {
        ProcessInput();

        if (down && world_pos.y < -2.5f) 
        {
            int figure_index = (int)( (world_pos.x + 2.5f) / (5.0 / 3.0) );
            figure_index = Mathf.Clamp(figure_index, 0, 2);

            if(figures[figure_index] != null) {
                carried = true;
                pressed = true;
                carried_index = figure_index;

                figures[figure_index].transform.localScale = new Vector3(1, 1, 1);

                figures[carried_index].transform.position = world_pos;

                Vector3 positon = figures[carried_index].transform.position;
                positon.y += 1;
                positon.z = -5.5f;
                figures[carried_index].transform.position = positon;
            }
        }

        if(pressed && carried)
        {
            figures[carried_index].transform.position = world_pos;

            Vector3 positon = figures[carried_index].transform.position;
            positon.y += 1;
            positon.z = -5.5f;
            figures[carried_index].transform.position = positon;
        }

        if(up && carried)
        {
            carried = false;

            Vector2 relative_pos = (Vector2)(figures[carried_index].transform.position) - start_pos;

            if (relative_pos.x < -0.4f               || 
                relative_pos.y < - 0.4f              || 
                relative_pos.x > field_size.x + 0.4f || 
                relative_pos.y > field_size.y + 0.4f)
            {
                MoveCarriedBack();
            }
            else {
                FigureOffsets figure_offsets = getFigureOffsets(relative_pos, figures[carried_index].Shape);

                if ( IsPlaceFree(figure_offsets) ) {
                    PlaceFigure(figure_offsets, figures[carried_index].CurrentColor);

                    Destroy(figures[carried_index].gameObject);
                    figures[carried_index] = null;
                    figuresCount--;

                    ClearFullRows(figure_offsets.orign);

                    if (figuresCount == 0) SpawnFigures();
                    if (!IsStepAvailable())
                    {
                        Debug.Log("Game over");
                        Init();
                    }
                }
                else {
                    MoveCarriedBack();
                }
            }
        }
    }

    Vector2Int GetVector2Int(Vector2 vector)
    {
        return new Vector2Int()
        {
            x = Mathf.RoundToInt(vector.x),
            y = Mathf.RoundToInt(vector.y)
        };
    }

    FigureOffsets getFigureOffsets(Vector2 position, Vector2[] shape)
    {
        int leng = shape.Length;
        FigureOffsets result = new FigureOffsets();
        result.offsets = new Vector2Int[leng];

        result.orign = GetVector2Int(position * new Vector2(2.5f, 2.5f));

        Vector2 start_pos = position * new Vector2(2.5f, 2.5f) + shape[0];
        result.position = GetVector2Int(start_pos);

        for (int i = 0; i < leng; i++){
            result.offsets[i] = GetVector2Int(shape[i] - shape[0]);
        }

        return result;
    }

    void MoveCarriedBack()
    {
        figures[carried_index].transform.position = figures_pos[carried_index];
        figures[carried_index].transform.localScale = new Vector3(figure_scale, figure_scale, figure_scale);

        Vector3 positon = figures[carried_index].transform.position;
        positon.z = 0;
        figures[carried_index].transform.position = positon;
    }

    void ClearFullRows(Vector2Int point)
    {
        List<int> full_rows = new List<int>();
        List<int> full_columns = new List<int>();

        bool is_row_full = true;
        for(int y = 0; y < 10; y++)
        {
            for(int x = 0; x < 10; x++)
            {
                if(!field[x,y].Enabled)
                {
                    is_row_full = false;
                    break;
                }
            }

            if (is_row_full) full_rows.Add(y);
            is_row_full = true;
        }

        bool is_column_full = true;
        for (int x = 0; x < 10; x++)
        {
            for (int y = 0; y < 10; y++)
            {
                if (!field[x, y].Enabled)
                {
                    is_column_full = false;
                    break;
                }
            }

            if (is_column_full) full_columns.Add(x);
            is_column_full = true;
        }

        foreach(int row in full_rows)
        {
            for(int x = 0; x < 10; x++)
            {
                float delay = Mathf.Abs(x - point.x) * 0.03f;
                field[x, row].DisableWithAnim(delay);
            }
        }

        foreach (int column in full_columns)
        {
            for (int y = 0; y < 10; y++)
            {
                float delay = Mathf.Abs(y - point.y) * 0.03f;
                field[column, y].DisableWithAnim(delay);
            }
        }

        int multipier = Mathf.Max(full_rows.Count, full_columns.Count);
        if(full_rows.Count > 0 && full_columns.Count > 0) {
            multipier *= Mathf.Min(full_rows.Count, full_columns.Count) + 1;
        }

        score += (full_rows.Count + full_columns.Count) * 10 * multipier;

        if(multipier > 0)
        {
            textController.SetMultiplier(multipier);
            textController.setScore(score);
        }
    }

    void PlaceFigure(FigureOffsets figureOffsets, Color color)
    {
        foreach (Vector2Int offset in figureOffsets.offsets)
        {
            Vector2Int cpos = offset + figureOffsets.position;
            field[cpos.x, cpos.y].Enable(color);
        }
    }

    bool IsPlaceFree(FigureOffsets figureOffsets)
    {
        foreach (Vector2Int offset in figureOffsets.offsets)
        {
            Vector2Int cpos = offset + figureOffsets.position;

            if (cpos.x < 0 || cpos.y < 0 || cpos.x > 9 || cpos.y > 9) return false;
            if (field[cpos.x, cpos.y].Enabled) return false;
        }

        return true;
    }
    
    bool IsStepAvailable()
    {
        foreach(Figure figure in figures)
        {
            if (figure == null) continue;

            FigureOffsets figureOffsets = getFigureOffsets(new Vector2(), figure.Shape);

            for(int y = 0; y < 10; y++) {
                for(int x = 0; x < 10; x++) {
                    figureOffsets.position = new Vector2Int(x, y);
                    if (IsPlaceFree(figureOffsets)) return true;
                }
            }
        }

        return false;
    }

    bool pressed;
    bool up;
    bool down;
    Vector2 world_pos;

    public bool phone_input = false;
    void ProcessInput()
    {
        pressed = false;
        up = false;
        down = false;


        if (phone_input)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began) down = true;
            else if (touch.phase == TouchPhase.Moved) pressed = true;
            else if (touch.phase == TouchPhase.Ended) up = true;

            world_pos = Camera.main.ScreenToWorldPoint(touch.position);
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Mouse0)) down = true;
            else if (Input.GetKey(KeyCode.Mouse0)) pressed = true;
            else if (Input.GetKeyUp(KeyCode.Mouse0)) up = true;

            world_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    public float figure_scale = 0.6f;
    readonly Vector3[] figures_pos = new Vector3[3] { new Vector3(-1.5f, -3.5f, -5), new Vector3(0, -3.5f, -5), new Vector3(1.5f, -3.5f, -5) };
    void SpawnFigures()
    {
        for(int i = 0; i < 3; i++) {
            if (figures[i] != null) Destroy(figures[i]);
            figures[i] = figureFabric.CreateFigure(Random.Range(0, FigureFabric.ShapesCount));
            figures[i].transform.position = figures_pos[i];
            figures[i].transform.localScale = new Vector3(figure_scale, figure_scale, figure_scale);
        }
        figuresCount = 3;
    }
}
