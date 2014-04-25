using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AudioLogView : EditorWindow
{
    [MenuItem( "Window/Audio Toolkit/Log" )]
    static void ShowWindow()
    {
        EditorWindow.GetWindow( typeof( AudioLogView ) );
    }

    static Vector2 _scrollPos;

#if AUDIO_TOOLKIT_DEMO
    void OnGUI()
    {
         EditorGUILayout.LabelField( "Audio Log is not available in the FREE version of Audio Toolkit. Please buy the full version." );
    }
#else
    void OnGUI()
    {
        // header

        float defaultColumnWidth = 120;
        float timeColumnWidth = 60;
        float nameColumnWidth = 90;

        GUIStyle headerStyle = new GUIStyle( EditorStyles.boldLabel );


        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.LabelField( "time", GUILayout.Width( timeColumnWidth ) );
        EditorGUILayout.LabelField( "audioID", headerStyle, GUILayout.Width( defaultColumnWidth ) );
        EditorGUILayout.LabelField( "clipName", headerStyle, GUILayout.Width( nameColumnWidth ) );
        EditorGUILayout.LabelField( "category", headerStyle, GUILayout.Width( nameColumnWidth ) );
        EditorGUILayout.LabelField( "volume", GUILayout.Width( timeColumnWidth ) );
        EditorGUILayout.LabelField( "startTime", GUILayout.Width( timeColumnWidth ) );
        EditorGUILayout.LabelField( "scheduledDSP", GUILayout.Width( timeColumnWidth ) );
        EditorGUILayout.LabelField( "delay", GUILayout.Width( timeColumnWidth ) );
        EditorGUILayout.LabelField( "parent", headerStyle, GUILayout.Width( defaultColumnWidth ) );
        EditorGUILayout.LabelField( "worldPos", headerStyle, GUILayout.Width( defaultColumnWidth ) );

        EditorGUILayout.EndHorizontal();

        // data

        AudioLog.LogData_PlayClip loggedClip;

        _scrollPos = EditorGUILayout.BeginScrollView( _scrollPos );

        foreach ( var log in AudioLog.logData )
        {
            EditorGUILayout.BeginHorizontal();
            
            loggedClip = log as AudioLog.LogData_PlayClip;
            if( loggedClip != null )
            {
                EditorGUILayout.LabelField( string.Format( "{0:0.00}", loggedClip.time ), GUILayout.Width( timeColumnWidth ) );
                EditorGUILayout.LabelField( loggedClip.audioID, GUILayout.Width( defaultColumnWidth ) );
                EditorGUILayout.LabelField( loggedClip.clipName, GUILayout.Width( nameColumnWidth ) );
                EditorGUILayout.LabelField( loggedClip.category, GUILayout.Width( nameColumnWidth ) );
                EditorGUILayout.LabelField( string.Format( "{0:0.00}", loggedClip.volume ), GUILayout.Width( timeColumnWidth ) );
                EditorGUILayout.LabelField( string.Format( "{0:0.00}", loggedClip.startTime ), GUILayout.Width( timeColumnWidth ) );
                EditorGUILayout.LabelField( string.Format( "{0:0.00}", loggedClip.scheduledDspTime ), GUILayout.Width( timeColumnWidth ) );
                EditorGUILayout.LabelField( string.Format( "{0:0.00}", loggedClip.delay ), GUILayout.Width( timeColumnWidth ) );
                EditorGUILayout.LabelField( loggedClip.parentObject, GUILayout.Width( defaultColumnWidth ) );
                EditorGUILayout.LabelField( string.Format( "{0:0.0} / {1:0.0} / {2:0.0}", loggedClip.position.x, loggedClip.position.y, loggedClip.position.z ), GUILayout.Width( defaultColumnWidth ) );
                
            }

            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndScrollView();

        if ( GUILayout.Button( "Clear", GUILayout.Width( 120 ) ) )
        {
            AudioLog.Clear();
        }
    }

    void OnNewLogEntry()
    {
        Repaint();
    }

    void OnEnable()
    {
        AudioLog.onLogUpdated += OnNewLogEntry;
    }

    void OnDisable()
    {
        AudioLog.onLogUpdated -= OnNewLogEntry;

    }
#endif
}
