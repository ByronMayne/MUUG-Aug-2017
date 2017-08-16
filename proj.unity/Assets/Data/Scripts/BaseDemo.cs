using UnityEngine;

[CreateAssetMenu(menuName = "Demo/Object Refernces", fileName = "1) Object Reference")]
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
