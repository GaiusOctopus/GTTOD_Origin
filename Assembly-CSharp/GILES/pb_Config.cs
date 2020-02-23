using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.Playables;
using UnityEngine.UI;

namespace GILES
{
	public static class pb_Config
	{
		public const int ASSET_MENU_ORDER = 800;

		public static readonly string[] Resource_Folder_Paths = new string[1]
		{
			"Level Editor Prefabs"
		};

		public static readonly string[] AssetBundle_Names = new string[1]
		{
			"TestAssets"
		};

		public static readonly string[] AssetBundle_SearchDirectories = new string[1]
		{
			"Assets/AssetBundles/"
		};

		public static readonly HashSet<Type> IgnoredComponentsInInspector = new HashSet<Type>
		{
			typeof(OcclusionArea),
			typeof(OcclusionPortal),
			typeof(MeshFilter),
			typeof(SkinnedMeshRenderer),
			typeof(LensFlare),
			typeof(Renderer),
			typeof(Projector),
			typeof(Skybox),
			typeof(TrailRenderer),
			typeof(LineRenderer),
			typeof(MeshRenderer),
			typeof(GUIElement),
			typeof(Image),
			typeof(GUILayer),
			typeof(ReflectionProbe),
			typeof(LODGroup),
			typeof(FlareLayer),
			typeof(LightProbeGroup),
			typeof(RectTransform),
			typeof(SpriteRenderer),
			typeof(Behaviour),
			typeof(Camera),
			typeof(MonoBehaviour),
			typeof(Component),
			typeof(BillboardRenderer),
			typeof(PlayableDirector),
			typeof(WindZone),
			typeof(ParticleSystem),
			typeof(ParticleSystemRenderer),
			typeof(Rigidbody),
			typeof(Joint),
			typeof(HingeJoint),
			typeof(SpringJoint),
			typeof(FixedJoint),
			typeof(CharacterJoint),
			typeof(ConfigurableJoint),
			typeof(ConstantForce),
			typeof(Collider),
			typeof(BoxCollider),
			typeof(SphereCollider),
			typeof(MeshCollider),
			typeof(CapsuleCollider),
			typeof(WheelCollider),
			typeof(CharacterController),
			typeof(Cloth),
			typeof(Rigidbody2D),
			typeof(Collider2D),
			typeof(CircleCollider2D),
			typeof(BoxCollider2D),
			typeof(EdgeCollider2D),
			typeof(PolygonCollider2D),
			typeof(Joint2D),
			typeof(AnchoredJoint2D),
			typeof(SpringJoint2D),
			typeof(DistanceJoint2D),
			typeof(HingeJoint2D),
			typeof(SliderJoint2D),
			typeof(WheelJoint2D),
			typeof(PhysicsUpdateBehaviour2D),
			typeof(ConstantForce2D),
			typeof(Effector2D),
			typeof(AreaEffector2D),
			typeof(PointEffector2D),
			typeof(PlatformEffector2D),
			typeof(SurfaceEffector2D),
			typeof(NavMeshAgent),
			typeof(NavMeshObstacle),
			typeof(OffMeshLink),
			typeof(AudioListener),
			typeof(AudioSource),
			typeof(AudioReverbZone),
			typeof(AudioLowPassFilter),
			typeof(AudioHighPassFilter),
			typeof(AudioDistortionFilter),
			typeof(AudioEchoFilter),
			typeof(AudioChorusFilter),
			typeof(AudioReverbFilter),
			typeof(Animation),
			typeof(Animator),
			typeof(Terrain),
			typeof(Tree),
			typeof(GUIText),
			typeof(TextMesh),
			typeof(Canvas),
			typeof(CanvasGroup),
			typeof(CanvasRenderer),
			typeof(TerrainCollider),
			typeof(NetworkMatch),
			typeof(NetworkAnimator),
			typeof(NetworkBehaviour),
			typeof(NetworkDiscovery),
			typeof(NetworkIdentity),
			typeof(NetworkLobbyManager),
			typeof(NetworkLobbyPlayer),
			typeof(NetworkManager),
			typeof(NetworkManagerHUD),
			typeof(NetworkProximityChecker),
			typeof(NetworkStartPosition),
			typeof(NetworkTransformChild),
			typeof(NetworkTransform),
			typeof(NetworkTransformVisualizer),
			typeof(EventSystem),
			typeof(EventTrigger),
			typeof(UIBehaviour),
			typeof(BaseInputModule),
			typeof(PointerInputModule),
			typeof(StandaloneInputModule),
			typeof(BaseRaycaster),
			typeof(Physics2DRaycaster),
			typeof(PhysicsRaycaster),
			typeof(Button),
			typeof(Dropdown),
			typeof(Graphic),
			typeof(GraphicRaycaster),
			typeof(Image),
			typeof(InputField),
			typeof(Mask),
			typeof(MaskableGraphic),
			typeof(RawImage),
			typeof(RectMask2D),
			typeof(Scrollbar),
			typeof(ScrollRect),
			typeof(Selectable),
			typeof(Slider),
			typeof(Text),
			typeof(Toggle),
			typeof(ToggleGroup),
			typeof(AspectRatioFitter),
			typeof(CanvasScaler),
			typeof(ContentSizeFitter),
			typeof(GridLayoutGroup),
			typeof(HorizontalLayoutGroup),
			typeof(HorizontalOrVerticalLayoutGroup),
			typeof(LayoutElement),
			typeof(LayoutGroup),
			typeof(VerticalLayoutGroup),
			typeof(BaseMeshEffect),
			typeof(Outline),
			typeof(PositionAsUV1),
			typeof(Shadow)
		};
	}
}
