using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.VR;
// using UnityEngine.VR.WSA.Input;
using UnityEngine.XR;
using UnityEngine.XR.WSA.Input;

public class RenderScaleController : MonoBehaviour {

    [SerializeField]
    private TextMesh textMesh;

	private float currentRenderScale = 1.0f;
	private float currentRenderViewportScale = 1.0f;
    private int maxVerticalResolution = 0;
    private int maxHorizontalResolution = 0;

	private HandController handController;
	private string currentTapAction;

	void Start()
	{
		FindScreenResolution();
		Debug.Log("Start Maximum resolution - " + maxHorizontalResolution + " x " + maxVerticalResolution);

		handController = gameObject.AddComponent<HandController>() as HandController;
		handController.TappedEvent += TappedEventForRenderScale;

        if (textMesh == null)
        {
            textMesh = GetComponent<TextMesh>();
        }
	}
	
	void Update()
	{
        string displayString = "EyeTextureResolutionScale = "
							 + XRSettings.eyeTextureResolutionScale + "\n\n"
							 + currentTapAction + "\n";

        if (textMesh != null)
        {
            textMesh.text = displayString;
        }
	}

	void OnDestroy()
	{
		handController.TappedEvent -= TappedEventForRenderScale;
	}

	public void TappedEventForRenderScale(HandController.TapActionEnum tapAction)
	{
		if(tapAction == HandController.TapActionEnum.LeftTap)
		{
			// Debug.Log("Left Tapped.");
			currentTapAction = "LeftTap";
			currentRenderScale -= 0.1f;
			SetRenderScale(currentRenderScale);
			// Debug.Log("Maximum resolution - " + maxHorizontalResolution + " x " + maxVerticalResolution);
			// Debug.Log("current resolution - " + GetCurrentResolution());
		}
		else if(tapAction == HandController.TapActionEnum.RightTap)
		{
			// Debug.Log("Right Tapped.");
			currentTapAction = "RightTap";
			currentRenderScale += 0.1f;
			SetRenderScale(currentRenderScale);
			// Debug.Log("Maximum resolution - " + maxHorizontalResolution + " x " + maxVerticalResolution);
			// Debug.Log("Current resolution - " + GetCurrentResolution());
		}
		else if(tapAction == HandController.TapActionEnum.LeftLeftTap)
		{
			currentTapAction = "LeftLeftTap";
		}
		else if(tapAction == HandController.TapActionEnum.RightRightTap)
		{
			currentTapAction = "RightRightTap";
		}
		else if(tapAction == HandController.TapActionEnum.LeftRightTap)
		{
			currentTapAction = "LeftRightTap";
			currentRenderScale = 1.0f;
			XRSettings.eyeTextureResolutionScale = 1.0f;
		}
		else if(tapAction == HandController.TapActionEnum.RightLeftTap)
		{
			currentTapAction = "RightLeftTap";
			currentRenderScale = 1.0f;
			XRSettings.eyeTextureResolutionScale = 1.0f;
		}
	}

	public void SetRenderScale(float scale)
    {
        float newScale = Mathf.Clamp(scale, 0.0f,  1.0f);
		XRSettings.eyeTextureResolutionScale = newScale;
		currentRenderScale = newScale;
    }

	public void SetRenderViewportScale(float scale)
    {
        float newScale = Mathf.Clamp(scale, 0.0f,  1.0f);
		XRSettings.renderViewportScale = newScale;
		currentRenderViewportScale = newScale;
    }

    public void FindScreenResolution()
    {
        var screenPoint = Camera.main.ViewportToScreenPoint(new Vector3(1.0f, 1.0f, Camera.main.nearClipPlane));
        maxHorizontalResolution = Mathf.RoundToInt(screenPoint.x);
        maxVerticalResolution = Mathf.RoundToInt(screenPoint.y);
    }

	public string GetCurrentResolution()
	{
		int currentHorizontalResolution = Mathf.RoundToInt(currentRenderViewportScale * maxHorizontalResolution);
		int currentVerticalResolution = Mathf.RoundToInt(currentRenderViewportScale * maxVerticalResolution);
		string text = string.Format("w:{0} h:{1}", currentHorizontalResolution, currentVerticalResolution);
		return text;
	}
}
