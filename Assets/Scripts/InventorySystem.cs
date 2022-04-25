using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    [SerializeField] private List<Item> itemList;
    private Item item;
    private InventoryGrid<CustomGridObject> grid;

    public int money = 750;
    public Text currency;

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
                if (grid.GetGridObject(gridPosition.x, gridPosition.y) != null) {
                    if (grid.GetGridObject(gridPosition.x, gridPosition.y).Occupied()) {
                        occupied = true;
                        break;
                    }
                } else {
                    occupied = true;
                    break;
                }
    
            }

            if (!occupied && money > item.cost) {
                money -= item.cost;
                Vector2Int rotationOffset = item.GetRotationOffset(item.rotated);
                Vector3 placedObjectWorldPosition = grid.GetWorldPosition(x, y) + 
                        new Vector3(rotationOffset.x, rotationOffset.y, 0) * grid.GetCellSize();
                PlacedObject placedObject = PlacedObject.Create(placedObjectWorldPosition, new Vector2Int(x, y), item.rotated, item);
                foreach (Vector2Int gridPosition in gridPositionList) {
                    grid.GetGridObject(gridPosition.x, gridPosition.y).SetPlacedObject(placedObject);
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

        if (Input.GetKeyDown(KeyCode.S)) {
            CustomGridObject gridObject = grid.GetGridObject(GetMouseWorldPosition());
            PlacedObject placedObject = gridObject.GetPlacedObject();
            if (placedObject != null) {
                money += placedObject.GetItem().cost;
                placedObject.DestroyObject();

                List<Vector2Int> gridPositionList = placedObject.GetGridPositionList();

                foreach (Vector2Int gridPosition in gridPositionList) {
                    grid.GetGridObject(gridPosition.x, gridPosition.y).DeletePlacedObject();
                }
            }
        }
        if (currency != null)
            currency.text = "$" + money.ToString();

        if (Input.GetKeyDown(KeyCode.Alpha1)) { 
            item = itemList[0]; 
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            item = itemList[1]; 
        }
        if (Input.GetKeyDown(KeyCode.Alpha3)) { 
            item = itemList[2]; 
        }
        if (Input.GetKeyDown(KeyCode.Alpha4)) { 
            item = itemList[3]; 
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
        private PlacedObject placedObject;

        public CustomGridObject(InventoryGrid<CustomGridObject> grid, int x, int y) {
            this.grid = grid;
            this.x = x;
            this.y = y;
        }

        public void SetPlacedObject(PlacedObject placedObject) {
            this.placedObject = placedObject;
            grid.TriggerGridObjectChanged(x, y);
        }

        public PlacedObject GetPlacedObject() {
            return placedObject;
        }

        public void DeletePlacedObject() {
            placedObject = null;
            grid.TriggerGridObjectChanged(x, y);
        }

        public bool Occupied() {
            return placedObject != null;
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

