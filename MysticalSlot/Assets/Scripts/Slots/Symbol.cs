using UnityEngine;
using UnityEngine.UI;

public class Symbol : MonoBehaviour {
    public enum SymbolsName { Wild, Scatter, Bonus, H1, H2, H3, H4, H5, L1, L2, L3, L4, L5};

    [SerializeField] private Image _image;
    [SerializeField] private SpineClipCreator _animObj;

    private AssetBundle _slotAssetBundle;
    public SymbolsName _identifier;

    public void setData(SymbolsName idName) {
        _identifier = idName;
    }

    //public void refreshImage() {
    //    _image.gameObject.SetActive(true);
    //    _image.sprite = CasinoHelper.loadAssetOfKind<Sprite>(_slotAssetBundle, "Assets/AppAssets/Slot/Symbols/", _identifier.ToString());
    //}

    public void doAnimation(bool val) {
        if (val) {
            _image.gameObject.SetActive(false);
            _animObj.gameObject.SetActive(true);
            _animObj.FullPath = "Assets/AppAssets/Slot/Animation/symbols/";
            _animObj.FolderName = _identifier.ToString();
            string seqName = setAnimDataForSymbolName();
            //print(_identifier);
            _animObj.playClipSeq(-2, seqName, false);
        }
        else {
            _animObj.gameObject.SetActive(false);
            _image.gameObject.SetActive(true);
            _image.sprite = CasinoHelper.loadAssetOfKind<Sprite>(_slotAssetBundle, "Assets/AppAssets/Slot/Symbols/", _identifier.ToString());
        }
    }

    private string setAnimDataForSymbolName()
    {
        
        switch (_identifier)
        {
            case SymbolsName.H1:
                _animObj.AssetName = "HP1";
                return "win";

            case SymbolsName.H2:
                _animObj.AssetName = "HP2";
                return "win";

            case SymbolsName.H3:
                _animObj.AssetName = "RG_HP3";
                return "win";

            case SymbolsName.H4:
                _animObj.AssetName = "HP4";
                return "win";

            case SymbolsName.L1:
                _animObj.AssetName = "A";
                return "WIN";
                
            case SymbolsName.L2:
                _animObj.AssetName = "K";
                return "WIN";

            case SymbolsName.L3:
                _animObj.AssetName = "Q";
                return "WIN";

            case SymbolsName.L4:
                _animObj.AssetName = "J";
                return "WIN";

            case SymbolsName.L5:
                _animObj.AssetName = "10";
                return "WIN";

            case SymbolsName.Wild:
                _animObj.AssetName = "LG_Wild";
                return "win2";

            case SymbolsName.Scatter:
                _animObj.AssetName = "Scatter Regular";
                return "win";


        }
        return "";
    }

}
