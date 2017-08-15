using UnityEngine;

[CreateAssetMenu(menuName="Demo/Base")]
public class BaseDemo : ScriptableObject
{
    public GameObject playerCoat;
    [SerializeField]
    private GameObject m_PlayerPrefab;
    [SerializeField]
    private GameObject m_GunPrefab;
    [SerializeField]
    protected AudioClip m_GunSound;
    [SerializeField]
    protected internal Transform m_PlayerSpawn; 
}
