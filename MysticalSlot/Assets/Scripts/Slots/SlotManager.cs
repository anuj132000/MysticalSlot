using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SlotManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _totalBetText, _winText, _balanceText, freeSpinsTxt;
    [SerializeField] private Button _spinBtn, _autoSpinBtn, _betPlusBtn, _betMinusBtn;
    [SerializeField] private List<Reel> _reelsList;
    [SerializeField] private Transform linesObj, paytableObj;
    [SerializeField] private List<Line> _allLinesList;
    [SerializeField] private SpineClipCreator baseGameBgAnimObj, freeSpinsBgAnimObj, freeSpinsPopupAnimObj, paylineAnimObj, winPopupAnimObj;
    [SerializeField] private AudioSource winningLineAudio, musicAudioSource;

    private int _totalLines, _usedBetIdx, _maxBetIdx, _totalColumns, _totalRows, _totalAutoSpinRound, _freeSpinsAwarded, totalFreeSpinRound;
    private bool _lineEvaluated, _spinSwitchOn, _autoSpinRunning, _isSpinPressed, _isDisableOnWin, _freeSpinsRunning;

    private List<List<int>> _allLinePatternsList;
    private List<Symbol.SymbolsName> _physicalReelSymbols;
    private List<Symbol.SymbolsName> _freeSpinsPhysicalReelSymbols;
    private List<int> _probabilityArray;
    private List<List<Symbol.SymbolsName>> _elementsMatrix;
    private List<float> _betRange;
    private IDictionary<Symbol.SymbolsName, Dictionary<int, float>> paytableData;
    private Dictionary<int, int> freeSpinsTableData;
    private List<Symbol> _scatterSymbolsList;
    private float _totalBetChips, _totalWinPayout, _balanceCoins, _totalBetPerLine;
    private HandoutModel handout;
    private List<Line> _winningLines;

    private const float ReelSpinDuration = 1.0f;
    private const float ReelSpinStopDelay = 0.3f;
    private const float LineHighlightingInterval = 2.0f;
    private const float AllLinesPreviewInterval = 1.0f;

    private const bool IsHackOn = false;

    void Start() {
        _balanceCoins = 10000.0f;
        initSlotData();
    }

    private void initSlotData() {
        this._totalColumns = 5;
        this._totalRows = 3;
        
        _elementsMatrix = new List<List<Symbol.SymbolsName>> {
            new List<Symbol.SymbolsName>{ Symbol.SymbolsName.Scatter, Symbol.SymbolsName.Wild, Symbol.SymbolsName.H2 },    //Reel1 Elements
			new List<Symbol.SymbolsName>{ Symbol.SymbolsName.L5, Symbol.SymbolsName.L1, Symbol.SymbolsName.Wild },    //Reel2 Elements
			new List<Symbol.SymbolsName>{ Symbol.SymbolsName.L4, Symbol.SymbolsName.L3, Symbol.SymbolsName.L3 },    //Reel3 Elements
			new List<Symbol.SymbolsName>{ Symbol.SymbolsName.L2, Symbol.SymbolsName.L2, Symbol.SymbolsName.Wild },    //Reel4 Elements
			new List<Symbol.SymbolsName>{ Symbol.SymbolsName.H4, Symbol.SymbolsName.Scatter, Symbol.SymbolsName.L2 }    //Reel5 Elements
		};

        _physicalReelSymbols = new List<Symbol.SymbolsName>(){
            Symbol.SymbolsName.Scatter,
            Symbol.SymbolsName.H1,
            Symbol.SymbolsName.L1,
            Symbol.SymbolsName.Wild,
            Symbol.SymbolsName.L5,
            Symbol.SymbolsName.H2,
            Symbol.SymbolsName.L4,
            Symbol.SymbolsName.H3,
            Symbol.SymbolsName.L1,
            Symbol.SymbolsName.H4,
            Symbol.SymbolsName.L2,
            Symbol.SymbolsName.Scatter,
            Symbol.SymbolsName.L3,
            Symbol.SymbolsName.L4,
            Symbol.SymbolsName.H3,
            Symbol.SymbolsName.L5,
            Symbol.SymbolsName.H4,
            Symbol.SymbolsName.L2,
            Symbol.SymbolsName.L3,
            Symbol.SymbolsName.Bonus
        };

        _freeSpinsPhysicalReelSymbols = new List<Symbol.SymbolsName>(){
            //Symbol.SymbolsName.Scatter,
            Symbol.SymbolsName.L5,
            Symbol.SymbolsName.H1,
            Symbol.SymbolsName.L1,
            Symbol.SymbolsName.Wild,
            Symbol.SymbolsName.L5,
            Symbol.SymbolsName.H2,
            Symbol.SymbolsName.L4,
            Symbol.SymbolsName.H3,
            Symbol.SymbolsName.L1,
            Symbol.SymbolsName.H4,
            Symbol.SymbolsName.L2,
            //Symbol.SymbolsName.Scatter,
            Symbol.SymbolsName.H4,
            Symbol.SymbolsName.L3,
            Symbol.SymbolsName.L4,
            Symbol.SymbolsName.H3,
            Symbol.SymbolsName.L5,
            Symbol.SymbolsName.H4,
            Symbol.SymbolsName.L2,
            Symbol.SymbolsName.L3,
            Symbol.SymbolsName.Bonus
        };

        _probabilityArray = new List<int>() { 50, 10, 50, 20, 100, 20, 90, 30, 50, 40, 60, 50, 80, 90, 30, 100, 40, 70, 80, 30 };

        this._allLinePatternsList = new List<List<int>> {
            new List<int>{2, 2, 2, 2, 2},
            new List<int>{1, 1, 1, 1, 1},
            new List<int>{3, 3, 3, 3, 3},
            new List<int>{1, 2, 3, 2, 1},
            new List<int>{3, 2, 1, 2, 3},
            new List<int>{2, 1, 1, 1, 2},
            new List<int>{2, 3, 3, 3, 2},
            new List<int>{1, 1, 2, 3, 3},
            new List<int>{3, 3, 2, 1, 1},
            new List<int>{2, 3, 2, 1, 2},
            new List<int>{2, 1, 2, 3, 2},
            new List<int>{1, 2, 2, 2, 1},
            new List<int>{3, 2, 2, 2, 3},
            new List<int>{1, 2, 1, 2, 1},
            new List<int>{3, 2, 3, 2, 3},
            new List<int>{2, 2, 1, 2, 2},
            new List<int>{2, 2, 3, 2, 2},
            new List<int>{1, 1, 3, 1, 1},
            new List<int>{3, 3, 1, 3, 3},
            new List<int>{1, 3, 3, 3, 1},


        };

        paytableData = new Dictionary<Symbol.SymbolsName, Dictionary<int, float>>{

            {Symbol.SymbolsName.Wild ,           new Dictionary<int, float> { {3, 1f}, {4, 10f}, {5, 20f} }},
            {Symbol.SymbolsName.H1 ,             new Dictionary<int, float> { {3, 1f}, {4, 10f}, {5, 20f} }},
            {Symbol.SymbolsName.H2 ,             new Dictionary<int, float> { {3, 0.8f}, {4, 6f}, {5, 16f} }},
            {Symbol.SymbolsName.H3 ,             new Dictionary<int, float> { {3, 0.6f}, {4, 4f}, {5, 12f} }},
            {Symbol.SymbolsName.H4 ,             new Dictionary<int, float> { {3, 0.4f}, {4, 2f}, {5, 8f} }},
            {Symbol.SymbolsName.L1 ,             new Dictionary<int, float> { {3, 0.3f}, {4, 1.2f}, {5, 3f} }},
            {Symbol.SymbolsName.L2 ,             new Dictionary<int, float> { {3, 0.3f}, {4, 1.2f}, {5, 3f} }},
            {Symbol.SymbolsName.L3 ,             new Dictionary<int, float> { {3, 0.3f}, {4, 1.2f}, {5, 3f} }},
            {Symbol.SymbolsName.L4 ,             new Dictionary<int, float> { {3, 0.3f}, {4, 1.2f}, {5, 3f} }},
            {Symbol.SymbolsName.L5 ,             new Dictionary<int, float> { {3, 0.3f}, {4, 1.2f}, {5, 3f} }},
            
        };

        freeSpinsTableData = new Dictionary<int, int> { { 3, 10 }, { 4, 15 }, { 5, 20 } };

        _betRange = new List<float>() { 0.1f, 0.2f, 0.5f, 1.0f, 5.0f, 10.0f, 20.0f, 25.0f };

        this._totalLines = this._allLinePatternsList.Count;
        Reel reelObj;
        _allLinesList = new List<Line>();
        Line line = null;
        List<Symbol> lineSymbolsList;
        List<int> linePattern;
        for (int lineNo = 0; lineNo < _allLinePatternsList.Count; ++lineNo)
        {
            line = linesObj.GetChild(lineNo).GetComponent<Line>();
            linePattern = _allLinePatternsList[lineNo];
            lineSymbolsList = new List<Symbol>();

            for (int i = 0; i < _totalColumns; ++i) {
                reelObj = _reelsList[i];
                lineSymbolsList.Add(reelObj._symbolsList[linePattern[i] - 1]);
            }
            line._symbolsList = lineSymbolsList;
            _allLinesList.Add(line);
        }
        initSlot();
        loadDefaultUIData();

    }

    private void initSlot() {
        baseGameBgAnimObj.gameObject.SetActive(true);
        freeSpinsBgAnimObj.gameObject.SetActive(false);
        playBgMusic();
        paytableObj.gameObject.SetActive(false);

        _winningLines = new List<Line>();
        _scatterSymbolsList = new List<Symbol>();
        _usedBetIdx = 1;
        _maxBetIdx = _betRange.Count - 1;
        checkForBetBtnsEnable();
        _totalBetPerLine = _betRange[_usedBetIdx];
        _totalBetChips = (_totalBetPerLine * _totalLines);
        updateTotalBetTxt();
    }

    private void loadDefaultUIData() {
        removeAllHighlightedLines();
        updateTotalBetTxt();
        updateTotalWinTxt();
        updateChips(0);
        Reel reelObj;
        Symbol reelSymbol;
        for (int i = 0; i < _totalColumns; ++i) {
            reelObj = _reelsList[i];
            reelObj.IsActive = true;
            
            for(int j = 0; j < _totalRows; ++j) {
                reelSymbol = reelObj._symbolsList[j];
                reelSymbol.setData(_elementsMatrix[i][j]);
                reelSymbol.doAnimation(false);
            }

        }
    }

    private void updateTotalBetTxt() {
        if (_totalBetText != null)
            _totalBetText.text = _totalBetChips.ToString("F2");
    }

    private void updateTotalWinTxt() {
        if (_winText != null)
            _winText.text = _totalWinPayout.ToString("F2");
    }

    public void btnsHandler(int btnTagNo) {
        if (_freeSpinsRunning || (_isDisableOnWin && btnTagNo <= 4))
            return;

        switch (btnTagNo) {
            case 1:
                endAutoSpin();
                break;
            case 2:
                spinBtnHandler();
                break;
            case 3:
                betPlusBtnHandler();
                break;
            case 4:
                betMinusBtnHandler();
                break;
            case 5:
                payTableBtnHandler();
                break;

        }
    }

    private void betPlusBtnHandler() {
        if (_autoSpinRunning || _usedBetIdx == _maxBetIdx)
            return;
        
        _usedBetIdx = Mathf.Min(_maxBetIdx, (_usedBetIdx + 1));
        checkForBetsUpdate();
    }

    private void betMinusBtnHandler() {
        if (_autoSpinRunning || _usedBetIdx == 0)
            return;
        
        _usedBetIdx = Mathf.Max(0, (_usedBetIdx - 1));
        checkForBetsUpdate();
    }

    private void checkForBetsUpdate() {
        _totalBetPerLine = _betRange[_usedBetIdx];
        _totalBetChips = (_totalLines * _totalBetPerLine);
        updateTotalBetTxt();
        checkForBetBtnsEnable();
    }

    private void checkForBetBtnsEnable() {
        _betMinusBtn.enabled = (_usedBetIdx > 0);
        _betPlusBtn.enabled = (_usedBetIdx < _betRange.Count - 1);
    }

    public void spinBtnDownHandler() {
        if (!_isDisableOnWin) {
            _isSpinPressed = true;
            Invoke("checkForLongPressed", 1.0f);
        }
    }

    public void spinBtnUpHandler() {
        if (!_isDisableOnWin)
            _isSpinPressed = false;
    }

    private void checkForLongPressed() {
        if (_isSpinPressed)
            startAutoSpin();
    }

    private void startAutoSpin() {
        _autoSpinRunning = true;
        _totalAutoSpinRound = 1000000;
        removeAllHighlightedLines();
        setAutoSpinUIOnRunning(true);
        spinBtnHandler();
    }

    private void setAutoSpinUIOnRunning(bool val) {
        _spinBtn.gameObject.SetActive(!val);
        _autoSpinBtn.gameObject.SetActive(val);
        _autoSpinBtn.enabled = val;
    }

    private void endAutoSpin() {
        _autoSpinRunning = false;
        _totalAutoSpinRound = 0;
        setAutoSpinUIOnRunning(false);
        if (_lineEvaluated)
            checkForBetBtnsEnable();
    }

    private void spinBtnHandler() {
        CancelInvoke();
        if (_spinSwitchOn && !_lineEvaluated) {
            quickResetAndUpdateUI();
            _spinSwitchOn = false;
            return;
        }
        else {
            
            if (_freeSpinsRunning && totalFreeSpinRound > 0) {
                totalFreeSpinRound--;
                freeSpinsTxt.text = "Remainig Free Spins : " + totalFreeSpinRound.ToString();

            }
            _spinSwitchOn = true;
        }

        if (_totalBetChips > _balanceCoins)
        {
            //Show Low Balance
            return;
        }

        if (_autoSpinRunning && _totalAutoSpinRound > 0) {
            _totalAutoSpinRound--;
        }

        initNewRound();
        preSpinSetup();
    }

    private void quickResetAndUpdateUI() {
        if (!_lineEvaluated) { 
            CancelInvoke();
            //reLayoutSlotViewElements();
            foreach (Reel reel in _reelsList) {
                reel.CancelInvoke();
                reel.stopSpin();
            }
            slotDidStop();
        }
    }

    private void initNewRound() {
        _totalWinPayout = 0;
        _lineEvaluated = false;
        _winningLines.Clear();
        _scatterSymbolsList.Clear();
        removeAllHighlightedLines();
        foreach (Line fsLine in _allLinesList)
            fsLine.resetData();
        paylineAnimObj.gameObject.SetActive(false);

    }

    private void preSpinSetup() {
        deductBetChips();
        StopCoroutine("checkForNextHighlight");
        //removeExitementBorder();
        resetUIOnSpin();
        shuffleElements();
        scheduleReelSpin();
    }

    private void resetUIOnSpin() {
        updateTotalWinTxt();
        enableUserInteraction(false);
    }

    private void enableUserInteraction(bool val) {
        _betPlusBtn.enabled = val;
        _betMinusBtn.enabled = val;
    }

    private void shuffleElements() {
        if (IsHackOn && !_freeSpinsRunning)
            return;

        Reel reelObj;
        for (int i = 0; i < _totalColumns; ++i) {
            reelObj = _reelsList[i];
            if (reelObj.IsActive)
                _elementsMatrix[i] = getNextElementsSlice(reelObj);

            for (int j = 0; j < _totalRows; ++j) {
                Symbol reelSymbol = reelObj._symbolsList[j];
                reelSymbol.setData(_elementsMatrix[i][j]);
            }
        }

    }

    public List<Symbol.SymbolsName> getNextElementsSlice(Reel reel)
    {
        int probabilityTotal = 0;
        foreach (int no in _probabilityArray)
            probabilityTotal += no;
        int randNo = UnityEngine.Random.Range(0, probabilityTotal);
        int fallingIdx = 0, range = 0;
        foreach (int number in _probabilityArray)
        {
            range += number;
            if (randNo < range)
                break;
            fallingIdx++;
        }
        int totCount = _physicalReelSymbols.Count;
        fallingIdx += (-Mathf.FloorToInt(reel._totElements / 2) + totCount);
        List<Symbol.SymbolsName> slicedElements = new List<Symbol.SymbolsName>(reel._totElements);
        for (int i = fallingIdx; i < (fallingIdx + reel._totElements); i++)
        {
            slicedElements.Add(_freeSpinsRunning? _freeSpinsPhysicalReelSymbols[i % totCount] : _physicalReelSymbols[i % totCount]);
        }

        return slicedElements;
    }

    private void scheduleReelSpin() {
        Reel reelObj;
        for (int i = 0; i < _totalColumns; ++i) {
            reelObj = _reelsList[i];

            if (reelObj.IsActive) {
                reelObj.startSpin();
            }
        }
        Invoke("preStopSetup", ReelSpinDuration);
    }

    private void preStopSetup() {
        scheduleReelStop();
    }

    private void scheduleReelStop() {
        Reel reelObj;
        float actualDelay = 0.0f;
        for (int i = 0; i < _totalColumns; ++i) {
            reelObj = _reelsList[i];
            if (i > 0)
                actualDelay += ReelSpinStopDelay;
            reelObj.Invoke("stopSpin", actualDelay);
        }
        Invoke("slotDidStop", actualDelay);
    }

    private void slotDidStop() {
        //_isSpining = false;
        if (!(_autoSpinRunning && _totalAutoSpinRound > 0)) {
            enableUserInteraction(true);
            checkForBetBtnsEnable();
        }
        evaluateActiveLines();
    }

    private void evaluateActiveLines() {
        Line line;
        float totalWinMultiplier = 0;
        handout = new HandoutModel();
        for (int i = 0; i < _totalLines; ++i) {
            line = _allLinesList[i];

            PaidLineModel resultantPaidLine = frequencyOfPivotElementIntoLineOnForwardDirection(line);
            resultantPaidLine.unitPayout = getPayoutUsingInvolvedElement(resultantPaidLine.pivotElement, resultantPaidLine.elementFrequency);
            if (resultantPaidLine.unitPayout > 0)
            {
                handout.paidLines.Add(resultantPaidLine);
                for (int x = 0; x < resultantPaidLine.elementFrequency; ++x) {
                    line.WinningSymbols.Add(line._symbolsList[x]);
                    _reelsList[x].IsInWin = true;
                }

                line.IsWinning = true;
                line.WinMultiplier = resultantPaidLine.unitPayout;
                line.pivotSymbol = resultantPaidLine.pivotElement._identifier;
            }
            if (i <= 2)
            {
                // For First 3-Lines; As it covers all visible elements of Slot..
                findScatterElementsIntoLine(handout, line);
                handout.gotFreeSpin = (handout.totalScatterElement >= 3);

            }
            if (line.IsWinning)
            {
                totalWinMultiplier += line.WinMultiplier;
                _winningLines.Add(line);
            }
        }
        _lineEvaluated = true;
        _totalWinPayout = (totalWinMultiplier * _totalBetPerLine);
        checkForLargeWins();
    }

    private void findScatterElementsIntoLine(HandoutModel handout, Line line)
    {
        foreach (Symbol element in line._symbolsList)
        {
            if (element._identifier == Symbol.SymbolsName.Scatter)
            {
                handout.scatterElements.Add(element);
                handout.totalScatterElement++;
                _scatterSymbolsList.Add(element);
            }
        }
    }

    private float getPayoutUsingInvolvedElement(Symbol element, int count)
    {
        float payoutMultiplier = count >= 3 ? paytableData[element._identifier][count] : 0;
        return payoutMultiplier;
    }


    private PaidLineModel frequencyOfPivotElementIntoLineOnForwardDirection(Line line) {
        int occurenceCount = 0;
        int len = line._symbolsList.Count;
        bool wildElementPresent = false;
        Symbol pivotElement = line._symbolsList[0];
        Symbol.SymbolsName pivotElementId = pivotElement._identifier;
        for (int idx = 0; idx < len; idx++) {
            Symbol innerElement = line._symbolsList[idx];
            Symbol.SymbolsName innerElementId = innerElement._identifier;
            if (!(innerElementId == Symbol.SymbolsName.Scatter || innerElementId == Symbol.SymbolsName.Bonus) && (pivotElementId == Symbol.SymbolsName.Wild || innerElementId == Symbol.SymbolsName.Wild || pivotElementId == innerElementId)) {
                occurenceCount++;
                if (!(innerElementId == Symbol.SymbolsName.Wild)) {
                    pivotElement = innerElement;
                    pivotElementId = pivotElement._identifier;
                }
                else
                    wildElementPresent = true;
            }
            else
                break;
        }

        PaidLineModel paidLine = new PaidLineModel(pivotElement, occurenceCount, wildElementPresent);
        return paidLine;
    }

    private void checkForLargeWins() {
        if (_totalWinPayout >= 1f * _totalBetChips)
        {
            winPopupAnimObj.playClipSeq(0, "02_appear_superwin", false, () => {
                winPopupAnimObj.playClipSeq(0, "02_idle_superwin", false, () => {
                    winPopupAnimObj.playClipSeq(0, "02_hide_superwin", false, () => {

                        updateAndCheckForNextRound();
                    });
                });
            });
        }
        else if(_totalWinPayout >= 0.5f * _totalBetChips) {
            winPopupAnimObj.playClipSeq(0, "01_appear_bigwin", false, () => {
                winPopupAnimObj.playClipSeq(0, "01_idle_bigwin", false, () => {
                    winPopupAnimObj.playClipSeq(0, "01_hide_bigwin", false, () => {

                        updateAndCheckForNextRound();
                    });
                });
            });
        }
        else
            updateAndCheckForNextRound();


    }

    private void updateAndCheckForNextRound() {

        if (_totalWinPayout > 0 || handout.gotFreeSpin) {
            handleCoinsChange();
            CancelInvoke();
            _isDisableOnWin = true;
            if (handout.gotFreeSpin)
                setUpFreeSpins();
            else {
                Invoke("resumeBtns", 0.75f);
                finishRound();
            }
        }
        else {
            if (!_freeSpinsRunning) {
                _isDisableOnWin = true;
                Invoke("resumeBtns", 0.25f);
            }
            finishRound();
        }
        checkForBetBtnsEnable();
    }

    private void handleCoinsChange() {
        updateTotalWinTxt();
        updateChips(_totalWinPayout, false);
    }

    private void finishRound() {
        if (_winningLines.Count > 0) {
            int index = (_winningLines.Count == 1) ? 0 : -1;
            StartCoroutine(checkForNextHighlight(index, 0));
        }

        if (_freeSpinsRunning) {
            if (totalFreeSpinRound > 0)
                Invoke(nameof(spinBtnHandler), 1.0f);
            else
                Invoke(nameof(finishFreeSpin), 1.0f);
        }
        else
            checkForAutoSpin();
    }

    private void setUpFreeSpins() {
        foreach (Symbol scatterSymbol in _scatterSymbolsList)
            scatterSymbol.doAnimation(true);

        _freeSpinsAwarded = freeSpinsTableData[handout.totalScatterElement];

        Invoke(nameof(initFreeSpins), 2.0f);
    }

    private void initFreeSpins() {

        if (_autoSpinRunning) {
            endAutoSpin();
        }

        freeSpinsPopupAnimObj.playClipSeq(0, "EntrytoFreeSpins_intro", false, () => {
            foreach (Symbol scatterSymbol in _scatterSymbolsList)
                scatterSymbol.doAnimation(false);

            freeSpinsPopupAnimObj.playClipSeq(0, "EntrytoFreeSpins_loop", false, () => {
                freeSpinsPopupAnimObj.playClipSeq(0, "EntrytoFreeSpins_outro", false, ()=> {
                    baseGameBgAnimObj.gameObject.SetActive(false);
                    freeSpinsBgAnimObj.gameObject.SetActive(true);
                    playBgMusic(true);
                    startFreeSpin();
                });
            });
        });
    }

    public void startFreeSpin() {
        _freeSpinsRunning = true;
        totalFreeSpinRound = _freeSpinsAwarded;
        _totalWinPayout = 0;

        CancelInvoke();
        StopAllCoroutines();

        freeSpinsTxt.text = "Remainig Free Spins : " + totalFreeSpinRound.ToString();
        Invoke(nameof(spinBtnHandler), 1.0f);
    }

    private void finishFreeSpin() {
        freeSpinsPopupAnimObj.playClipSeq(0, "summary_panel_intro", false, () => {
            freeSpinsPopupAnimObj.playClipSeq(0, "summary_panel_loop", false, () => {
                freeSpinsPopupAnimObj.playClipSeq(0, "summary_panel_outro", false, () => {
                    baseGameBgAnimObj.gameObject.SetActive(true);
                    freeSpinsBgAnimObj.gameObject.SetActive(false);
                    playBgMusic();
                    freeSpinsTxt.text = "";

                    _freeSpinsRunning = false;
                   
                });
            });
        });
    }

    private void checkForAutoSpin() {
        if (_autoSpinRunning && _totalAutoSpinRound > 0) {
            float delayTime = 0.75f;
            if (_totalWinPayout > 0)
                delayTime = LineHighlightingInterval * _winningLines.Count;
            Invoke("spinBtnHandler", delayTime);
        }
    }

    private void resumeBtns() {
            _isDisableOnWin = false;
    }

    private IEnumerator checkForNextHighlight(int index, float delay) {
        yield return new WaitForSeconds(delay);

        if (_winningLines.Count > 0) {
            float nextTime = LineHighlightingInterval;
            removeAllHighlightedLines();
            if (index == -1) {
                foreach (Line winLine in _winningLines)
                    winLine.highlightLine(true);
                nextTime = AllLinesPreviewInterval;
                paylineAnimObj.gameObject.SetActive(false);

            }
            else {
                Line winningLine = _winningLines[index];
                winningLine.highlightLine(true, true);
                paylineAnimObj.gameObject.SetActive(true);
                paylineAnimObj.playClipSeq(0, "payline_" + (_allLinesList.IndexOf(winningLine) + 1).ToString(), false);
                playWinningLineSound(winningLine.pivotSymbol);
            }
            index = (_winningLines.Count == 1) ? 0 : ((index == (_winningLines.Count - 1)) ? -1 : (index + 1));
            StartCoroutine(checkForNextHighlight(index, nextTime));
        }
    }

    private void removeAllHighlightedLines() {
        foreach (Line line in _allLinesList)
            line.highlightLine(true);
    }

    private void deductBetChips() {
        if (_freeSpinsRunning)
            return;
        updateChips(_totalBetChips, true);
    }

    private void updateChips(float changeInVal, bool isToDeduct = false) {
        if (isToDeduct)
            _balanceCoins -= changeInVal;
        else
            _balanceCoins += changeInVal;

        _balanceText.text = _balanceCoins.ToString("F2");
    }

    private void payTableBtnHandler() {
        paytableObj.gameObject.SetActive(true);
    }

    private void playWinningLineSound(Symbol.SymbolsName pivotSymbol) {
        
        winningLineAudio.clip = CasinoHelper.loadAssetOfKind<AudioClip>(null, "Assets/AppAssets/Slot/Sounds/", getSoundClipNameForWinningLine(pivotSymbol),"wav");
        winningLineAudio.Play();
    }

    private string getSoundClipNameForWinningLine(Symbol.SymbolsName pivotSymbolName)
    {
        switch (pivotSymbolName)
        {
            case Symbol.SymbolsName.H1: return "highPay1";
            case Symbol.SymbolsName.H2: return "highPay2";
            case Symbol.SymbolsName.H3: return "highPay3";
            case Symbol.SymbolsName.H4: return "highPay4";
            case Symbol.SymbolsName.L1:
            case Symbol.SymbolsName.L2:
            case Symbol.SymbolsName.L3:
            case Symbol.SymbolsName.L4:
            case Symbol.SymbolsName.L5: return "lowPay";
            case Symbol.SymbolsName.Wild: return "lowPay";
            default:   return "";
        }
    }

    private void playBgMusic(bool forFreeSpins = false) {
        musicAudioSource.clip = CasinoHelper.loadAssetOfKind<AudioClip>(null, "Assets/AppAssets/Slot/Sounds/", forFreeSpins ? "music_freespin" : "music_main", "wav");
        musicAudioSource.Play();

    }
}
