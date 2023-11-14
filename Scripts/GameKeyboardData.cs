using Godot;
using System;
using System.Collections.Generic;

public class GameKeyboardData  
{
	private Key key;
	private int indexInRow;
	private int indexInColumn;
	private int indexGlobal;

	public GameKeyboardData()
	{
		key = Key.Escape;
		indexInRow = 0;
		indexInColumn = 0;
		indexGlobal = 0; 
	}

	public GameKeyboardData(Key pKey, int pIndexInRow, int pIndexInColumn, int pIndexGlobal)
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