using Spine.Unity;
using UnityEngine;

public class SpineBoundingBoxAttacher : MonoBehaviour {

    public string SlotName;
    public string BoneName;

    GameObject _thisGO;
    Transform _originalParent;
    SpineClipCreator _spineCreatorComp;
    BoundingBoxFollower _bbFollowerComp;
    BoneFollower _bFollowerComp;

    void Awake() {
        Debug.Log("SpineBoundingBoxAttacher.Awake()...SlotName :" + SlotName);
        createBoundingBoxAttacher();
    }

    void OnEnable() {
        if (_thisGO == null) {
            Debug.Log("SpineBoundingBoxAttacher.OnEnable()...SlotName :" + SlotName);            
            createBoundingBoxAttacher(false);
        }
    }

    public void createBoundingBoxAttacher(bool attachCB = true) {
        _spineCreatorComp = GetComponent<SpineClipCreator>();
        _spineCreatorComp.boundingBoxAttacher = this;
        if (attachCB) {
            _spineCreatorComp.OnAnimStartCBs.Add(attacheToSpine);
        }
        if (_thisGO == null) {
            _thisGO = new GameObject(SlotName);
            _thisGO.transform.SetParent(transform, false);
            _bbFollowerComp = _thisGO.AddComponent<BoundingBoxFollower>();
            _bFollowerComp = _thisGO.AddComponent<BoneFollower>();
            _bbFollowerComp.slotName = SlotName;
            _bFollowerComp.boneName = BoneName;
            _bFollowerComp.followLocalScale = true;
        }
    }

    private void attacheToSpine() {
        var sAnim = _spineCreatorComp.getAnimationClip();
        if (_thisGO != null && _bbFollowerComp.skeletonRenderer == null) {
            _thisGO.transform.SetParent(sAnim.transform, false);
            _bbFollowerComp.skeletonRenderer = sAnim;
            _bFollowerComp.skeletonRenderer = sAnim;
        }
    }

    public void attacheToBoundingBox(Transform tGo) {
        if (_thisGO != null) {
            _originalParent = tGo.parent;
            tGo.transform.SetParent(_thisGO.transform);
            _bbFollowerComp.slotName = SlotName;
            _bFollowerComp.boneName = BoneName;
        }
    }

    public void detacheFromBoundingBox(Transform tGo, bool worldPosiStays = false) {
        if (tGo != null) {
            tGo.transform.SetParent(_originalParent, worldPosiStays);
        }
    }
}
