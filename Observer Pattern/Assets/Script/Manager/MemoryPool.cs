using UnityEngine;
using System.Collections;

public class MemoryPool : IEnumerable, System.IDisposable {
	class Item{
		public bool active;
		public GameObject gameObject;
	}

	private Item[] table;

	public IEnumerator GetEnumerator(){
		if (table == null)
			yield break;

		for (int i = 0; i < table.Length; i++) {
			Item item = table [i];
			if (item.active)
				yield return item.gameObject;
		}
	}

	//풀 생성 특정 오브젝트, 수량만큼 넣어준 뒤 액티브 꺼줌 
	public void CreatePool(Object _obj, int _count, Vector3 _pos){
		Dispose ();

		table = new Item[_count];
		GameObject Node = new GameObject ();
		Node.name = "Nodes";
		for (int i = 0; i < table.Length; i++) {
			table [i] = new Item ();
			table [i].active = false;
			table [i].gameObject = GameObject.Instantiate(_obj) as GameObject;
			table [i].gameObject.transform.position = _pos;
			table [i].gameObject.SetActive (false);
			table [i].gameObject.transform.parent = Node.transform;
		}
	}

	//만들어둔 오브젝트 켜줌 
	public GameObject NewItem(Vector3 _pos){
		if (table != null) {
			for (int i = 0; i < table.Length; i++) {
				if (table [i].active == false) {
					table [i].active = true;
					table [i].gameObject.SetActive (true);
					table [i].gameObject.transform.position = _pos;

					return table [i].gameObject;
				}
			}
		}

		return null;
	}

	//사용된 오브젝트 꺼둠 
	public void RemoveItem(GameObject _gameobject){
		if (table != null && _gameobject != null) {
			for (int i = 0; i < table.Length; i++) {
				if (table [i].gameObject == _gameobject) {
					table [i].active = false;
					table [i].gameObject.SetActive (false);
					break;
				}
			}
		}
	}

	//모든 오브젝트 꺼둠 
	public void PausePool(){
		if (table != null) {
			for (int i = 0; i < table.Length; i++) {
				if (table [i].active) {
					table [i].active = false;
					table [i].gameObject.SetActive (false);
				}
			}
		}
	}

	//풀 제거함 
	public void Dispose(){
		if (table != null) {
			for (int i = 0; i < table.Length; i++) {
				GameObject.Destroy (table [i].gameObject);
			}

			table = null;
		}
	}
}
