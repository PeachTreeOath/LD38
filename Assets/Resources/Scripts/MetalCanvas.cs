using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MetalCanvas : MonoBehaviour {

    public Text MetalText;

    public float _duration = 1.0f;
    public float _scale = 1.5f;
    private float _timeSinceAnimationStart = 0.0f;
    private bool _animatingScale = false;
    private bool _reverse = false;
    private Vector2 originalScale;
    private Vector2 tempScale;

    private int previousMetalTotal = 0;
    
	// Use this for initialization
	void Start () {
        originalScale = new Vector2( transform.localScale.x,transform.localScale.y);
	}
	
	// Update is called once per frame
	void Update () {
        UpdateScaleAnaimation();

        if (previousMetalTotal != PlayerInventoryManager.Instance.PlayerResources)
            UpdateMetalCount(PlayerInventoryManager.Instance.PlayerResources);
    }

    public void UpdateMetalCount(int metal)
    {
        MetalText.text = "x " + metal;
        PlayScaleAnimation();
    }

    void UpdateScaleAnaimation()
    {
        if (_animatingScale)
        {
            if (_reverse)
            {

                if (Time.time / (_timeSinceAnimationStart + _duration) <= 1.0f)
                {
                    float normalizedTime = Time.time / (_timeSinceAnimationStart + _duration);
                    Vector2 scale = Vector2.Lerp(tempScale, originalScale, normalizedTime);
                    transform.localScale = scale;
                }
                else
                {
                    _reverse = false;
                    _animatingScale = false;
                    transform.localScale = originalScale;
                }
            }
            else
            {
                if ( Time.time / (_timeSinceAnimationStart + _duration) <= 1.0f)
                {
                    float normalizedTime = Time.time / (_timeSinceAnimationStart + _duration);
                    Vector2 scale = Vector2.Lerp(originalScale, tempScale, normalizedTime);
                    transform.localScale = scale;
                }
                else
                {
                    _reverse = true;
                    _timeSinceAnimationStart = Time.time;
                    transform.localScale = tempScale;
                }
            }
        }
    }

    void PlayScaleAnimation()
    {
        _animatingScale = true;
        _reverse = false;
        _timeSinceAnimationStart = Time.time;
        transform.localScale = originalScale;
        tempScale = new Vector2(_scale, _scale);
    }
}
