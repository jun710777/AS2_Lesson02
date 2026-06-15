using UnityEditor;
using UnityEditor.Actions;
using UnityEngine;
using UnityEngine.UIElements;

public class UiMainGame : MonoBehaviour
{
    [Header("* * * UI Document * * *")]
    public UIDocument uid;

    private VisualElement _root;
    private Label _scoreText;
    private Button _gameQuitButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _root = uid.rootVisualElement;
        _scoreText = _root.Q<Label>("score-label");
        _scoreText.text = "ここにスコアの文字列を表示する";
        _gameQuitButton = _root.Q<Button>("game-quit-button");
        _gameQuitButton.clicked += () =>
        {
            Debug.Log("ゲーム終了");
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        };

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
