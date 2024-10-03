using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An interface for an item that can be released into the world.
/// </summary>
public interface IThrowableItem
{
    public GameObject CreateGameObject(Vector3 position, Vector3 direction);
}
