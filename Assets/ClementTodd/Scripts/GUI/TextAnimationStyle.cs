using UnityEngine;
using System;

namespace ClementTodd.GUI
{
	[Serializable]
	public class TextAnimationStyle
	{
		public string name;

		public enum Type
		{
			None,
			Bounce,
			Wave
		}

		[Serializable]
		public class Translation
		{
			public Type type = Type.None;
			public Vector2 offset = new Vector2(0f, 10f);
			public float duration = 0.5f;
			public float delayPerCharacter = 0.1f;
		}

		[Serializable]
		public class Rotation
		{
			public Type type = Type.None;
			public float startAngle = 0f;
			public float angleOffset = 45f;
			public float duration = 0.5f;
			public float delayPerCharacter = 0.1f;
		}

		[Serializable]
		public class Scale
		{
			public Type type = Type.None;
			public Vector2 startScale = Vector2.one;
			public Vector2 scaleOffset = Vector2.one * 0.2f;
			public float duration = 0.5f;
			public float delayPerCharacter = 0.1f;
		}

		[Serializable]
		public class AnimationCurveXY
		{
			public AnimationCurve x;
			public AnimationCurve y;

			public AnimationCurveXY(float defaultValue)
			{
				x = AnimationCurve.Constant(0f, 1f, defaultValue);
				y = AnimationCurve.Constant(0f, 1f, defaultValue);
			}
		}

		[Header("Repeating Animations")]
		public Translation translation;
		public Rotation rotation;
		public Scale scale;

		[Header("Typewriter Animations")]
		public AnimationCurveXY translateIn = new AnimationCurveXY(0f);
		public AnimationCurve rotateIn = AnimationCurve.Constant(0f, 1f, 0f);
		public AnimationCurveXY scaleIn = new AnimationCurveXY(1f);
	}
}