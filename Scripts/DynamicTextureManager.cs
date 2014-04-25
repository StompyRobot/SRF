using System.Collections.Generic;
using UnityEngine;
using System.Collections;

/// <summary>
/// Handles dynamically loading and unloading textures from Resources or StreamingAssets
/// </summary>
public class DynamicTextureManager : SRAutoSingleton<DynamicTextureManager>
{

	public enum Priority
	{

		Low = 0,
		Medium = 1,
		High = 2

	}

	class TextureEntry
	{

		public int RefCount;

		public Texture Texture;

		public Priority Priority;

	}

	private Dictionary<string, TextureEntry> _textures = new Dictionary<string, TextureEntry>();

	/// <summary>
	/// Preload a texture.
	/// </summary>
	/// <param name="path">Path to texture</param>
	/// <param name="priority">Textures with higher priority will be kept in memory longer.</param>
	public void Preload(string path, Priority priority = Priority.Medium)
	{

		if (_textures.ContainsKey(path))
			return;

		var t = Resources.Load<Texture>(path);

		if (t == null) {
			Debug.LogError("Resource not found: " + path);
			return;
		}

		_textures.Add(path, new TextureEntry() {
			Texture = t,
			Priority = priority,
			RefCount = 0
		});

	}

	/// <summary>
	/// Get the texture at path. Remember to Release when done, so it can be freed later if required.
	/// </summary>
	/// <param name="path"></param>
	/// <returns></returns>
	public Texture Get(string path)
	{
		
		Preload(path);

		var t = _textures[path];

		t.RefCount++;

		return t.Texture;

	}

	/// <summary>
	/// Free texture at path.
	/// </summary>
	/// <param name="path"></param>
	public void Free(string path)
	{

		_textures[path].RefCount--;

	}

	/// <summary>
	/// Free texture
	/// </summary>
	/// <param name="tex"></param>
	public void Free(Texture tex)
	{

		foreach (var t in _textures) {

			if (t.Value.Texture == tex) {
				t.Value.RefCount--;
				return;
			}

		}

		Debug.LogWarning("Texture was not found in DynamicTextureManager.", this);

	}

}
