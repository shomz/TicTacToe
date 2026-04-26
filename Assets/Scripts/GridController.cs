using System;
using UnityEngine;

public class GridController : MonoBehaviour
{
    public enum CellType { Blank, X, O }
    public CellType[] Grid = new CellType[9];

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ResetGrid();
    }

    public void SpawnCells(Action<int> OnClickAction)
    {
        for (int i = Grid.Length - 1; i >= 0; i--)
        {
            if (transform.childCount > i && transform.GetChild(i) != null)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }

        for (int i = 0; i < Grid.Length; i++)
        {
            OnClickAction?.Invoke(i);
        }
    }

    public void ResetGrid()
    {
        Grid = new CellType[9];
    }

    public bool CheckVictory(CellType type)
    {
        for (int i = 0; i < 3; i++)
        {
            // Check rows
            if (Grid[i * 3] == type && Grid[i * 3 + 1] == type && Grid[i * 3 + 2] == type)
            {
                return true;
            }

            // Check columns
            if (Grid[i] == type && Grid[i + 3] == type && Grid[i + 6] == type)
            {
                return true;
            }

            // Diagonals
            if (Grid[0] == type && Grid[4] == type && Grid[8] == type ||
                Grid[2] == type && Grid[4] == type && Grid[6] == type)
            {
                return true;
            }
        }

        return false;
    }
}
