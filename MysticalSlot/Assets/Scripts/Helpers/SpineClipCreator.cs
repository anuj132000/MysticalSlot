using System;
using System.Collections.Generic;
using Spine;
using Spine.Unity;
using UnityEngine;

public class SpineClipCreator : AnimationClipCreator {

    [HideInInspector]
    public SpineBoundingBoxAttacher boundingBoxAttacher;
    public List<Action> OnAnimStartCBs = new List<Action>(0);

    private SkeletonAnimation _skAnim;
    private MeshRenderer _mRenderer;    
    private string _skin;

    private void Awake() {
        IsSpineCreator = true;
    }

    public void Start() {
        loadAnimation();
    }

    public override void loadAnimation() {
        if (_skAnim == null) {
            initializeData();
            createClip();
            if (_playOnLoad && !string.IsNullOrEmpty(SeqName)) {
                playClipSeq(0, SeqName, false);
            }
        }
    }

    public override void initializeData() {
        if (AssetBundleName != null) {
            //_aBundle = string.IsNullOrEmpty (AssetBundleName) ? null : AssetLoader.Instance.GetLoadedAssetBundle (AssetBundleName);
            //FolderName = string.IsNullOrEmpty(FolderName) ? AssetName : FolderName;
            resourcePath = FullPath.Contains(AssetName + "/") ? FullPath : FullPath + FolderName + "/";
            //Debug.Log("SpineClipCreator::createClip()....resourcePath : " + resourcePath + " FullPath : " + FullPath + " AssetName : " + AssetName + " FolderName : " + FolderName);
        }
    }

    public override void createClip() {
        if ((_aBundle != null || !string.IsNullOrEmpty(resourcePath)) && !string.IsNullOrEmpty(AssetName)) { 
            GameObject newGo = new GameObject(AssetName);
            newGo.transform.parent = transform;
            newGo.transform.localScale = Scale;
            newGo.transform.localPosition = Position;
            newGo.transform.localEulerAngles = Rotation;
            _skAnim = newGo.AddComponent<SkeletonAnimation>();
            _skAnim.skeletonDataAsset = CasinoHelper.loadAssetOfKind<SkeletonDataAsset>(_aBundle, resourcePath, AssetName + "_SkeletonData", "asset");
            _skAnim.skeletonDataAsset.scale = 1;
            _skAnim.skeletonDataAsset.defaultMix = 0;
            _mRenderer = _skAnim.GetComponent<MeshRenderer>();
            _mRenderer.sortingOrder = SpriteLayerValue;
            _mRenderer.sortingLayerName = SpriteLayerName;
            initSkeletonData(_skAnim.initialSkinName);
            gameObject.SetActive(_playOnLoad);
        }
    }

    private void initSkeletonData(string skin) {
        _skAnim.initialSkinName = skin;
        _skAnim.Initialize(true);
        _skAnim.AnimationState.Start += onAnimStart;
        _skAnim.AnimationState.End += onAnimEnd;
        _skAnim.AnimationState.Complete += onAnimComplete;  //This callback may occur multiple times if looped.
    }

    public override void playClipSeq(int actionID, string seq = "default", bool destroyOnFinish = true, Action callback = null, bool playImmediately = true, string skin = "default") {
        _actionID = actionID;
        _seq = string.IsNullOrEmpty(seq) ? "default" : seq;
        _skin = skin;
        _destroyOnFinish = destroyOnFinish;
        _playImmediately = playImmediately;
        loadAnimation();
        playClip(actionID, destroyOnFinish, callback, playImmediately);
    }

    public override void playClip(int cCount, bool destroyOnFinish = true, Action callback = null, bool playImmediately = true, bool playFromStart = false) {
        _childCount = cCount;
        _callback = callback;
        _destroyOnFinish = destroyOnFinish;
        if (_skAnim.initialSkinName != _skin) {
            initSkeletonData(_skin);
        }
        _skAnim.skeletonDataAsset.defaultMix = 0;               //To Fix/Avoid Jerk
        _skAnim.AnimationState.SetAnimation(0, _seq, Loop || _actionID == ActionPlayInLoop);
    }

    private void onAnimStart(TrackEntry trackEntry) {
        foreach(var action in OnAnimStartCBs) {
            action.Invoke();
        }
    }

    private void onAnimEnd(TrackEntry trackEntry) {
        //Debug.Log("onAnimEnd....");
    }

    private void onAnimComplete(TrackEntry trackEntry) {
        //Debug.Log("onAnimComplete....");
        _callback?.Invoke();
        if (_destroyOnFinish) {
            Destroy(gameObject);
        } else if (_actionID == ActionDestroyClip) {
            destroyAnimationClip();
        }
    }

    public override SkeletonAnimation getAnimationClip() {
        return _skAnim;
    }

    public override string getCurrentClipName() {
        return _seq;
    }

    public override void ChangeAnimationLayer(string layerName) {
        if (_mRenderer != null) {
            _mRenderer.sortingLayerName = layerName;
        }
    }

    public override void ChangeAnimationLayer(int layerValue) {
        if(_mRenderer != null) {
            _mRenderer.sortingOrder = layerValue;
        }
    }

    public override void ChangeAnimationLayer(string layerName, int layerValue) {
        if (_mRenderer != null) {
            _mRenderer.sortingOrder = layerValue;
            _mRenderer.sortingLayerName = layerName;
        }
    }

    public override void destroyAnimationClip() {
        if (_skAnim != null) {
            Destroy(_skAnim.gameObject);
            _skAnim = null;
        }
    }

    public override void attacheToBoundingBoxes(Transform[] tGos, GameObject whereToFindComp = null, int startIdx = 0) {  //Order is importeant :- tGos order should be same as 'SpineBoundingBoxAttacher' Components order in Editor...
        var comSource = whereToFindComp == null ? gameObject : whereToFindComp; //Where is the Compontent "SpineBoundingBoxAttacher" attached...
        var boxAs = comSource.GetComponents<SpineBoundingBoxAttacher>();
        int minC = Math.Min(boxAs.Length, tGos.Length);   //Avoiding out of bound exception...
        for (int idx = 0; idx < minC; idx++) {
            boxAs[idx].attacheToBoundingBox(tGos[idx + startIdx]);
        }
    }

    public override float getAnimationDuration(string seq = "default") {
        if (_skAnim != null)
            return _skAnim.Skeleton.Data.FindAnimation(seq).Duration;
        return 0.0f;
    }
}