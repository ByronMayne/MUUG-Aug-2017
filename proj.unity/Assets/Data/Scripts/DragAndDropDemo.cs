using MUUG;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(menuName = "Demo/Drag and Drop", fileName = "2) Drag And Drop")]
public class DragAndDropDemo : ScriptableObject
{
    [SerializeField, DragAndDrop(typeof(GameObject))]
    public string m_Environment;
    [SerializeField, DragAndDrop(typeof(ParticleSystem))]
    private string m_GunParticles;
    [SerializeField, DragAndDrop(typeof(Light))]
    private string m_Lights;
    [SerializeField, DragAndDrop(typeof(AudioClip))]
    protected string m_BackgroundMusic;
    [SerializeField, DragAndDrop(typeof(AudioMixer))]
    protected internal string m_MasterMixer;
}