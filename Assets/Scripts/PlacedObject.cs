using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacedObject : MonoBehaviour
{
    
    public static PlacedObject Create(Vector3 worldPosition, Vector2Int origin, bool rotate, Item item){
        Transform placedObjectTransform = Instantiate(item.prefab, worldPosition, Quaternion.Euler(0, 0, item.GetRotationAngle()));

        PlacedObject placedObject = placedObjectTransform.GetComponent<PlacedObject>();

        placedObject.placedItem = item;
        placedObject.origin = origin;
        placedObject.rotate = rotate;

        return placedObject;
    }
    private Item placedItem;
    private Vector2Int origin;
    private bool rotate;
    public List<Vector2Int> GetGridPositionList() {
        return placedItem.GetGridPositionList(origin, rotate);
    }

    public void DestroyObject() {
        Destroy(gameObject);
    }

    public Item GetItem(){
        return placedItem;
    }

}
