using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsSpawner : MonoBehaviour
{
    [SerializeField] List<GameObject> _spawnableEffects;

    /// <summary>
    /// All effects that are spawnable by the object are in this
    /// The key is the name of the effect and the value is the effect
    /// </summary>
    Dictionary<string, GameObject> _effects = new Dictionary<string, GameObject>();

    private void Start()
    {
        foreach (var _effect in _spawnableEffects)
        {
            _effects.Add(_effect.name, _effect);
        }
    }

    /// <summary>
    /// Spawn an effect by the given name and return it;
    /// Do a debug.log if the name is incorrect
    /// </summary>
    /// <param name="name">The name of the effect, corresponds to a key in the dict of effects this holds</param>
    /// <param name="parent">Wether the gameObject that owns this script should be the parent or not</param>
    public GameObject SpawnEffect(string name, bool parent = false)
    {
        if (_effects.ContainsKey(name))
        {
            if (parent)
            {
                return Instantiate(_effects[name], transform);
            }
            else
            {
                return Instantiate(_effects[name], transform.position, transform.rotation);
            }
        }
        else
        {
            Debug.Log(transform.name + " doesn't countain an effect named : " + name + ".");
            return null;
        }
    }

    /// <summary>
    /// Spawn an effect by the given name and return it;
    /// Do a debug.log if the name is incorrect
    /// </summary>
    /// <param name="name">The name of the effect, corresponds to a key in the dict of effects this holds</param>
    /// <param name="parent">The parent of the new object</param>
    public GameObject SpawnEffect(string name, Transform parent)
    {
        if (_effects.ContainsKey(name))
        {
            return Instantiate(_effects[name], parent);
        }
        else
        {
            Debug.Log(transform.name + " doesn't countain an effect named : " + name + ".");
            return null;
        }
    }
}
