using System.Collections;
using UnityEngine;

namespace TacticalAI
{
	public class GunScript : MonoBehaviour
	{
		public BaseScript myAIBaseScript;

		public AnimationScript animationScript;

		public SoundScript soundScript;

		public AudioSource audioSource;

		private int[] enemyTeams;

		public GameObject bulletObject;

		public AudioClip bulletSound;

		[Range(0f, 1f)]
		public float bulletSoundVolume = 1f;

		public Transform bulletSpawn;

		public GameObject muzzleFlash;

		public Transform muzzleFlashSpawn;

		public float flashDestroyTime = 0.3f;

		private bool canCurrentlyFire = true;

		public int pelletsPerShot = 1;

		public bool isRocketLauncher;

		public GameObject secondaryFireObject;

		[Range(0f, 1f)]
		public float oddsToSecondaryFire = 0.1f;

		public float minDistForSecondaryFire = 10f;

		public float maxDistForSecondaryFire = 50f;

		private bool canFireGrenadeAgain;

		private Vector3 lastPosTargetSeen = Vector3.zero;

		public bool needsLOSForSecondaryFire;

		private bool canThrowGrenade = true;

		public float minTimeBetweenSecondaryFire = 4f;

		public float minPauseTime = 1f;

		public float randomPauseTimeAdd = 2f;

		public int minRoundsPerVolley = 1;

		public int maxRoundsPerVolley = 10;

		public int minBurstsPerVolley;

		public int maxBurstsPerVolley;

		public int currentRoundsPerVolley;

		public float rateOfFire = 2f;

		private float timeBetweenBursts;

		public float burstFireRate = 12f;

		public int shotsPerBurst = 1;

		private float timeBetweenBurstBullets;

		public int bulletsUntilReload = 60;

		public AudioClip reloadSound;

		[Range(0f, 1f)]
		public float reloadSoundVolume = 1f;

		private bool isReloading;

		private int currentBulletsUntilReload;

		public float reloadTime = 2f;

		public float inaccuracy = 1f;

		[Range(0f, 90f)]
		public float maxFiringAngle = 10f;

		[Range(0f, 90f)]
		public float maxSecondaryFireAngle = 40f;

		private Quaternion fireRotation;

		private Transform targetTransform;

		private Transform LOSTargetTransform;

		private LayerMask LOSLayermask;

		public float timeBetweenLOSChecks = 2f;

		private bool aware;

		private bool isFiring;

		private bool isWaiting;

		public float distInFrontOfTargetAllowedForCover = 3f;

		public float coverTransitionTime = 0.45f;

		private float rayDist;

		public float soundRadius = 7f;

		public float minimumDistToFireGun;

		public float maximumDistToFireGun = 9999f;

		private float timer = 30f;

		public bool checkForFriendlyFire;

		public string friendlyTag;

		public Transform grenadeSpawn;

		private bool locatedNewGrenadeTargetYet;

		private void Awake()
		{
			LOSLayermask = ControllerScript.currentController.GetLayerMask();
			if (!audioSource && (bool)bulletSpawn && (bool)bulletSpawn.gameObject.GetComponent<AudioSource>())
			{
				audioSource = bulletSpawn.gameObject.GetComponent<AudioSource>();
			}
			if ((bool)base.gameObject.GetComponent<SoundScript>())
			{
				soundScript = base.gameObject.GetComponent<SoundScript>();
			}
			if (!grenadeSpawn)
			{
				grenadeSpawn = bulletSpawn;
			}
			isFiring = false;
			isWaiting = false;
			currentBulletsUntilReload = bulletsUntilReload;
			timeBetweenBurstBullets = 1f / burstFireRate;
			timeBetweenBursts = 1f / rateOfFire;
			minBurstsPerVolley = minRoundsPerVolley / shotsPerBurst;
			maxBurstsPerVolley = maxRoundsPerVolley / shotsPerBurst;
			maxFiringAngle /= 2f;
			maxSecondaryFireAngle /= 2f;
			minimumDistToFireGun *= minimumDistToFireGun;
			maximumDistToFireGun *= maximumDistToFireGun;
		}

