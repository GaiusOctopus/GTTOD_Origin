using UnityEngine;

public class GUIControls : MonoBehaviour
{
	private WarriorAnimationDemo warriorAnimationDemo;

	private bool blockGui;

	private bool ledgeGui;

	private void Start()
	{
		warriorAnimationDemo = GetComponent<WarriorAnimationDemo>();
	}

	private void OnGUI()
	{
		if (!warriorAnimationDemo.dead && (warriorAnimationDemo.warrior == Warrior.Mage || warriorAnimationDemo.warrior == Warrior.Ninja || warriorAnimationDemo.warrior == Warrior.Knight || warriorAnimationDemo.warrior == Warrior.Archer || warriorAnimationDemo.warrior == Warrior.TwoHanded || warriorAnimationDemo.warrior == Warrior.Swordsman || warriorAnimationDemo.warrior == Warrior.Spearman || warriorAnimationDemo.warrior == Warrior.Hammer || warriorAnimationDemo.warrior == Warrior.Crossbow))
		{
			if (!warriorAnimationDemo.dead && (warriorAnimationDemo.weaponSheathed & !warriorAnimationDemo.isSitting))
			{
				if (GUI.Button(new Rect(30f, 310f, 100f, 30f), "Unsheath Weapon"))
				{
					warriorAnimationDemo.UnSheathWeapon();
				}
				if (warriorAnimationDemo.warrior == Warrior.Ninja && GUI.Button(new Rect(30f, 350f, 100f, 30f), "Sit"))
				{
					warriorAnimationDemo.isStunned = true;
					warriorAnimationDemo.isSitting = true;
					warriorAnimationDemo.animator.SetTrigger("Idle-Relax-ToSitTrigger");
				}
			}
			if (warriorAnimationDemo.isSitting && GUI.Button(new Rect(30f, 350f, 100f, 30f), "Stand"))
			{
				warriorAnimationDemo.animator.SetTrigger("Idle-Relax-FromSitTrigger");
				StartCoroutine(warriorAnimationDemo._LockMovementAndAttack(1.2f));
				warriorAnimationDemo.isSitting = false;
			}
		}
		if (!warriorAnimationDemo.dead && !warriorAnimationDemo.weaponSheathed)
		{
			if (warriorAnimationDemo.warrior == Warrior.Ninja)
			{
				if (!warriorAnimationDemo.isBlocking && !blockGui)
				{
					ledgeGui = GUI.Toggle(new Rect(245f, 60f, 100f, 30f), ledgeGui, "Ledge Jump");
					warriorAnimationDemo.ledgeGui = ledgeGui;
				}
				if (warriorAnimationDemo.ledge)
				{
					if (GUI.Button(new Rect(245f, 90f, 100f, 30f), "Ledge Drop"))
					{
						warriorAnimationDemo.animator.SetTrigger("Ledge-Drop");
						warriorAnimationDemo.ledge = false;
						warriorAnimationDemo.animator.SetBool("Ledge-Catch", value: false);
					}
					if (GUI.Button(new Rect(245f, 20f, 100f, 30f), "Ledge Climb"))
					{
						warriorAnimationDemo.animator.SetTrigger("Ledge-Climb-Trigger");
						warriorAnimationDemo.ledge = false;
						warriorAnimationDemo.animator.SetBool("Ledge-Catch", value: false);
					}
				}
			}
			if (!warriorAnimationDemo.ledge)
			{
				blockGui = GUI.Toggle(new Rect(25f, 215f, 50f, 30f), blockGui, "Block");
				warriorAnimationDemo.blockGui = blockGui;
				if (warriorAnimationDemo.warrior == Warrior.Archer)
				{
					bool flag = GUI.Toggle(new Rect(80f, 215f, 50f, 30f), warriorAnimationDemo.isAiming, "Aiming");
					if (flag != warriorAnimationDemo.isAiming)
					{
						if (flag)
						{
							warriorAnimationDemo.animator.SetBool("Aiming", value: true);
							warriorAnimationDemo.animator.SetTrigger("AimTrigger");
							StartCoroutine(warriorAnimationDemo._ArcherArrowOn(0.2f));
							StartCoroutine(warriorAnimationDemo._SetLayerWeight(1f));
						}
						else
						{
							warriorAnimationDemo.animator.SetBool("Aiming", value: false);
							StartCoroutine(warriorAnimationDemo._SetLayerWeight(0f));
						}
						warriorAnimationDemo.isAiming = flag;
					}
				}
			}
			if (!warriorAnimationDemo.ledge)
			{
				if (!warriorAnimationDemo.isBlocking)
				{
					if (!blockGui)
					{
						warriorAnimationDemo.animator.SetBool("Block", value: false);
					}
					else
					{
						warriorAnimationDemo.animator.SetBool("Block", value: true);
						warriorAnimationDemo.animator.SetFloat("Input X", 0f);
						warriorAnimationDemo.animator.SetFloat("Input Z", 0f);
						warriorAnimationDemo.newVelocity = new Vector3(0f, 0f, 0f);
					}
				}
				if (blockGui)
				{
					if (GUI.Button(new Rect(30f, 240f, 100f, 30f), "BlockHitReact"))
					{
						StartCoroutine(warriorAnimationDemo._BlockHitReact());
					}
					if (GUI.Button(new Rect(30f, 275f, 100f, 30f), "BlockBreak"))
					{
						StartCoroutine(warriorAnimationDemo._BlockBreak());
					}
				}
				else if (!warriorAnimationDemo.inBlock && !warriorAnimationDemo.inBlock && GUI.Button(new Rect(30f, 240f, 100f, 30f), "Hit React"))
				{
					StartCoroutine(warriorAnimationDemo._GetHit());
				}
			}
			if (!blockGui && !warriorAnimationDemo.isBlocking && !warriorAnimationDemo.ledge)
			{
				if (GUI.Button(new Rect(25f, 20f, 100f, 30f), "Dash Forward"))
				{
					StartCoroutine(warriorAnimationDemo._Dash(1));
				}
				if (GUI.Button(new Rect(135f, 20f, 100f, 30f), "Dash Right"))
				{
					StartCoroutine(warriorAnimationDemo._Dash(2));
				}
				if (!warriorAnimationDemo.ledge && GUI.Button(new Rect(245f, 20f, 100f, 30f), "Jump") && warriorAnimationDemo.canJump && warriorAnimationDemo.isGrounded)
				{
					StartCoroutine(warriorAnimationDemo._Jump(0.8f));
				}
				if (GUI.Button(new Rect(25f, 50f, 100f, 30f), "Dash Backward"))
				{
					StartCoroutine(warriorAnimationDemo._Dash(3));
				}
				if (GUI.Button(new Rect(135f, 50f, 100f, 30f), "Dash Left"))
				{
					StartCoroutine(warriorAnimationDemo._Dash(4));
				}
				if (warriorAnimationDemo.warrior == Warrior.Knight)
				{
					if (GUI.Button(new Rect(355f, 20f, 100f, 30f), "Roll Forward"))
					{
						StartCoroutine(warriorAnimationDemo._Dash2(1));
					}
					if (GUI.Button(new Rect(355f, 50f, 100f, 30f), "Roll Backward"))
					{
						StartCoroutine(warriorAnimationDemo._Dash2(3));
					}
					if (GUI.Button(new Rect(460f, 20f, 100f, 30f), "Roll Left"))
					{
						StartCoroutine(warriorAnimationDemo._Dash2(4));
					}
					if (GUI.Button(new Rect(460f, 50f, 100f, 30f), "Roll Right"))
					{
						StartCoroutine(warriorAnimationDemo._Dash2(2));
					}
				}
				if (GUI.Button(new Rect(25f, 85f, 100f, 30f), "Attack Chain") && warriorAnimationDemo.attack <= 3)
				{
					warriorAnimationDemo.AttackChain();
				}
				if ((warriorAnimationDemo.warrior == Warrior.Sorceress || warriorAnimationDemo.warrior == Warrior.Karate || warriorAnimationDemo.warrior == Warrior.Spearman) && GUI.Button(new Rect(135f, 85f, 100f, 30f), "Attack 4"))
				{
					warriorAnimationDemo.animator.SetInteger("Attack", 4);
					StartCoroutine(warriorAnimationDemo._LockMovementAndAttack(0.65f));
				}
				if ((warriorAnimationDemo.warrior == Warrior.Sorceress || warriorAnimationDemo.warrior == Warrior.Karate || warriorAnimationDemo.warrior == Warrior.Spearman) && GUI.Button(new Rect(245f, 85f, 100f, 30f), "Attack 5"))
				{
					warriorAnimationDemo.animator.SetInteger("Attack", 5);
					if (warriorAnimationDemo.warrior == Warrior.Spearman)
					{
						StartCoroutine(warriorAnimationDemo._LockMovementAndAttack(1.15f));
					}
					else
					{
						StartCoroutine(warriorAnimationDemo._LockMovementAndAttack(1f));
					}
				}
				if ((warriorAnimationDemo.warrior == Warrior.Sorceress || warriorAnimationDemo.warrior == Warrior.Karate) && GUI.Button(new Rect(355f, 85f, 100f, 30f), "Attack 6"))
				{
					warriorAnimationDemo.animator.SetInteger("Attack", 6);
					if (warriorAnimationDemo.warrior == Warrior.Sorceress)
					{
						StartCoroutine(warriorAnimationDemo._LockMovementAndAttack(1.25f));
					}
					if (warriorAnimationDemo.warrior == Warrior.Karate)
					{
						StartCoroutine(warriorAnimationDemo._LockMovementAndAttack(0.8f));
					}
				}
				if ((warriorAnimationDemo.warrior == Warrior.Sorceress || warriorAnimationDemo.warrior == Warrior.Karate) && GUI.Button(new Rect(465f, 85f, 100f, 30f), "Attack 7"))
				{
					warriorAnimationDemo.animator.SetInteger("Attack", 7);
					if (warriorAnimationDemo.warrior == Warrior.Sorceress)
					{
						StartCoroutine(warriorAnimationDemo._LockMovementAndAttack(1.25f));
					}
					if (warriorAnimationDemo.warrior == Warrior.Karate)
					{
						StartCoroutine(warriorAnimationDemo._LockMovementAndAttack(1.1f));
					}
				}
				if (warriorAnimationDemo.warrior == Warrior.Karate)
				{
					if (GUI.Button(new Rect(575f, 85f, 100f, 30f), "Attack 8"))
					{
						warriorAnimationDemo.animator.SetInteger("Attack", 8);
						StartCoroutine(warriorAnimationDemo._LockMovementAndAttack(0.7f));
					}
					if (GUI.Button(new Rect(685f, 85f, 100f, 30f), "Attack 9"))
					{
						warriorAnimationDemo.animator.SetInteger("Attack", 9);
						StartCoroutine(warriorAnimationDemo._LockMovementAndAttack(0.7f));
					}
				}
				if (warriorAnimationDemo.warrior == Warrior.Sorceress && GUI.Button(new Rect(585f, 85f, 100f, 30f), "Attack 8"))
				{
					warriorAnimationDemo.animator.SetInteger("Attack", 8);
					StartCoroutine(warriorAnimationDemo._LockMovementAndAttack(1.3f));
				}
				if (warriorAnimationDemo.warrior == Warrior.Crossbow && GUI.Button(new Rect(135f, 85f, 100f, 30f), "Reload"))
				{
					StartCoroutine(warriorAnimationDemo._SetLayerWeightForTime(1.2f));
					warriorAnimationDemo.animator.SetTrigger("Reload1Trigger");
				}
				if (warriorAnimationDemo.warrior == Warrior.Ninja)
				{
					if (GUI.Button(new Rect(135f, 85f, 100f, 30f), "Attack1_R") && warriorAnimationDemo.attack == 0)
					{
						warriorAnimationDemo.animator.SetTrigger("Attack1RTrigger");
						warriorAnimationDemo.attack = 4;
						StartCoroutine(warriorAnimationDemo._LockMovementAndAttack(0.8f));
					}
					if (GUI.Button(new Rect(245f, 85f, 100f, 30f), "Attack2_R") && warriorAnimationDemo.attack == 0)
					{
						warriorAnimationDemo.attack = 4;
						warriorAnimationDemo.animator.SetTrigger("Attack2RTrigger");
						StartCoroutine(warriorAnimationDemo._LockMovementAndAttack(0.8f));
					}
				}
				if (!blockGui && !warriorAnimationDemo.isBlocking)
				{
					if (GUI.Button(new Rect(25f, 115f, 100f, 30f), "RangeAttack1") && warriorAnimationDemo.attack == 0)
					{
						warriorAnimationDemo.RangedAttack();
					}
					if ((warriorAnimationDemo.warrior == Warrior.Ninja || warriorAnimationDemo.warrior == Warrior.Crossbow || warriorAnimationDemo.warrior == Warrior.Karate || warriorAnimationDemo.warrior == Warrior.Mage) && GUI.Button(new Rect(135f, 115f, 100f, 30f), "RangeAttack2"))
					{
						if (warriorAnimationDemo.warrior != 0)
						{
							if (warriorAnimationDemo.attack == 0)
							{
								warriorAnimationDemo.attack = 4;
								warriorAnimationDemo.animator.SetTrigger("RangeAttack2Trigger");
								if (warriorAnimationDemo.warrior == Warrior.Crossbow)
								{
									if (warriorAnimationDemo.animator.GetBool("Moving"))
									{
										StartCoroutine(warriorAnimationDemo._SetLayerWeightForTime(0.6f));
									}
									else
									{
										StartCoroutine(warriorAnimationDemo._LockMovementAndAttack(0.7f));
									}
									warriorAnimationDemo.animator.SetInteger("Attack", 0);
									warriorAnimationDemo.attack = 0;
								}
								else if (warriorAnimationDemo.warrior == Warrior.Mage)
								{
									StartCoroutine(warriorAnimationDemo._LockMovementAndAttack(2f));
								}
								else
								{
									StartCoroutine(warriorAnimationDemo._LockMovementAndAttack(0.6f));
								}
							}
						}
						else
						{
							warriorAnimationDemo.animator.SetTrigger("RangeAttack2Trigger");
							StartCoroutine(warriorAnimationDemo._LockMovementAndAttack(1f));
						}
					}
					if (warriorAnimationDemo.warrior == Warrior.Ninja && GUI.Button(new Rect(245f, 115f, 100f, 30f), "RangeAttack3") && warriorAnimationDemo.attack == 0)
					{
						warriorAnimationDemo.attack = 4;
						warriorAnimationDemo.animator.SetTrigger("RangeAttack3Trigger");
						StartCoroutine(warriorAnimationDemo._LockMovementAndAttack(0.9f));
					}
					if (GUI.Button(new Rect(25f, 145f, 100f, 30f), "MoveAttack1") && warriorAnimationDemo.attack == 0)
					{
						warriorAnimationDemo.MoveAttack();
					}
					if ((warriorAnimationDemo.warrior == Warrior.Archer || warriorAnimationDemo.warrior == Warrior.Sorceress || warriorAnimationDemo.warrior == Warrior.Karate) && GUI.Button(new Rect(135f, 145f, 100f, 30f), "MoveAttack2"))
					{
						if (warriorAnimationDemo.warrior == Warrior.Archer && warriorAnimationDemo.attack == 0)
						{
							warriorAnimationDemo.attack = 4;
							warriorAnimationDemo.animator.SetTrigger("MoveAttack2Trigger");
							StartCoroutine(warriorAnimationDemo._ArcherArrowOn(0.6f));
							StartCoroutine(warriorAnimationDemo._LockMovementAndAttack(1.1f));
						}
						if (warriorAnimationDemo.warrior == Warrior.Sorceress)
						{
							warriorAnimationDemo.animator.SetTrigger("MoveAttack2Trigger");
							StartCoroutine(warriorAnimationDemo._LockMovementAndAttack(1.3f));
						}
						if (warriorAnimationDemo.warrior == Warrior.Karate)
						{
							warriorAnimationDemo.animator.SetTrigger("MoveAttack2Trigger");
							StartCoroutine(warriorAnimationDemo._LockMovementAndAttack(1f));
						}
					}
					if (warriorAnimationDemo.warrior == Warrior.Sorceress && GUI.Button(new Rect(245f, 145f, 100f, 30f), "MoveAttack3"))
					{
						warriorAnimationDemo.animator.SetTrigger("MoveAttack3Trigger");
						StartCoroutine(warriorAnimationDemo._LockMovementAndAttack(1f));
					}
					if (GUI.Button(new Rect(25f, 175f, 100f, 30f), "SpecialAttack1") && warriorAnimationDemo.attack == 0)
					{
						warriorAnimationDemo.SpecialAttack();
					}
					if ((warriorAnimationDemo.warrior == Warrior.Ninja || warriorAnimationDemo.warrior == Warrior.Sorceress || warriorAnimationDemo.warrior == Warrior.Karate || warriorAnimationDemo.warrior == Warrior.Knight || warriorAnimationDemo.warrior == Warrior.Mage) && GUI.Button(new Rect(135f, 175f, 100f, 30f), "SpecialAttack2") && warriorAnimationDemo.attack == 0)
					{
						if (warriorAnimationDemo.warrior == Warrior.Sorceress)
						{
							if (!warriorAnimationDemo.specialAttack2Bool)
							{
								warriorAnimationDemo.attack = 4;
								warriorAnimationDemo.animator.SetTrigger("SpecialAttack2Trigger");
								warriorAnimationDemo.animator.SetBool("warriorAnimationDemo.specialAttack2Bool", value: true);
								warriorAnimationDemo.specialAttack2Bool = true;
							}
							else
							{
								warriorAnimationDemo.attack = 4;
								warriorAnimationDemo.specialAttack2Bool = false;
								warriorAnimationDemo.animator.SetBool("warriorAnimationDemo.specialAttack2Bool", value: false);
								warriorAnimationDemo.animator.SetBool("SpecialAttack2Trigger", value: false);
							}
						}
						else
						{
							warriorAnimationDemo.attack = 4;
							warriorAnimationDemo.animator.SetTrigger("SpecialAttack2Trigger");
							if (warriorAnimationDemo.warrior == Warrior.Knight)
							{
								StartCoroutine(warriorAnimationDemo._LockMovementAndAttack(0.85f));
							}
							else if (warriorAnimationDemo.warrior == Warrior.Mage)
							{
								StartCoroutine(warriorAnimationDemo._LockMovementAndAttack(1.95f));
							}
							else
							{
								StartCoroutine(warriorAnimationDemo._LockMovementAndAttack(1.25f));
							}
						}
					}
					if (warriorAnimationDemo.warrior == Warrior.Sorceress && GUI.Button(new Rect(245f, 175f, 100f, 30f), "SpecialAttack3"))
					{
						warriorAnimationDemo.animator.SetTrigger("SpecialAttack3Trigger");
						StartCoroutine(warriorAnimationDemo._LockMovementAndAttack(1.2f));
					}
					if (GUI.Button(new Rect(30f, 270f, 100f, 30f), "Death"))
					{
						warriorAnimationDemo.animator.SetTrigger("DeathTrigger");
						warriorAnimationDemo.dead = true;
					}
					if ((warriorAnimationDemo.warrior == Warrior.Mage || warriorAnimationDemo.warrior == Warrior.Ninja || warriorAnimationDemo.warrior == Warrior.Knight || warriorAnimationDemo.warrior == Warrior.Archer || warriorAnimationDemo.warrior == Warrior.TwoHanded || warriorAnimationDemo.warrior == Warrior.Swordsman || warriorAnimationDemo.warrior == Warrior.Spearman || warriorAnimationDemo.warrior == Warrior.Hammer || warriorAnimationDemo.warrior == Warrior.Crossbow) && !warriorAnimationDemo.dead && !warriorAnimationDemo.weaponSheathed && !warriorAnimationDemo.weaponSheathed2 && GUI.Button(new Rect(30f, 310f, 100f, 30f), "Sheath Wpn"))
					{
						warriorAnimationDemo.SheathWeapon();
					}
					if (warriorAnimationDemo.warrior == Warrior.Knight && !warriorAnimationDemo.weaponSheathed && !warriorAnimationDemo.dead && !warriorAnimationDemo.weaponSheathed2 && GUI.Button(new Rect(140f, 310f, 100f, 30f), "Sheath Wpn2"))
					{
						StartCoroutine(warriorAnimationDemo._LockMovementAndAttack(1.4f));
						warriorAnimationDemo.animator.SetTrigger("WeaponSheath2Trigger");
						StartCoroutine(warriorAnimationDemo._WeaponVisibility(0.75f, weaponVisiblity: false));
						warriorAnimationDemo.weaponSheathed2 = true;
					}
					if (warriorAnimationDemo.warrior == Warrior.Knight && !warriorAnimationDemo.dead && warriorAnimationDemo.weaponSheathed2 && GUI.Button(new Rect(140f, 310f, 100f, 30f), "UnSheath Wpn2"))
					{
						StartCoroutine(warriorAnimationDemo._LockMovementAndAttack(1.4f));
						warriorAnimationDemo.animator.SetTrigger("WeaponUnsheath2Trigger");
						StartCoroutine(warriorAnimationDemo._WeaponVisibility(0.5f, weaponVisiblity: true));
						warriorAnimationDemo.weaponSheathed2 = false;
						warriorAnimationDemo.weaponSheathed = false;
					}
					if (warriorAnimationDemo.warrior == Warrior.Ninja && !warriorAnimationDemo.isStealth && !warriorAnimationDemo.dead && !warriorAnimationDemo.weaponSheathed && GUI.Button(new Rect(30f, 350f, 100f, 30f), "Stealth"))
					{
						warriorAnimationDemo.animator.SetBool("Stealth", value: true);
						warriorAnimationDemo.isStealth = true;
					}
					if (warriorAnimationDemo.warrior == Warrior.Ninja && warriorAnimationDemo.isStealth && !warriorAnimationDemo.isWall && !warriorAnimationDemo.dead && !warriorAnimationDemo.weaponSheathed && GUI.Button(new Rect(30f, 350f, 100f, 30f), "UnStealth"))
					{
						warriorAnimationDemo.animator.SetBool("Stealth", value: false);
						warriorAnimationDemo.isStealth = false;
					}
					if (warriorAnimationDemo.warrior == Warrior.Ninja && warriorAnimationDemo.isStealth && !warriorAnimationDemo.dead && !warriorAnimationDemo.weaponSheathed)
					{
						if (!warriorAnimationDemo.isWall)
						{
							if (GUI.Button(new Rect(140f, 350f, 100f, 30f), "Wall On"))
							{
								warriorAnimationDemo.animator.applyRootMotion = true;
								warriorAnimationDemo.animator.SetBool("Stealth-Wall", value: true);
								warriorAnimationDemo.isWall = true;
							}
						}
						else if (GUI.Button(new Rect(140f, 350f, 100f, 30f), "Wall Off"))
						{
							warriorAnimationDemo.animator.SetBool("Stealth-Wall", value: false);
							warriorAnimationDemo.isWall = false;
							StartCoroutine(warriorAnimationDemo._LockMovementAndAttack(0.7f));
						}
					}
				}
			}
		}
		if (warriorAnimationDemo.dead && GUI.Button(new Rect(30f, 270f, 100f, 30f), "Revive"))
		{
			StartCoroutine(warriorAnimationDemo._Revive());
		}
	}
}
