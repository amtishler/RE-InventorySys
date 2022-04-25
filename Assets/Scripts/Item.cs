using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Items")]
public class Item : ScriptableObject
{
    public bool rotated = false;
    
    public string nameString;
    public Transform prefab;
    public Transform visual;
    public int width;
    public int height;

    public int cost = 10;

    public int GetRotationAngle() {
        if (rotated) {
            return 270;
        }
        return 0;
    }

    public Vector2Int GetRotationOffset(bool _rotated) {
        if (_rotated) return new Vector2Int(0, width);
        return new Vector2Int(0, 0);
    }

    public List<Vector2Int> GetGridPositionList(Vector2Int offset, bool _rotated) {
        List<Vector2Int> gridPositionList = new List<Vector2Int>();
        if (_rotated) {
            for (int x = 0; x < height; ++x) {
                for (int y = 0; y < width; ++y) {
                    gridPositionList.Add(offset + new Vector2Int(x, y));
                }
            }
        } else {
            for (int x = 0; x < width; ++x) {
                for (int y = 0; y < height; ++y) {
                    gridPositionList.Add(offset + new Vector2Int(x, y));
                }
            }
        }
        return gridPositionList;
    }
}
