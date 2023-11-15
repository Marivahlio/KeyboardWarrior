using Godot;
using System;
using System.Collections.Generic;

public class InteractableKeyData  
{
	private Key key;
	private int indexInRow;
	private int indexInColumn;
	private int indexGlobal;

	public InteractableKeyData(Key pKey, int pIndexInRow, int pIndexInColumn, int pIndexGlobal)
	{
		key = pKey;
		indexInRow = pIndexInRow;
		indexInColumn = pIndexInColumn;
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