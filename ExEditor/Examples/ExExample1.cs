using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Esa
{
	public class MadExample1 : MonoBehaviour
	{
		public int id;
		[Group(GroupAttribute.EState.StartGroup, "Texts")]
		public string text1;
		public string text2;
		[Group(GroupAttribute.EState.EndGroup)]
		public string text3;
		[Group(GroupAttribute.EState.StartAndEndGroup)]
		public List<string> textList;
		public RuntimePlatform platform;

		[ShowProperty(ShowPropertyAttribute.EValueType.Int)]
		public int ID
		{
			get { return id; }
			set { id = value; }
		}

		[ShowProperty(ShowPropertyAttribute.EValueType.Enum)]
		public RuntimePlatform Platform
		{
			get { return platform; }
			set { platform = value; }
		}

		[ShowProperty(ShowPropertyAttribute.EValueType.Bool)]
		public bool Enabled
		{
			get { return enabled; }
			set { enabled = value; }
		}

		[ShowProperty(ShowPropertyAttribute.EValueType.String)]
		public string Name
		{
			get { return gameObject.name; }
		}

		[ShowProperty(ShowPropertyAttribute.EValueType.Vector3)]
		public Vector3 Pos
		{
			get { return transform.localPosition; }
			set { transform.localPosition = value; }
		}

		[Esa.Button("Method A")]
		public void MethodA()
		{
			Debug.Log(gameObject.name + "：Method A");
		}

		[Esa.Button("Method B", ButtonAttribute.ESize.Mini)]
		public void MethodB()
		{
			Debug.Log(gameObject.name + "：Method B");
		}

		[Group(GroupAttribute.EState.StartGroup, "Color Methods")]
		[Esa.Button("Method C", ButtonAttribute.ESize.Common, ButtonAttribute.EColor.Red)]
		public void MethodC()
		{
			Debug.Log(gameObject.name + "：Method C");
		}
        [Esa.Button]
		//[Button("Static Method D", ButtonAttribute.ESize.Large, 1.0f, 1.0f, 0.7f)]
		static private void MethodD()
		{
			Debug.Log("Static Method D");
		}
	}
}
