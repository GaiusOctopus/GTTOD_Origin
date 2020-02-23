using System.Collections;
using UnityEngine;

namespace TacticalAI
{
	public class AnimationScript : MonoBehaviour
	{
		public BaseScript myBaseScript;

		public Transform myAIBodyTransform;

		public GunScript gunScript;

		public Animator animator;

		private Transform myTransform;

		private float currentVelocityRatio;

		private float animationDampTime = 0.1f;

		private NavmeshInterface navi;

		public Vector3 bodyOffset;

		public float minDistToCrouch = 1f;

		public float maxMovementSpeed = -1f;

		public float animatorSpeed = 1f;

		public float meleeAnimationSpeed = 1f;

		private int currentAngleHash;

		private int engagingHash;

		private int crouchingHash;

		private int reloadingHash;

		private int meleeHash;

		private int fireHash;

		private int forwardsMoveHash;

		private int sidewaysMoveHash;

		private int sprintingHash;

		private int dodgeRightHash;

		private int dodgeLeftHash;

		private int staggerHash;

		private int grenadeHash;

		private int coverHash;

		private int centerHash;

		private int rightHash;

		private int leftHash;

		public float maxAngleDeviation = 10f;

		private Quaternion currRotRequired;

		public bool useCustomRotation;

		private Vector3 directionToFace;

		private float myAngle;

		[Range(0f, 90f)]
		public float minAngleToRotateBase = 65f;

		private Quaternion newRotation;

		public float turnSpeed = 4f;

		public float meleeAnimationLength = 3f;

		private bool sprinting;

		private bool useAdvancedCover;

		private int coverFaceDirection;

		private Vector3 coverStandDirection;

		public float coverTransitionTime = 0.5f;

		private float timeToAimRotate;

		private bool enteredCover;

		private bool startedFireCycle;

		private bool isPlaying = true;

		public bool currentlyRotating = true;

		private bool setHashes;

		public bool onLink;

		public float leapAnimationLength = 1.5f;

		public float vaultAnimationLength = 1.5f;

		public float leapMaxAngle = 15f;

		public float vaultMaxAngle = 10f;

		public Vector3 lerpTargPos;

		private float lerpSpeed = 5f;

		private float lerpAmt;

		private Vector3 startPos;

		public bool isVLerping;

		private void Awake()
		{
			SetHashes();
		}

		private void Start()
		{
			if ((bool)myAIBodyTransform)
			{
				bodyOffset = myAIBodyTransform.localPosition;
				bodyOffset.x *= base.transform.localScale.x;
				bodyOffset.y *= base.transform.localScale.y;
				bodyOffset.z *= base.transform.localScale.z;
				myAIBodyTransform.parent = null;
			}
			else
			{
				Debug.LogWarning("No transform set for 'myAIBodyTransform'.  Please assign a transform in the inspector!");
				base.enabled = false;
			}
			navi = myBaseScript.GetAgent();
			minDistToCrouch *= minDistToCrouch;
			myTransform = base.transform;
			lerpSpeed = myBaseScript.runSpeed;
			if (!myBaseScript)
			{
				Debug.LogWarning("No Base Script found!  Please add one in the inspector!");
				base.enabled = false;
			}
			else if (maxMovementSpeed < 0f)
			{
				maxMovementSpeed = myBaseScript.runSpeed;
			}
			if (!animator)
			{
				Debug.LogWarning("No animator component found!  Please add one in the inspector!");
				base.enabled = false;
			}
			else
			{
				animator.speed = animatorSpeed;
			}
		}

		private void LateUpdate()
		{
			if (!onLink && !myBaseScript.isStaggered && !enteredCover)
			{
				myAIBodyTransform.position = myTransform.position + bodyOffset;
			}
			else if (isVLerping)
			{
				LerpPos();
			}
			AnimateAI();
			RotateAI();
		}

		private void AnimateAI()
		{
			if (!onLink && !myBaseScript.isStaggered)
			{
				float dampTime = 0.01f;
				if (!myBaseScript.inCover)
				{
					dampTime = animationDampTime;
				}
				animator.SetFloat(forwardsMoveHash, Vector3.Dot(myAIBodyTransform.forward, navi.GetDesiredVelocity()) / maxMovementSpeed, dampTime, Time.deltaTime);
				animator.SetFloat(sidewaysMoveHash, Vector3.Dot(myAIBodyTransform.right, navi.GetDesiredVelocity()) / maxMovementSpeed, dampTime, Time.deltaTime);
			}
			Cover();
		}

