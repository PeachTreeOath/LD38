using UnityEngine;
using System.Collections;

/// <summary>
/// Unity singleton implementation. Call SetDontDestroy() to persist objects between scenes. 
/// </summary>
/// <typeparam name="T">Class to singleton</typeparam>
public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{

	static T instance;

	public static T Instance
	{
		get
		{
			MakeSingletone();

			return instance;
		}
	}

	void Start()
	{
		MakeSingletone();
	}

	static void MakeSingletone()
	{
		if(instance == null)
		{
			GameObject foo = new GameObject();
			foo.name = typeof(T).ToString();
			instance = foo.AddComponent<T>();
			instance.SendMessage("Init", SendMessageOptions.DontRequireReceiver);
			DontDestroyOnLoad(foo);
		}
	}

	protected abstract void Init();
}
