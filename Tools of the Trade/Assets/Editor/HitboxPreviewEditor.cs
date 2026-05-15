using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

[CustomEditor(typeof(HitboxPreview))]
public class HitboxPreviewEditor : Editor {
    private int selectedClipIndex = 0;
    private int currentFrame = 0;

    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        HitboxPreview preview = (HitboxPreview)target;

        // doesnt do anything in play mode
        if (EditorApplication.isPlaying) {
            EditorGUILayout.HelpBox("Hitbox preview disabled during play mode.", MessageType.Info);
            return;
        }

        // make sure all required components are non-null
        if (preview.moveset == null || preview.animator == null) return;
        if (preview.moveset.clipBindings == null || preview.moveset.clipBindings.Count == 0) {
            EditorGUILayout.HelpBox("No clip bindings on moveset.", MessageType.Warning);
            return;
        }

        // build dropdown from moveset bindings instead of raw animator clips
        string[] bindingNames = preview.moveset.clipBindings
            .Select(b => b.clip != null ? b.clip.name : "(no clip)")
            .ToArray();

        EditorGUI.BeginChangeCheck();
        selectedClipIndex = EditorGUILayout.Popup("Preview Clip", selectedClipIndex, bindingNames);
        if (EditorGUI.EndChangeCheck()) {
            preview.selectedBindingIndex = selectedClipIndex;
            currentFrame = 0; // reset scrubber on clip change
        }

        AnimationClip selectedClip = preview.moveset.clipBindings[selectedClipIndex].clip;
        if (selectedClip == null) {
            EditorGUILayout.HelpBox("Assign a clip to this binding.", MessageType.Warning);
            return;
        }

        int totalFrames = Mathf.RoundToInt(selectedClip.frameRate * selectedClip.length);

        // Frame scrubber
        EditorGUI.BeginChangeCheck();
        currentFrame = EditorGUILayout.IntSlider("Frame", currentFrame, 0, totalFrames - 1);
        preview.currentFrame = currentFrame;

        if (EditorGUI.EndChangeCheck()) {
            // only sample when the slider actually changed
            preview.animator.enabled = false;
            Vector3 originalPos = preview.animator.transform.position;
            Quaternion originalRot = preview.animator.transform.rotation;

            float time = currentFrame / (float)selectedClip.frameRate;
            selectedClip.SampleAnimation(preview.animator.gameObject, time);

            preview.animator.transform.position = originalPos;
            preview.animator.transform.rotation = originalRot;

            SceneView.RepaintAll();
        }

        // show active window info for each hitbox in the binding
        List<AttackData> hitboxes = preview.moveset.clipBindings[selectedClipIndex].hitboxes;
        if (hitboxes == null || hitboxes.Count == 0) {
            EditorGUILayout.HelpBox("No hitboxes on this binding.", MessageType.None);
            return;
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Hitboxes", EditorStyles.boldLabel);
        foreach (AttackData data in hitboxes) {
            if (data == null) continue;
            int start = data.startupFrames;
            int end = start + data.activeFrames - 1;
            bool isActive = currentFrame >= start && currentFrame <= end;
            EditorGUILayout.HelpBox(
                $"{data.name} | Active frames: {start} – {end} | {(isActive ? "ACTIVE" : "inactive")}",
                isActive ? MessageType.Warning : MessageType.None
            );
        }
    }
}