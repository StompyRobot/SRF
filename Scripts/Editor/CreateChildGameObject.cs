using UnityEngine;
using UnityEditor;


public class FixStupidEditorBehavior : MonoBehaviour
{



	[MenuItem("GameObject/Create Empty Parent #&e")]

	static void createEmptyParent()
	{

		GameObject go = new GameObject("GameObject");

		if (Selection.activeTransform != null) {



			go.transform.parent = Selection.activeTransform.parent;



			go.transform.Translate(Selection.activeTransform.position);



			Selection.activeTransform.parent = go.transform;



		}



	}





	[MenuItem("GameObject/Create Empty Duplicate #&d")]

	static void createEmptyDuplicate()
	{



		GameObject go = new GameObject("GameObject");



		if (Selection.activeTransform != null) {

			go.transform.parent = Selection.activeTransform.parent;

			go.transform.Translate(Selection.activeTransform.position);

		}



	}



	[MenuItem("GameObject/Create Empty Child #&c")]

	static void createEmptyChild()
	{



		GameObject go = new GameObject("GameObject");



		if (Selection.activeTransform != null) {

			go.transform.parent = Selection.activeTransform;

			go.transform.Translate(Selection.activeTransform.position);

		}



	}



}