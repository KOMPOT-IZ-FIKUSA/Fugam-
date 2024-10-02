using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IThrowableItem
{
    public GameObject CreateGameObject(Vector3 position, Vector3 direction);
}