		private void Cover()
		{
			if (!useAdvancedCover)
			{
				if (myBaseScript.inCover && (!gunScript || !gunScript.IsFiring() || !myBaseScript.shouldFireFromCover) && Vector3.SqrMagnitude(myTransform.position - myBaseScript.GetCurrentCoverNodePos()) < minDistToCrouch && (double)currentVelocityRatio < 0.3)
				{
					animator.SetBool(crouchingHash, value: true);
				}
				else
				{
					animator.SetBool(crouchingHash, value: false);
				}
			}
			else if (myBaseScript.inCover)
			{
				if (!enteredCover && navi.ReachedDestination())
				{
					if (!gunScript || !gunScript.IsFiring() || !myBaseScript.shouldFireFromCover)
					{
						animator.SetBool(coverHash, value: true);
						enteredCover = true;
					}
					useCustomRotation = true;
					directionToFace = coverStandDirection;
				}
				if (!enteredCover)
				{
					return;
				}
				if (!gunScript || !gunScript.IsFiring() || !myBaseScript.shouldFireFromCover)
				{
					animator.SetBool(centerHash, value: false);
					animator.SetBool(leftHash, value: false);
					animator.SetBool(rightHash, value: false);
					startedFireCycle = false;
				}
				else if (gunScript.IsFiring())
				{
					if (!startedFireCycle)
					{
						timeToAimRotate = coverTransitionTime;
						startedFireCycle = true;
					}
					timeToAimRotate -= Time.deltaTime;
					_ = timeToAimRotate;
					_ = 0f;
					if (coverFaceDirection == 0)
					{
						animator.SetBool(centerHash, value: true);
					}
					if (coverFaceDirection == 1)
					{
						animator.SetBool(rightHash, value: true);
					}
					if (coverFaceDirection == -1)
					{
						animator.SetBool(leftHash, value: true);
					}
				}
			}
			else
			{
				animator.SetBool(coverHash, value: false);
			}
		}

		public void StartAdvancedCover(Vector3 standDir, int faceDir)
		{
			useAdvancedCover = true;
			coverStandDirection = standDir;
			coverFaceDirection = faceDir;
			enteredCover = false;
			startedFireCycle = false;
		}

		public void EndAdvancedCover()
		{
			useAdvancedCover = false;
			useCustomRotation = false;
			enteredCover = false;
		}

		public void StartSprinting()
		{
			if (sprinting)
			{
				return;
			}
			sprinting = true;
			for (int i = 0; i < animator.parameters.Length; i++)
			{
				if (animator.parameters[i].name == "Sprinting")
				{
					animator.SetBool(sprintingHash, value: true);
				}
			}
		}

		public void StopSprinting()
		{
			if (!sprinting)
			{
				return;
			}
			sprinting = false;
			for (int i = 0; i < animator.parameters.Length; i++)
			{
				if (animator.parameters[i].name == "Sprinting")
				{
					animator.SetBool(sprintingHash, value: false);
				}
			}
		}

		public bool isSprinting()
		{
			return sprinting;
		}

		public void PlayReloadAnimation()
		{
			animator.SetTrigger(reloadingHash);
		}

		public void PlayFiringAnimation()
		{
			animator.SetTrigger(fireHash);
		}

		public void PlayDodgingAnimation(bool dodgeRight)
		{
			if (dodgeRight)
			{
				for (int i = 0; i < animator.parameters.Length; i++)
				{
					if (animator.parameters[i].name == "DodgeRight")
					{
						animator.SetTrigger(dodgeRightHash);
					}
				}
				return;
			}
			for (int j = 0; j < animator.parameters.Length; j++)
			{
				if (animator.parameters[j].name == "DodgeLeft")
				{
					animator.SetTrigger(dodgeLeftHash);
				}
			}
		}

		public void PlayStaggerAnimation()
		{
			for (int i = 0; i < animator.parameters.Length; i++)
			{
				if (animator.parameters[i].name == "Stagger")
				{
					animator.SetTrigger(staggerHash);
					animator.SetFloat(forwardsMoveHash, 0f, 0f, Time.deltaTime);
					animator.SetFloat(sidewaysMoveHash, 0f, 0f, Time.deltaTime);
				}
			}
		}

		public void PlayDeathAnimation()
		{
			for (int i = 0; i < animator.parameters.Length; i++)
			{
				if (animator.parameters[i].name == "Death")
				{
					animator.SetTrigger(staggerHash);
					animator.SetFloat(forwardsMoveHash, 0f, 0f, Time.deltaTime);
					animator.SetFloat(sidewaysMoveHash, 0f, 0f, Time.deltaTime);
				}
			}
		}

		public void PlayGrenadeAnimation()
		{
			for (int i = 0; i < animator.parameters.Length; i++)
			{
				if (animator.parameters[i].name == "Grenade")
				{
					animator.SetTrigger(grenadeHash);
					animator.SetFloat(forwardsMoveHash, 0f, 0f, Time.deltaTime);
					animator.SetFloat(sidewaysMoveHash, 0f, 0f, Time.deltaTime);
				}
			}
		}

		public void StartRot()
		{
			currentlyRotating = true;
		}

