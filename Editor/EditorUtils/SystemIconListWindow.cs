using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// unity内置Icon集合
/// </summary>
public class SystemIconListWindow : EditorWindow
{
    public List<string> icons = new List<string>() {
        "TreeEditor.AddLeaves","TreeEditor.AddBranches","TreeEditor.Trash","TreeEditor.Duplicate","TreeEditor.Refresh","editicon.sml","tree_icon_branch_frond","tree_icon_branch","tree_icon_frond","tree_icon_leaf","tree_icon","animationvisibilitytoggleon","animationvisibilitytoggleoff","MonoLogo","AgeiaLogo","AboutWindow.MainHeader","Animation.AddEvent","lightMeter/greenLight","lightMeter/lightRim","lightMeter/orangeLight","lightMeter/redLight","Animation.PrevKey","Animation.NextKey","Animation.AddKeyframe","Animation.EventMarker","Animation.Play","Animation.Record","AS Badge Delete","AS Badge New","preAudioAutoPlayOff","preAudioAutoPlayOn","preAudioPlayOff","preAudioPlayOn","preAudioLoopOff","preAudioLoopOn","AvatarInspector/BodySilhouette","AvatarInspector/HeadZoomSilhouette","AvatarInspector/LeftHandZoomSilhouette","AvatarInspector/RightHandZoomSilhouette","AvatarInspector/Torso","AvatarInspector/Head","AvatarInspector/LeftArm","AvatarInspector/LeftFingers","AvatarInspector/RightArm","AvatarInspector/RightFingers","AvatarInspector/LeftLeg","AvatarInspector/RightLeg","AvatarInspector/HeadZoom","AvatarInspector/LeftHandZoom","AvatarInspector/RightHandZoom","AvatarInspector/DotFill","AvatarInspector/DotFrame","AvatarInspector/DotFrameDotted","AvatarInspector/DotSelection","SpeedScale","AvatarPivot","Avatar Icon","Mirror","AvatarInspector/BodySIlhouette","AvatarInspector/BodyPartPicker","AvatarInspector/MaskEditor_Root","AvatarInspector/LeftFeetIk","AvatarInspector/RightFeetIk","AvatarInspector/LeftFingersIk","AvatarInspector/RightFingersIk","BuildSettings.SelectedIcon","SocialNetworks.UDNLogo","SocialNetworks.LinkedInShare","SocialNetworks.FacebookShare","SocialNetworks.Tweet","SocialNetworks.UDNOpen","Clipboard","Toolbar Minus","ClothInspector.PaintValue","EditCollider","EyeDropper.Large","ColorPicker.CycleColor","ColorPicker.CycleSlider","PreTextureMipMapLow","PreTextureMipMapHigh","PreTextureAlpha","PreTextureRGB","Icon Dropdown","UnityLogo","Profiler.PrevFrame","Profiler.NextFrame","GameObject Icon","Prefab Icon","PrefabModel Icon","ScriptableObject Icon","sv_icon_none","PreMatLight0","PreMatLight1","Toolbar Plus","Camera Icon","PreMatSphere","PreMatCube","PreMatCylinder","PreMatTorus","PlayButton","PauseButton","HorizontalSplit","VerticalSplit","BuildSettings.Web.Small","js Script Icon","cs Script Icon","boo Script Icon","Shader Icon","TextAsset Icon","AnimatorController Icon","AudioMixerController Icon","RectTransformRaw","RectTransformBlueprint","MoveTool","MeshRenderer Icon","Terrain Icon","SceneviewLighting","SceneviewFx","SceneviewAudio","SettingsIcon","TerrainInspector.TerrainToolRaise","TerrainInspector.TerrainToolSetHeight","TerrainInspector.TerrainToolSmoothHeight","TerrainInspector.TerrainToolSplat","TerrainInspector.TerrainToolTrees","TerrainInspector.TerrainToolPlants","TerrainInspector.TerrainToolSettings","RotateTool","ScaleTool","RectTool","MoveTool On","RotateTool On","ScaleTool On","RectTool On","ViewToolOrbit","ViewToolMove","ViewToolZoom","ViewToolOrbit On","ViewToolMove On","ViewToolZoom On","StepButton","PlayButtonProfile","PlayButton On","PauseButton On","StepButton On","PlayButtonProfile On","Toolbar Plus More"
     };

    public List<string> systemIcons = new List<string>();
    string iconLabel = "";

    [MenuItem("Tools/System Icon")]
    public static void ShowWindow()
    {
        var window = EditorWindow.GetWindow<SystemIconListWindow>("SystemIcons");
        window.systemIcons = new();
        foreach (var item in window.icons)
        {
            var text = EditorGUIUtility.IconContent(item).image as Texture2D;
            if (text != null && text.name != null)
            {
                window.systemIcons.Add(text.name);
            }
        }
    }
    public Vector2 scrollPosition;
    void OnGUI()
    {
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);

        for (int i = 0; i < systemIcons.Count; i += 8)
        {
            GUILayout.BeginHorizontal();
            for (int j = 0; j < 8; j++)
            {
                int index = i + j;
                if (index < systemIcons.Count)
                {
                    if (GUILayout.Button(EditorGUIUtility.IconContent(systemIcons[index]), GUILayout.Width(50), GUILayout.Height(30)))
                    {
                        iconLabel = systemIcons[index];
                    }
                    GUILayout.Height(30);
                }
            }
            GUILayout.EndHorizontal();
        }
        GUILayout.EndScrollView();
        GUILayout.TextArea(iconLabel, GUILayout.Width(position.y), GUILayout.Height(30));
    }
}

