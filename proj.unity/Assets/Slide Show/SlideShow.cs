using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SlideShow : EditorWindow
{
    private const string MENU_PATH = "MUGG/Open Slide Show #s";
    private const string STATE_PLAYER_PREF_KEY = "SlideShow.State";
    private const float ASPECT_RATIO = 16f / 9f;

    private SlideShowState m_State;
    private Slide[] m_Slides;
    private GUIContent m_CurrentSlide;

    [MenuItem(MENU_PATH)]
    private static void GetWindow()
    {
        GetWindow<SlideShow>().Focus();   
    }

    private void OnEnable()
    {
        string json = PlayerPrefs.GetString(STATE_PLAYER_PREF_KEY, string.Empty);
        try
        {
            m_State = JsonUtility.FromJson<SlideShowState>(json);
        }
        catch
        { }

        if (m_State == null)
        {
            m_State = new SlideShowState();
        }
        m_CurrentSlide = new GUIContent();
        LoadSlidePaths();
    }

    public void OnDisable()
    {
        string json = JsonUtility.ToJson(m_State);
        PlayerPrefs.SetString(STATE_PLAYER_PREF_KEY, json);
    }


    private void OnGUI()
    {
        // Toolbar
        DrawToolBar();
        HandleInput();

        float width = position.width;
        float height = position.height;
        float aspectHeight = width / ASPECT_RATIO;

        if(aspectHeight > height)
        {
            aspectHeight = height;
            width = aspectHeight * ASPECT_RATIO;
            height = aspectHeight;
        }

        Rect slideRect = GUILayoutUtility.GetRect(width, height);

        if (m_CurrentSlide != null)
        {
            GUI.Box(slideRect, m_CurrentSlide);
        }
    }

    private void DrawToolBar()
    {
        EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
        {
            if (GUILayout.Button("Load Slides", EditorStyles.toolbarButton))
            {
                string defaultPath = string.IsNullOrEmpty(m_State.directory) ? Application.dataPath + "/" : m_State.directory;
                string slidesDirectory = EditorUtility.OpenFolderPanel("Slides Location", defaultPath, "Slides");
                slidesDirectory = FileUtil.GetProjectRelativePath(slidesDirectory);
                if (!slidesDirectory.Equals(m_State.directory, System.StringComparison.Ordinal))
                {
                    m_State.directory = slidesDirectory;
                    LoadSlidePaths();
                    m_State.currentSlide = 0;
                }

            }

            GUILayout.FlexibleSpace();

            if (GUILayout.Button("<", EditorStyles.toolbarButton))
            {
                Previous();
            }

            if (GUILayout.Button(">", EditorStyles.toolbarButton))
            {
                Next();
            }
        }
        EditorGUILayout.EndHorizontal();
    }

    private void HandleInput()
    {
        Event current = Event.current;
        if (current.type == EventType.KeyDown)
        {
            if (current.keyCode == KeyCode.LeftArrow)
            {
                Previous();
                current.Use();
            }
            else if (current.keyCode == KeyCode.RightArrow)
            {
                Next();
                current.Use();
            }
        }
    }

    private Slide currentSlide
    {
        get { return m_Slides[m_State.currentSlide]; }
    }


    private void Next()
    {
        int index = m_State.currentSlide;
        index++;
        if (index >= m_Slides.Length)
        {
            index--;
        }
        m_State.currentSlide = index;
        LoadCurrentSlide();
    }

    private void Previous()
    {
        int index = m_State.currentSlide;
        index--;
        if (index < 0)
        {
            index = 0;
        }
        m_State.currentSlide = index;
        LoadCurrentSlide();
    }

    private void LoadSlidePaths()
    {
        if (string.IsNullOrEmpty(m_State.directory))
        {
            m_Slides = new Slide[0];
        }
        else
        {
            string[] guids = AssetDatabase.FindAssets("t:Slide ", new string[] { m_State.directory });
            m_Slides = new Slide[guids.Length];
            for (int i = 0; i < guids.Length; i++)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
                m_Slides[i] = AssetDatabase.LoadAssetAtPath<Slide>(assetPath);
            }
        }
        LoadCurrentSlide();
    }


    public void LoadCurrentSlide()
    {
        if (m_Slides.Length == 0)
        {
            m_State.currentSlide = 0;
            m_CurrentSlide = new GUIContent("[No Slides Loaded]");
        }
        else
        {
            m_CurrentSlide.image = m_Slides[m_State.currentSlide].slideImage;
            m_CurrentSlide.text = string.Empty;
            Repaint();
        }
    }
}
