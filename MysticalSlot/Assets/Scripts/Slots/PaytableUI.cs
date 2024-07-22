
using UnityEngine;
using UnityEngine.UI;

public class PaytableUI : MonoBehaviour
{
    
    [SerializeField] private Button leftBtn, rightBtn;
    [SerializeField] private Transform pagesContainer;

    private int currPaytablePageNo, totalPages;

    private void Start() {
        totalPages = pagesContainer.childCount;
    }

    private void OnEnable() {
        currPaytablePageNo = 0;
        updatePage();
    }

    public void leftBtnHandler()
    {
        currPaytablePageNo = Mathf.Max(0, currPaytablePageNo - 1);
        updatePage();
    }

    public void rightBtnHandler()
    {
        currPaytablePageNo = Mathf.Min(totalPages - 1, currPaytablePageNo + 1);
        updatePage();
    }

    private void updatePage() {
        for (int i = 0; i < totalPages; ++i)
            pagesContainer.GetChild(i).gameObject.SetActive(false);

        pagesContainer.GetChild(currPaytablePageNo).gameObject.SetActive(true);
        leftBtn.interactable = currPaytablePageNo > 0;
        rightBtn.interactable = currPaytablePageNo < totalPages - 1;
    }

    public void closeBtnHandler() {
        this.gameObject.SetActive(false);
    }
}
