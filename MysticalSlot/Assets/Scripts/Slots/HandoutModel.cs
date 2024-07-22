using System.Collections.Generic;

public class HandoutModel {

	public List<PaidLineModel> paidLines;
	public List<Symbol> scatterElements;
	public List<Symbol> bonusElements;
	public List<Symbol> elementsHeap;
	public int totalScatterElement;
	public int maxOfAKind, idx;
	public int totalBonusElement;
	public bool activateBonus;
	public bool gotFreeSpin;

	public HandoutModel (){

		paidLines = new List<PaidLineModel>(0);
		scatterElements = new List<Symbol>(0);
		bonusElements = new List<Symbol>(0);
		elementsHeap = new List<Symbol>(0);
	}
}
