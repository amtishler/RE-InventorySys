using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryGrid<GridObject>
{
    public event EventHandler<OnGridValueChangedEventArgs> OnGridValueChanged;
    public class OnGridValueChangedEventArgs : EventArgs {
        public int x;
        public int y;
    }
    private int width;
    private int height;
    private float cellSize;

    private Vector3 originPosition;
    private GridObject[,] gridArray;


    public InventoryGrid(int width, int height, float cellSize, Vector3 originPosition, Func<InventoryGrid<GridObject>, int, int, GridObject> createGridObject) {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;

        gridArray = new GridObject[width, height];

        for (int x = 0; x < gridArray.GetLength(0); ++x){
            for (int y = 0; y < gridArray.GetLength(1); ++y){
                gridArray[x, y] = createGridObject(this, x, y);
            }
        }

        bool showDebug = true;
        if (showDebug) {
            //TextMesh[,] debugTextArray = new TextMesh[width, height];

            for (int x = 0; x < gridArray.GetLength(0); ++x){
                for (int y = 0; y < gridArray.GetLength(1); ++y){
                    //debugTextArray[x, y] = CreateWorldText(null, gridArray[x, y]?.ToString(), GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * 0.5f, 20, Color.white, TextAnchor.MiddleCenter, TextAlignment.Left, 5000);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 300f);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 300f);
            }
        }
        Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 300f);
        Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 300f);

        // OnGridValueChanged += (object sender, OnGridValueChangedEventArgs eventArgs) => {
        //     debugTextArray[eventArgs.x, eventArgs.y].text = gridArray[eventArgs.x, eventArgs.y]?.ToString();
        // };
        }
    }

    public Vector3 GetWorldPosition(int x, int y) {
        return new Vector3(x, y) * cellSize;
    }

    public float GetCellSize() {
        return cellSize;
    }

    public void GetXY(Vector3 worldPosition, out int x, out int y) {
        x = Mathf.FloorToInt(worldPosition.x / cellSize);
        y = Mathf.FloorToInt(worldPosition.y / cellSize);
    }

    public void SetGridObject(int x, int y, GridObject value) {
        if (x >= 0 && y >= 0 && x < width && y < height) {
            gridArray[x, y] = value;
            if (OnGridValueChanged != null) OnGridValueChanged(this, new OnGridValueChangedEventArgs { x = x, y = y});
        }
    }

    public void TriggerGridObjectChanged(int x, int y) {
        if (OnGridValueChanged != null) OnGridValueChanged(this, new OnGridValueChangedEventArgs { x = x, y = y});
    }

    public void SetGridObject(Vector3 worldPosition, GridObject value){
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetGridObject(x, y, value);
    }

    public GridObject GetGridObject(int x, int y) {
        if (x >= 0 && y >= 0 && x < width && y < height) {
            return gridArray[x, y];
        }else{
            return default(GridObject);
        }
    }

    public GridObject GetGridObject(Vector3 worldPosition) {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetGridObject(x, y);
    }

    public static TextMesh CreateWorldText(Transform parent, string text, Vector3 localPosition, int fontSize, Color color, TextAnchor textAnchor, TextAlignment textAlignment, int sortingOrder){
        GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
        Transform transform = gameObject.transform;
        transform.SetParent(parent, false);
        transform.localPosition = localPosition;
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.anchor = textAnchor;
        textMesh.alignment = textAlignment;
        textMesh.text = text;
        textMesh.fontSize  = fontSize;
        textMesh.color = color;
        textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
        return textMesh;
    }

    // //Get Mouse Position in World
    // public static Vector3 GetMouseWorldPosition() {
    //     Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
    //     vec.z = 0f;
    //     return vec;
    // }

    // public static Vector3 GetMouseWorldPositionWithZ() {
    //     return GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
    // }

    //     public static Vector3 GetMouseWorldPositionWithZ(Camera worldCamera) {
    //     return GetMouseWorldPositionWithZ(Input.mousePosition, worldCamera);
    // }

    //     public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera) {
    //     Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
    //     return worldPosition;
    // }
}
