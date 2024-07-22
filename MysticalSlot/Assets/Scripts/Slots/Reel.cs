using System.Collections.Generic;
using UnityEngine;
//using DG.Tweening;

public class Reel : MonoBehaviour
{
	[SerializeField] public List<Symbol> _symbolsList;
	[SerializeField] private Transform _symbolsContainer;
	[SerializeField] private Animator reelMovementAnim;
    public int _totElements;

	public bool IsActive { get; set; }
	public bool IsInWin { get; set; }

	void Start() {
        _totElements = _symbolsList.Count;
    }

	public void startSpin()
	{
		_symbolsContainer.gameObject.SetActive(false);
		_symbolsContainer.localPosition = new Vector3();
		reelMovementAnim.gameObject.SetActive(true);
		reelMovementAnim.Play(0, -1, Random.Range(0.1f, 0.4f));
		this.enabled = true;
	}

	public void stopSpin()
	{
		this.enabled = false;
		_symbolsContainer.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
		reelMovementAnim.gameObject.SetActive(false);
		_symbolsContainer.gameObject.SetActive(true);
		for (int i = 0; i < _totElements; ++i) {
			_symbolsList[i].doAnimation(false);
		}
		
		//_symbolsContainer.DOLocalMoveY(-50, 0.4f).From().SetEase(Ease.OutBack);

	}

}
