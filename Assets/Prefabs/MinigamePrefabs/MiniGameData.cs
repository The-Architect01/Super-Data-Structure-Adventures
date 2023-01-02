using UnityEngine;
using System;

[Serializable]
public class MiniGameData {
    [TextArea(3,300)]
    public string DataStructureInfo;
    [TextArea(3,300)]
    public string HowToPlay;

    public void OnAwake(GameObject go) {
//        try {
//            Zombie.MinigameDB.Add(go.scene.name, new string[] { HowToPlay, DataStructureInfo });
            Zombie.MiniGameList.SetSceneData(go.scene.name, HowToPlay, DataStructureInfo);
//        } catch {}
    } 
}
