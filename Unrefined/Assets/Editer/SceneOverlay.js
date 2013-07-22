class SceneOverlay extends EditorWindow {
    @MenuItem ("Window/SceneOverlay")

    static function Init() {
        var window : SceneOverlay = EditorWindow.GetWindow(SceneOverlay);
    }
    
    function OnGUI() {
       	var mergerObject = GameObject.Find("SceneMerger");
       	if (!mergerObject) return;
       	
       	var merger = mergerObject.GetComponent.<SceneMerger>();

        if (GUILayout.Button("Load")) {
        	merger.ClearOverlay();
			merger.PushSceneObjects();
			merger.LoadScenesToOverlay();
			merger.PopSceneObjects();
        }
        
        if (GUILayout.Button("Clean up")) {
        	merger.ClearOverlay();
        }
    }
}
