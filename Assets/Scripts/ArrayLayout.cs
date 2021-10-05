using UnityEngine;
using System.Collections;

[System.Serializable]
public class ArrayLayout  {

	[System.Serializable]
	public struct RowData {
		public bool[] row;
	}

    public Grid grid;
    public RowData[] rows = new RowData[12];
}
