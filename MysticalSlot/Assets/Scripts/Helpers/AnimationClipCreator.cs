using UnityEngine;
using System;
using Spine.Unity;

public class AnimationClipCreator : MonoBehaviour {

    public const int ActionDestroyClip = -1;
    public const int ActionPlayInLoop = -2;

    [HideInInspector] public string resourcePath = string.Empty;
    [HideInInspector] public int position;
    public string FolderName = string.Empty;
    public string AssetName = string.Empty;        
    public string AssetBundleName = string.Empty, FullPath = string.Empty, SeqName = string.Empty;
    public string SpriteLayerName = "Animation";
    public int SpriteLayerValue = 10;
    public Color animColor = Color.white;
    public Vector3 Position = Vector3.zero;
    public Vector3 Scale = Vector3.one;
    public Vector3 Rotation = Vector3.zero;
    public bool Loop = true, HasIndiVisualMaterial;
    public bool IsSpineCreator;    
    [SerializeField] protected bool _playOnLoad = true;
    [SerializeField] protected bool BakeObjects = true;
    protected AssetBundle _aBundle;    
    protected int _childCount, _actionID;
    protected string _seq;
    protected bool _destroyOnFinish, _playImmediately;
    protected Action _callback;    

    public virtual void loadAnimation() { }

    public virtual void initializeData() { }

    public virtual void createClip() { }

    public virtual void playClipSeq(int cCount, string seq, bool destroyOnFinish = true, Action callback = null, bool playImmediately = true, string skin = "default") { }

    public virtual void playClip(int cCount, bool destroyOnFinish = true, Action callback = null, bool playImmediately = true, bool playFromStart = false) { }

    public virtual SkeletonAnimation getAnimationClip() {
        return null;
    }

    public virtual GameObject getAnimationClipObj() {
        return null;
    }

    public virtual string getCurrentClipName() {
        return null;
    }

    public virtual void ChangeAnimationLayer(string layerName) { }

    public virtual void ChangeAnimationLayer(int layerValue) { }

    public virtual void ChangeAnimationLayer(string layerName, int layerValue) { }

    public virtual void destroyAnimationClip() { }

    public virtual void attacheToBoundingBoxes(Transform[] tGos, GameObject comAttachedTo = null, int startIdx = 0) { }

    public virtual float getAnimationDuration(string seq = "default") {
        return 0;
    }

    public virtual void removeCallback() {
        _callback = null;
    }

    public void setupBaseInfoOfAnimation(string assetName, string assetBundleName, string fullPath, int spriteLayerValue = 10, string spriteLayerName = "Animation") {
        AssetName = assetName;
        AssetBundleName = assetBundleName;
        FullPath = fullPath;
        SpriteLayerName = spriteLayerName;
        if (spriteLayerValue != SpriteLayerValue && SpriteLayerValue != 0)
            SpriteLayerValue = spriteLayerValue;
    }
}