		private void Start()
		{
			enemyTeams = myAIBaseScript.GetEnemyTeamIDs();
		}

		private void LateUpdate()
		{
			if (aware)
			{
				if (!isFiring && !isWaiting && (bool)bulletObject)
				{
					StartCoroutine(BulletFiringCycle());
				}
				else if (!bulletObject)
				{
					Debug.LogWarning("Can't fire because there is no bullet object selected!");
				}
			}
			timer -= 1f;
		}

		private IEnumerator BulletFiringCycle()
		{
			isFiring = true;
			if (myAIBaseScript.inCover)
			{
				yield return new WaitForSeconds(coverTransitionTime * 1.5f);
			}
			if (myAIBaseScript.IsEnaging() && !myAIBaseScript.isMeleeing && !myAIBaseScript.inParkour)
			{
				if ((bool)LOSTargetTransform && !animationScript.isSprinting())
				{
					if (!Physics.Linecast(bulletSpawn.position, LOSTargetTransform.position, LOSLayermask) || !locatedNewGrenadeTargetYet)
					{
						lastPosTargetSeen = targetTransform.position;
						locatedNewGrenadeTargetYet = true;
						canFireGrenadeAgain = true;
						FireOneGrenade();
						canFireGrenadeAgain = true;
					}
					else if (!needsLOSForSecondaryFire)
					{
						lastPosTargetSeen = targetTransform.position;
						if (canFireGrenadeAgain)
						{
							FireOneGrenade();
						}
						canFireGrenadeAgain = true;
					}
				}
				if (soundRadius > 0f)
				{
					ControllerScript.currentController.CreateSound(bulletSpawn.position, soundRadius, enemyTeams);
				}
				if (animationScript.currentlyRotating)
				{
					yield return StartCoroutine(Fire());
				}
			}
			isWaiting = true;
			isFiring = false;
			if (currentBulletsUntilReload > 0 && reloadTime > 0f)
			{
				yield return new WaitForSeconds(minPauseTime + Random.value * randomPauseTimeAdd);
			}
			else
			{
				isReloading = true;
				if ((bool)reloadSound)
				{
					audioSource.volume = reloadSoundVolume;
					audioSource.PlayOneShot(reloadSound);
				}
				if ((bool)animationScript)
				{
					animationScript.PlayReloadAnimation();
				}
				if ((bool)soundScript)
				{
					soundScript.PlayReloadAudio();
				}
				yield return new WaitForSeconds(reloadTime);
				currentBulletsUntilReload = bulletsUntilReload;
				isReloading = false;
				yield return new WaitForSeconds(minPauseTime * Random.value);
			}
			isWaiting = false;
		}

		private IEnumerator Fire()
		{
			float num = Vector3.SqrMagnitude(bulletSpawn.position - LOSTargetTransform.position);
			if (minimumDistToFireGun <= num && maximumDistToFireGun >= num)
			{
				currentRoundsPerVolley = Mathf.Min(Random.Range(minBurstsPerVolley, maxBurstsPerVolley), currentBulletsUntilReload);
			}
			while (currentRoundsPerVolley > 0 && base.enabled && !animationScript.isSprinting() && !myAIBaseScript.inParkour && (bool)LOSTargetTransform && canCurrentlyFire)
			{
				rayDist = Mathf.Max(1E-05f, Vector3.Distance(bulletSpawn.position, LOSTargetTransform.position) - distInFrontOfTargetAllowedForCover);
				if (rayDist == 0f || !Physics.Raycast(bulletSpawn.position, LOSTargetTransform.position - bulletSpawn.position, rayDist, LOSLayermask))
				{
					bool flag = true;
					if (checkForFriendlyFire && Physics.Raycast(bulletSpawn.position, targetTransform.position - bulletSpawn.position, out RaycastHit hitInfo, Vector3.Distance(bulletSpawn.position, LOSTargetTransform.position), LOSLayermask) && hitInfo.transform.tag == friendlyTag)
					{
						flag = false;
					}
					if (flag)
					{
						for (int i = 0; i < shotsPerBurst; i++)
						{
							if (i < shotsPerBurst - 1)
							{
								yield return new WaitForSeconds(timeBetweenBurstBullets);
							}
							currentBulletsUntilReload--;
							FireOneShot();
						}
					}
				}
				currentRoundsPerVolley--;
				if (currentRoundsPerVolley > 0)
				{
					yield return new WaitForSeconds(timeBetweenBursts);
				}
			}
		}

