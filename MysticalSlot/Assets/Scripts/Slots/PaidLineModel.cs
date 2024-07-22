using System.Collections.Generic;

public class PaidLineModel {

    public Symbol pivotElement;
    public List<Symbol> paidElements;
    public int elementFrequency;
    public bool wildElementPresent;
    public int wildElementCount;
    public float unitPayout;
    public float totalPayout;

    public PaidLineModel() {}

    public PaidLineModel(Symbol pivEle, int freq, bool wild) {
        pivotElement = pivEle;
        elementFrequency = freq;
        wildElementPresent = wild;
        
    }
}