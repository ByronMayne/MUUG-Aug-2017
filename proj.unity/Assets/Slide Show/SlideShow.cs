using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
/// <summary>
/// This is a hacky script so don't judge me :D
/// </summary>
public class SlideShow : EditorWindow
{
    private const string MENU_PATH = "Slideshow/Open... &s";
    private const string STATE_PLAYER_PREF_KEY = "SlideShow.State";
    private const float ASPECT_RATIO = 16f / 9f;

    private SlideShowState m_State = new SlideShowState();
    private Slide[] m_Slides;
    private GUIContent m_CurrentSlide;

    [MenuItem(MENU_PATH)]
    private static void GetWindow()
    {
        GetWindow<SlideShow>().Focus();
    }

    private void OnEnable()
    {
        LoadSlides();
        m_CurrentSlide = null;

        string json = PlayerPrefs.GetString(STATE_PLAYER_PREF_KEY, string.Empty);
        try
        {
            m_State = JsonUtility.FromJson<SlideShowState>(json);
        }
        finally
        {
            if (m_State == null)
            {
                m_State = new SlideShowState();
            }
        }
        m_CurrentSlide = new GUIContent();
        LoadSlidePaths();

        Selection.selectionChanged += OnSelectionChanged;
    }

    private void OnSelectionChanged()
    {
        Slide[] selectedSlides = Selection.GetFiltered<Slide>(SelectionMode.TopLevel);
        if (selectedSlides.Length > 0)
        {
            for (int i = 0; i < m_Slides.Length; i++)
            {
                if (m_Slides[i] == selectedSlides[0])
                {
                    JumpTo(i);
                    break;
                }
            }
        }
    }

    public void OnDisable()
    {
        string json = JsonUtility.ToJson(m_State);
        PlayerPrefs.SetString(STATE_PLAYER_PREF_KEY, json);
        Selection.selectionChanged -= OnSelectionChanged;
    }


    private void OnGUI()
    {
        // Toolbar
        DrawToolBar();
        HandleInput();

        float width = position.width;
        float height = position.height;
        float aspectHeight = width / ASPECT_RATIO;

        if (aspectHeight > height)
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
            GUIContent jumpLabel = new GUIContent(m_State.currentSlide.ToString("0: "));
            Rect jumpButtonRect = GUILayoutUtility.GetRect(jumpLabel, EditorStyles.toolbarButton);
            if (GUI.Button(jumpButtonRect, jumpLabel, EditorStyles.toolbarButton))
            {
                GenericMenu jumpMenu = new GenericMenu();
                for (int i = 0; i < m_Slides.Length; i++)
                {
                    GUIContent label = new GUIContent(i + ": " + m_Slides[i].title);
                    jumpMenu.AddItem(label, false, (object index) => JumpTo((int)index), i);
                }
                jumpMenu.DropDown(jumpButtonRect);
            }


            if (currentSlide != null && currentSlide.context != null && GUILayout.Button("Context", EditorStyles.toolbarButton))
            {
                if (currentSlide.context is TextAsset)
                {
                    AssetDatabase.OpenAsset(currentSlide.context, 0);
                }
                else
                {
                    Selection.activeObject = currentSlide.context;
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

            GUILayout.Space(10);

            if (GUILayout.Button("Load Slides", EditorStyles.toolbarButton))
            {
                LoadSlides();
            }

        }
        EditorGUILayout.EndHorizontal();
    }

    private void LoadSlides()
    {
        LoadSlidePaths();
        m_State.currentSlide = 0;
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
        get
        {
            if (m_Slides.Length == 0)
            {
                return null;
            }
            return m_Slides[m_State.currentSlide];
        }
    }

    private void JumpTo(int index)
    {
        if (index >= 0 && index < m_Slides.Length)
        {
            m_State.currentSlide = index;
            LoadSlide();
        }
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
        LoadSlide();
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
        LoadSlide();
    }

    private void LoadSlidePaths()
    {
        string[] guids = AssetDatabase.FindAssets("t:Slide ");
        m_Slides = new Slide[guids.Length];
        for (int i = 0; i < guids.Length; i++)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
            m_Slides[i] = AssetDatabase.LoadAssetAtPath<Slide>(assetPath);
        }

        LoadSlide();
    }


    public void LoadSlide()
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
