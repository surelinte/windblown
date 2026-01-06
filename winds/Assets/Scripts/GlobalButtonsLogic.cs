using UnityEngine;

public class GlobalButtonsLogic : MonoBehaviour
{
    public void MainMenuButton()
    {
        GameFlow.Instance?.GoToMainMenu();
    }
    public void BattleButton()
    {
        GameFlow.Instance?.GoToBattle();
    }
    public void StoryButton()
    {
        GameFlow.Instance?.GoToStory();
    }
}