		public void StopRot()
		{
			currentlyRotating = false;
		}

		public IEnumerator StartMelee()
		{
			directionToFace = -(myAIBodyTransform.position - myBaseScript.targetTransform.position);
			useCustomRotation = true;
			directionToFace.y = 0f;
			while (isPlaying && (bool)myAIBodyTransform && (bool)myBaseScript.targetTransform && Vector3.Angle(directionToFace, myAIBodyTransform.forward) > maxAngleDeviation)
			{
				directionToFace = -(myAIBodyTransform.position - myBaseScript.targetTransform.position);
				directionToFace.y = 0f;
				Debug.DrawRay(myTransform.position, myTransform.forward * 100f, Color.magenta);
				Debug.DrawRay(myTransform.position, directionToFace * 100f, Color.blue);
				yield return null;
			}
			if (isPlaying && (bool)myAIBodyTransform)
			{
				animator.SetTrigger(meleeHash);
				yield return new WaitForSeconds(meleeAnimationLength);
			}
			useCustomRotation = false;
			myBaseScript.StopMelee();
		}

		private void OnApplicationQuit()
		{
			isPlaying = false;
		}

		public IEnumerator WaitForAnimationToFinish()
		{
			while (animator.IsInTransition(1))
			{
				yield return null;
			}
			while (!animator.IsInTransition(1))
			{
				yield return null;
			}
			while (animator.IsInTransition(1))
			{
				yield return null;
			}
		}

		public IEnumerator DynamicObjectAnimation(string transitionName, Vector3 dir, DynamicObject dynamicObjectScript, float timeToWait)
		{
			directionToFace = dir;
			useCustomRotation = true;
			directionToFace.y = 0f;
			while (Vector3.Angle(directionToFace, myAIBodyTransform.forward) > maxAngleDeviation)
			{
				Debug.DrawRay(myTransform.position, myTransform.forward * 100f, Color.magenta);
				Debug.DrawRay(myTransform.position, directionToFace * 100f, Color.blue);
				yield return null;
			}
			currentlyRotating = false;
			yield return new WaitForSeconds(0.25f);
			bool shouldReactivate = false;
			dynamicObjectScript.AffectDynamicObject();
			for (int i = 0; i < animator.parameters.Length; i++)
			{
				if (animator.parameters[i].name == transitionName)
				{
					animator.SetTrigger(Animator.StringToHash(transitionName));
					yield return new WaitForSeconds(timeToWait);
					break;
				}
			}
			_ = shouldReactivate;
			currentlyRotating = true;
			dynamicObjectScript.EndDynamicObjectUsage();
			useCustomRotation = false;
		}

		private void RotateAI()
		{
			if (!currentlyRotating)
			{
				return;
			}
			if (useCustomRotation)
			{
				newRotation = Quaternion.LookRotation(directionToFace);
				newRotation.eulerAngles = new Vector3(0f, newRotation.eulerAngles.y, 0f);
				myAIBodyTransform.rotation = Quaternion.Slerp(myAIBodyTransform.rotation, newRotation, turnSpeed * Time.deltaTime);
				animator.SetFloat(forwardsMoveHash, 0f, animationDampTime, Time.deltaTime);
				animator.SetFloat(sidewaysMoveHash, 0f, animationDampTime, Time.deltaTime);
			}
			else if (myBaseScript.IsEnaging() && (bool)myBaseScript.targetTransform && !sprinting)
			{
				myAngle = Vector3.Angle(myTransform.forward, myAIBodyTransform.forward);
				if (Vector3.Angle(-myAIBodyTransform.right, myTransform.forward) > 90f)
				{
					myAngle = 0f - myAngle;
				}
				float num = Vector3.Angle(myTransform.forward, myBaseScript.targetTransform.position - myAIBodyTransform.position);
				if (num > minAngleToRotateBase && num < 180f - minAngleToRotateBase)
				{
					newRotation = Quaternion.LookRotation(myBaseScript.targetTransform.position - myAIBodyTransform.position);
				}
				else if (num < 90f)
				{
					newRotation = Quaternion.LookRotation(myTransform.forward);
					animator.SetFloat(forwardsMoveHash, Vector3.Magnitude(navi.GetDesiredVelocity()) / maxMovementSpeed, animationDampTime, Time.deltaTime);
					animator.SetFloat(sidewaysMoveHash, 0f, animationDampTime, Time.deltaTime);
				}
				else
				{
					newRotation = Quaternion.LookRotation(-myTransform.forward);
					animator.SetFloat(forwardsMoveHash, (0f - Vector3.Magnitude(navi.GetDesiredVelocity())) / maxMovementSpeed, animationDampTime, Time.deltaTime);
					animator.SetFloat(sidewaysMoveHash, 0f, animationDampTime, Time.deltaTime);
				}
				newRotation.eulerAngles = new Vector3(0f, newRotation.eulerAngles.y, 0f);
				if (!ControllerScript.pMode || Quaternion.Angle(myAIBodyTransform.rotation, newRotation) > 10f)
				{
					myAIBodyTransform.rotation = Quaternion.Slerp(myAIBodyTransform.rotation, newRotation, Time.deltaTime * turnSpeed);
				}
			}
			else
			{
				myAngle = 0f;
				newRotation = Quaternion.LookRotation(myTransform.forward);
				newRotation.eulerAngles = new Vector3(0f, newRotation.eulerAngles.y, 0f);
				if (!ControllerScript.pMode || Quaternion.Angle(myAIBodyTransform.rotation, newRotation) > 10f)
				{
					myAIBodyTransform.rotation = Quaternion.Slerp(myAIBodyTransform.rotation, newRotation, turnSpeed * Time.deltaTime);
				}
			}
		}