		private void FireOneShot()
		{
			if (!targetTransform || myAIBaseScript.inParkour)
			{
				return;
			}
			bool flag = Vector3.Angle(bulletSpawn.forward, targetTransform.position - bulletSpawn.position) < maxFiringAngle;
			for (int i = 0; i < pelletsPerShot; i++)
			{
				if (flag)
				{
					fireRotation.SetLookRotation(targetTransform.position - bulletSpawn.position);
				}
				else
				{
					fireRotation = Quaternion.LookRotation(bulletSpawn.forward);
				}
				fireRotation *= Quaternion.Euler(Random.Range(0f - inaccuracy, inaccuracy), Random.Range(0f - inaccuracy, inaccuracy), 0f);
				Object.Instantiate(bulletObject, bulletSpawn.position, fireRotation);
			}
			if ((bool)bulletSound)
			{
				audioSource.volume = bulletSoundVolume;
				audioSource.PlayOneShot(bulletSound);
			}
			if ((bool)animationScript)
			{
				animationScript.PlayFiringAnimation();
			}
			if ((bool)muzzleFlash)
			{
				GameObject gameObject = Object.Instantiate(muzzleFlash, muzzleFlashSpawn.position, muzzleFlashSpawn.rotation);
				gameObject.transform.parent = muzzleFlashSpawn;
				Object.Destroy(gameObject, flashDestroyTime);
			}
		}

		private void FireOneGrenade()
		{
			if (Random.value < oddsToSecondaryFire && canThrowGrenade && Vector3.Angle(bulletSpawn.forward, lastPosTargetSeen - bulletSpawn.position) < maxSecondaryFireAngle && (bool)secondaryFireObject)
			{
				float num = Vector3.Distance(lastPosTargetSeen, bulletSpawn.position);
				if (num < maxDistForSecondaryFire && num > minDistForSecondaryFire && timer < 0f)
				{
					canFireGrenadeAgain = false;
					StartCoroutine(SetTimeUntilNextGrenade());
					myAIBaseScript.ThrowGrenade(secondaryFireObject, lastPosTargetSeen, grenadeSpawn);
				}
			}
		}

		public void EndEngage()
		{
			targetTransform = null;
			aware = false;
		}

		public void AssignTarget(Transform newTarget, Transform newLOSTarget)
		{
			targetTransform = newTarget;
			LOSTargetTransform = newLOSTarget;
			aware = true;
		}

		public void SetCanCurrentlyFire(bool b)
		{
			canCurrentlyFire = b;
		}

		public void SetAware()
		{
			aware = true;
		}

		public void SetEnemyBaseScript(BaseScript x)
		{
			myAIBaseScript = x;
		}

		public void ChangeTargets(Transform newl, Transform newt)
		{
			LOSTargetTransform = newl;
			targetTransform = newt;
		}

		private IEnumerator SetTimeUntilNextGrenade()
		{
			canThrowGrenade = false;
			yield return new WaitForSeconds(minTimeBetweenSecondaryFire);
			canThrowGrenade = true;
		}

		public bool IsFiring()
		{
			return isFiring;
		}

		public bool IsReloading()
		{
			return isReloading;
		}
	}
}
