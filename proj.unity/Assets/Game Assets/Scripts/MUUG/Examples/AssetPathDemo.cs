using MUUG;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(menuName = "Demo/Asset Path")]
public class AssetPathDemo : ScriptableObject
{
    [SerializeField, AssetPath(typeof(GameObject))]
    public string m_Environment;
    [SerializeField, AssetPath(typeof(ParticleSystem))]
    private string m_GunParticles;
    [SerializeField, AssetPath(typeof(Light))]
    private string m_Lights;
    [SerializeField, AssetPath(typeof(AudioClip))]
    protected string m_BackgroundMusic;
    [SerializeField, AssetPath(typeof(AudioMixer))]
    protected internal string m_MasterMixer;
}