using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class SuperHotTime : MonoBehaviour
{
	public bool Active;

	private float MinimumTime = 0.2f;

	private float CurrentTime = 1f;

	private ac_CharacterController CharacterController;

	private GameManager GM;

	private MenuScript Menu;

	private PostProcessVolume PostProcessor;

	private ColorGrading ColorGradingProcessing;

	private void Start()
	{
		GM = GameManager.GM;
		Menu = GM.GetComponent<MenuScript>();
		CharacterController = GM.Player.GetComponent<ac_CharacterController>();
		PostProcessor = GetComponent<TimeSlow>().PostProcessor;
		PostProcessor.profile.TryGetSettings(out ColorGradingProcessing);
	}

	private void Update()
	{
		if (Active && !Menu.inMenu)
		{
			Time.timeScale = CurrentTime;
			Time.fixedDeltaTime = Time.timeScale * 0.02f;
			if (KeyBindingManager.GetKey(KeyAction.Forward) || KeyBindingManager.GetKey(KeyAction.Backward) || KeyBindingManager.GetKey(KeyAction.Left) || KeyBindingManager.GetKey(KeyAction.Right) || Input.GetAxis("Vertical") < 0f || Input.GetAxis("Horizontal") < 0f)
			{
				if (CurrentTime < 1f)
				{
					CurrentTime += Time.unscaledDeltaTime;
				}
				else
				{
					CurrentTime = 1f;
				}
			}
			else if (CurrentTime > MinimumTime)
			{
				CurrentTime -= Time.unscaledDeltaTime;
			}
			else
			{
				CurrentTime = MinimumTime;
			}
			if ((KeyBindingManager.GetKey(KeyAction.Fire) || Input.GetAxis("RightTrigger") == 1f || KeyBindingManager.GetKey(KeyAction.Jump)) && CurrentTime < 0.35f)
			{
				CurrentTime = 0.35f;
			}
			if (CharacterController.inAir)
			{
				MinimumTime = 0.1f;
			}
			else
			{
				MinimumTime = 0.025f;
			}
		}
		else
		{
			CurrentTime = 1f;
		}
	}
}