		private void SetHashes()
		{
			crouchingHash = Animator.StringToHash("Crouching");
			engagingHash = Animator.StringToHash("Engaging");
			reloadingHash = Animator.StringToHash("Reloading");
			meleeHash = Animator.StringToHash("Melee");
			fireHash = Animator.StringToHash("Fire");
			sidewaysMoveHash = Animator.StringToHash("Horizontal");
			forwardsMoveHash = Animator.StringToHash("Forwards");
			sprintingHash = Animator.StringToHash("Sprinting");
			dodgeRightHash = Animator.StringToHash("DodgeRight");
			dodgeLeftHash = Animator.StringToHash("DodgeLeft");
			staggerHash = Animator.StringToHash("Stagger");
			grenadeHash = Animator.StringToHash("Grenade");
			coverHash = Animator.StringToHash("Cover");
			centerHash = Animator.StringToHash("CoverCenter");
			rightHash = Animator.StringToHash("CoverRight");
			leftHash = Animator.StringToHash("CoverLeft");
			setHashes = true;
		}

		public void SetEngaging()
		{
			if (!setHashes)
			{
				SetHashes();
			}
			animator.SetBool(engagingHash, value: true);
		}

		public void SetDisengage()
		{
			if ((bool)animator)
			{
				if (!setHashes)
				{
					SetHashes();
				}
				animator.SetBool(engagingHash, value: false);
			}
		}

		public void LerpPos()
		{
			if (lerpAmt < 1f)
			{
				lerpAmt += lerpSpeed / Vector3.Distance(lerpTargPos, startPos) * Time.deltaTime;
				myAIBodyTransform.position = Vector3.Lerp(startPos, lerpTargPos, lerpAmt);
				base.transform.position = myAIBodyTransform.position;
			}
			else
			{
				isVLerping = false;
				lerpAmt = 0f;
				animator.SetFloat(forwardsMoveHash, 0f, animationDampTime, Time.deltaTime);
				animator.SetFloat(sidewaysMoveHash, 0f, animationDampTime, Time.deltaTime);
			}
		}

		public void Parkour(Vector3 dir, string transitionName, Vector3 lerPos)
		{
			onLink = true;
			lerpAmt = 0f;
			float animTime = 3f;
			startPos = myTransform.position;
			lerpTargPos = lerPos;
			lerpTargPos.y = myTransform.position.y;
			float maxAng = 15f;
			if (!(transitionName == "Vault"))
			{
				if (transitionName == "Leap")
				{
					animTime = leapAnimationLength;
					maxAng = leapMaxAngle;
				}
			}
			else
			{
				animTime = vaultAnimationLength;
				maxAng = vaultMaxAngle;
			}
			StartCoroutine(ParkourAnimate(transitionName, dir, animTime, maxAng));
		}

		public IEnumerator ParkourAnimate(string transitionName, Vector3 dir, float animTime, float maxAng)
		{
			lerpAmt = 0f;
			isVLerping = true;
			yield return new WaitForSeconds(0.1f);
			directionToFace = dir;
			directionToFace.y = 0f;
			useCustomRotation = true;
			while (Vector3.Angle(directionToFace, myAIBodyTransform.forward) > maxAng || isVLerping || gunScript.IsFiring() || gunScript.IsReloading())
			{
				Debug.DrawRay(myTransform.position, myAIBodyTransform.forward * 100f, Color.yellow);
				Debug.DrawRay(myTransform.position, directionToFace * 100f, Color.cyan);
				yield return null;
			}
			currentlyRotating = false;
			for (int i = 0; i < animator.parameters.Length; i++)
			{
				if (animator.parameters[i].name == transitionName)
				{
					animator.SetTrigger(Animator.StringToHash(transitionName));
					yield return new WaitForSeconds(animTime);
					break;
				}
			}
			currentlyRotating = true;
			useCustomRotation = false;
			onLink = false;
		}
	}
}
