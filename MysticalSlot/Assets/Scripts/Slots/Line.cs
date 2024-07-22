using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    public List<Symbol> _symbolsList;
	public Symbol.SymbolsName pivotSymbol;
    public List<Symbol> WinningSymbols { get; set; }
    public bool IsWinning { get; set; }
    public float WinMultiplier { get; set; }

	void Awake()
	{
		WinningSymbols = new List<Symbol>();
	}

	public void resetData()
	{
		IsWinning = false;
		WinMultiplier = 0f;
		WinningSymbols.Clear();
	}

	public void highlightLine(bool val, bool isToAnimateElements = false)
	{
		if (WinningSymbols.Count > 0)
		{
			foreach (Symbol winSymbol in WinningSymbols)
				winSymbol.doAnimation(isToAnimateElements);
		}
	}

}
