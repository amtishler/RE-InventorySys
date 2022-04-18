using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    [SerializeField] private List<Item> itemList;
    private Item item;
    private InventoryGrid<CustomGridObject> grid;
    private void Awake(){
        int gridWidth = 10;
        int gridHeight = 10;
        float cellSize = 10f;
        grid = new InventoryGrid<CustomGridObject>(gridWidth, gridHeight, cellSize, Vector3.zero, (InventoryGrid<CustomGridObject> g, int x, int y) => new CustomGridObject(g, x, y));

        item = itemList[0];
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            grid.GetXY(GetMouseWorldPosition(), out int x, out int y);

            List<Vector2Int> gridPositionList = item.GetGridPositionList(new Vector2Int(x, y), item.rotated);
            bool occupied = false;
            foreach (Vector2Int gridPosition in gridPositionList) {
                if (grid.GetGridObject(gridPosition.x, gridPosition.y).Occupied()) {
                    occupied = true;
                    break;
                }
            }

            if (!occupied) {
                Vector2Int rotationOffset = item.GetRotationOffset(item.rotated);
                Vector3 placedObjectWorldPosition = grid.GetWorldPosition(x, y) + 
                        new Vector3(rotationOffset.x, rotationOffset.y, 0) * grid.GetCellSize();
                Transform placedTransform = Instantiate(item.prefab, placedObjectWorldPosition, Quaternion.Euler(0, 0, item.GetRotationAngle()));
                foreach (Vector2Int gridPosition in gridPositionList) {
                    grid.GetGridObject(gridPosition.x, gridPosition.y).SetTransform(placedTransform);
                }
            }
            // else {
            //     Put optional notification here!
            // }

            // if (Input.GetMouseButtonDown(1)) {
            //     if (item.rotated) {
            //         item.rotated = false;
            //     } else {
            //         item.rotated = true;
            //     }
            // }
            // CustomGridObject customObject = grid.GetGridObject(GetMouseWorldPosition());
            // if (customObject != null) {
            //     customObject.AddValue(5);
            // }
        }
        if (Input.GetMouseButtonDown(1)) {
            if (item.rotated) {
                item.rotated = false;
            } else {
                item.rotated = true;
            }
        }
    }

    //Modify Objects in Grid
    public class CustomGridObject {
        // private const int MIN = 0;
        // private const int MAX = 100;

        private InventoryGrid<CustomGridObject> grid;
        private int x;
        private int y;
        // private int value;
        private Transform transform;

        public CustomGridObject(InventoryGrid<CustomGridObject> grid, int x, int y) {
            this.grid = grid;
            this.x = x;
            this.y = y;
        }

        public void SetTransform(Transform transform) {
            this.transform = transform;
            grid.TriggerGridObjectChanged(x, y);
        }

        public void DeleteTransform() {
            transform = null;
            grid.TriggerGridObjectChanged(x, y);
        }

        public bool Occupied() {
            return (transform != null);
        }

        // public void AddValue(int addValue) {
        //     value += addValue;
        //     value = Mathf.Clamp(value, MIN, MAX);
        //     grid.TriggerGridObjectChanged(x, y);
        // }

        // public float GetValueNormalized() {
        //     return (float)value / MAX;
        // }

        // public override string ToString() {
        //     return value.ToString();
        // }
    }

        //Get Mouse Position in World
    public static Vector3 GetMouseWorldPosition() {
        Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        vec.z = 0f;
        return vec;
    }

    public static Vector3 GetMouseWorldPositionWithZ() {
        return GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
    }

        public static Vector3 GetMouseWorldPositionWithZ(Camera worldCamera) {
        return GetMouseWorldPositionWithZ(Input.mousePosition, worldCamera);
    }

        public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera) {
        Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
        return worldPosition;
    }
}

