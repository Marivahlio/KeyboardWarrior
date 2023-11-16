using Godot;
using System;
using System.Collections.Generic;

public class InteractableKeyData  
{
	private Key key;
	private int indexInRow;
	private int indexInColumn;
	private int indexGlobal;

	public InteractableKeyData(Key pKey, int pIndexInColumn, int pIndexInRow, int pIndexGlobal)
	{
		key = pKey;
		indexInColumn = pIndexInColumn;
		indexInRow = pIndexInRow;
		indexGlobal = pIndexGlobal; 
	}

	public Key GetKey()
	{
		return key;
	}

	public int GetRowIndex()
	{
		return indexInRow;
	}

	public int GetColumnIndex()
	{
		return indexInColumn;
	}

	public int GetGlobalIndex()
	{
		return indexGlobal;
	}
}