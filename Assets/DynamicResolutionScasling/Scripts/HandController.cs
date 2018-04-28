using UnityEngine;
using UnityEngine.XR.WSA.Input;

public class HandController : MonoBehaviour
{
	public delegate void TappedEventHandler(TapActionEnum tapAction);
	public TappedEventHandler TappedEvent;

	private float intervalTime = 0.8f;
	private float previousPressedTime = 0.0f;
	private HandednessEnum previousPressedHand = HandednessEnum.Neither;

    public enum HandednessEnum
    {
        Right,
        Left,
        Neither,
    }

    public enum TapActionEnum
    {
        LeftTap,
        RightTap,
        LeftLeftTap,
        RightRightTap,
        LeftRightTap,
        RightLeftTap,
    }

	void Awake()
	{
		InteractionManager.InteractionSourcePressed += FingerPressed;
	}

	void OnDestroy()
	{
		InteractionManager.InteractionSourcePressed -= FingerPressed;
	}

	private void FingerPressed(InteractionSourcePressedEventArgs args)
	{
		Vector3 handPosition;
		if (args.state.sourcePose.TryGetPosition(out handPosition))
		{
			Transform headTransform = Camera.main.transform;
			HandednessEnum pressedHand = GetHandedness(headTransform.forward, handPosition, headTransform.up);
			float pressedTime = Time.time;

			TappedHandler(pressedHand, pressedTime);

			previousPressedHand = pressedHand;
			previousPressedTime = pressedTime;
		}
	}

	private void TappedHandler(HandednessEnum pressedHand, float pressedTime)
	{
		// Debug.Log("diff of pressedTime: " + (pressedTime - previousPressedTime));
		if(pressedTime - previousPressedTime > intervalTime){
			if(pressedHand == HandednessEnum.Left){
				TappedEvent(TapActionEnum.LeftTap);
				Debug.Log("LeftTappedEvent.");
			}
			else if(pressedHand == HandednessEnum.Right)
			{
				TappedEvent(TapActionEnum.RightTap);
				Debug.Log("RightTappedEvent.");
			}
		}
		else
		{
			if(previousPressedHand == HandednessEnum.Left && pressedHand == HandednessEnum.Left){
				TappedEvent(TapActionEnum.LeftLeftTap);
				Debug.Log("LeftLeftTappedEvent.");
			}
			else if(previousPressedHand == HandednessEnum.Right && pressedHand == HandednessEnum.Right)
			{
				TappedEvent(TapActionEnum.RightRightTap);
				Debug.Log("RightRightTappedEvent.");
			}
			else if(previousPressedHand == HandednessEnum.Left && pressedHand == HandednessEnum.Right)
			{
				TappedEvent(TapActionEnum.LeftRightTap);
				Debug.Log("LeftRightTappedEvent.");
			}
			else if(previousPressedHand == HandednessEnum.Right && pressedHand == HandednessEnum.Left)
			{
				TappedEvent(TapActionEnum.RightLeftTap);
				Debug.Log("RightLeftTappedEvent.");
			}
		}
	}

    public static HandednessEnum GetHandedness(Vector3 forward, Vector3 handPos, Vector3 up)
	{
        // Get handedness based on position relative to camera
        Vector3 cross = Vector3.Cross(forward, handPos);
        float direction = Vector3.Dot(cross, up);

        HandednessEnum newHandedness = HandednessEnum.Neither;
        if (direction >= 0f)
		{
            // Favor right-handedness slightly
            newHandedness = HandednessEnum.Right;
        }
		else
        {
            newHandedness = HandednessEnum.Left;
        }

        return newHandedness;
	}
}
