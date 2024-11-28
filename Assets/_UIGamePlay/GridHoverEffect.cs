using UnityEngine;

public class GridHoverEffect : MonoBehaviour
{
    //Apply Change Map
    public int rows = 3; // Số hàng
    public int cols = 5; // Số cột


    public Color hoverColor = Color.red; // Màu khi drag
    public Color defaultColor = Color.white; // Màu mặc định
    public Color activeColor = Color.green;


    public float cellSize = 1.1f; // Kích thước mỗi ô Cua 3 Row
    public float columnSize = 1.0f;
    public float rowSize = 0.5f;

    public float defaultSize = 6.6f;

    private SpriteRenderer[,] grid; // Lưu các ô trong ma trận


    private bool isDragging = false; // Trạng thái kéo chuột
    private int lastHoveredRow = -1; // Hàng cuối cùng được hover
    private int lastHoveredCol = -1; // Cột cuối cùng được hover

    [SerializeField] Transform ObjTilePrefab;

    void Start()
    {
        // Khởi tạo ma trận
        grid = new SpriteRenderer[rows, cols];


        this.NewTileTower();
    }
    void Update()
    {
        if (!GameManager.Instance.IsClickTile) return;
        // Kiểm tra trạng thái chuột kéo
        if (Input.GetMouseButtonDown(0)) // Bắt đầu drag
        {
            isDragging = true;
        }

        if (Input.GetMouseButtonUp(0)) // Kết thúc drag
        {
            if (isDragging && lastHoveredRow >= 0 && lastHoveredCol >= 0)
            {
                if (GameManager.Instance.ClickBtn != null && isHit)
                {
                    GameObject droppedCell = grid[lastHoveredRow, lastHoveredCol].gameObject;

                    droppedCell.GetComponent<TileTower>().IsActive();

                    Debug.Log("droppedCell");
                }
            }

            isDragging = false;
            ResetGridColors(); // Reset màu khi thả chuột
        }

        if (isDragging)
        {
            DetectHover(); // Đổi màu khi kéo

            Debug.Log("DetectHover");
        }
        Hover.Instance.FollowMouse(isDragging);

    }
    protected void NewTileTower()
    {
        Debug.Log("New Tile");
        for (int row = 0; row < rows; row++)
        {
            // Tạo một GameObject mới
            GameObject column = new GameObject();

            column.transform.SetParent(transform);

            column.name = "Row_" + row.ToString();

            for (int col = 0; col < cols; col++)
            {
                //New OBj Instance
                Transform newTile = Instantiate(ObjTilePrefab, column.transform);

                newTile.SetParent(column.transform); // Đặt GameObject là con của Grid cha

                newTile.GetComponent<TileTower>().SetLandIndex(row);


                // Đặt vị trí cho từng ô trong lưới

                newTile.transform.position = new Vector3(-defaultSize + cellSize * col+1 - rowSize*row, columnSize - row , 0);


                SpriteRenderer sprite = newTile.GetComponent<SpriteRenderer>();

                if (sprite == null) // Kiểm tra nếu không có SpriteRenderer
                {
                    Debug.LogError($"Missing SpriteRenderer on tile at row {row}, col {col}");
                    continue; // Bỏ qua ô này
                }

                // Lưu vào lưới
                grid[row, col] = sprite;
            }
        }
    }
    [SerializeField]
    bool isHit = false; // Cờ để kiểm tra nếu có va chạm hợp lệ

    // Phát hiện ô được hover khi drag
    void DetectHover()
    {
        bool validHover = false; // Cờ để kiểm tra nếu có va chạm hợp lệ

        RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        if (hits.Length > 0)
        {
            isHit = true;
            foreach (RaycastHit2D hit in hits)
            {
                GameObject hoveredCell = hit.collider.gameObject;

                for (int row = 0; row < rows; row++)
                {
                    for (int col = 0; col < cols; col++)
                    {
                        if (grid[row, col].gameObject == hoveredCell)
                        {
                            if (row != lastHoveredRow || col != lastHoveredCol) // Nếu ô hiện tại khác ô trước đó
                            {
                                ResetGridColors(); // Reset màu tất cả các ô
                                HighlightRowAndColumn(row, col); // Highlight ô hiện tại
                                lastHoveredRow = row; // Cập nhật hàng cuối cùng
                                lastHoveredCol = col; // Cập nhật cột cuối cùng
                                validHover = true; // Đánh dấu là ô hợp lệ

                                // Đặt màu activeColor cho ô được hover
                                grid[row, col].gameObject.GetComponent<SpriteRenderer>().color = activeColor;
                            }
                            return;
                        }
                    }
                }
            }

            if (!validHover)
            {
                ResetGridColors(); // Nếu không có ô hợp lệ nào, reset tất cả màu về mặc định
            }
        }
        else
        {
            isHit = false;
            ResetGridColors(); // Nếu không có va chạm nào, reset tất cả màu về mặc định
        }
    }


    // Highlight hàng và cột
    void HighlightRowAndColumn(int row, int col)
    {
        Debug.Log("Highlight");

        for (int r = 0; r < rows; r++)
        {
            grid[r, col].color = hoverColor; // Highlight cột
        }
        for (int c = 0; c < cols; c++)
        {
            grid[row, c].color = hoverColor; // Highlight hàng
        }
    }

    // Reset lại màu của toàn bộ grid
    void ResetGridColors()
    {
        foreach (var cell in grid)
        {
            cell.color = defaultColor;
        }
    }
}
