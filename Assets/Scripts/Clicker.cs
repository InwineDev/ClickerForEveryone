using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class Clicker : NetworkBehaviour
{
    public Text scoreText;
    public InputField inputField;
    public Button applyButton;

    [SyncVar(hook = nameof(OnScoreChanged))]
    public int score;

    void Start()
    {
        UpdateScoreText();
        applyButton.onClick.AddListener(OnApplyButtonClicked);
    }

    private void OnApplyButtonClicked()
    {
        if (int.TryParse(inputField.text, out int newScore))
        {
            CmdSetClicks(newScore);
        }
    }

    public void OnClick()
    {
        CmdIncreaseScore();
    }

    [Command(requiresAuthority = false)]
    private void CmdIncreaseScore()
    {
        score++;
    }

    [Command(requiresAuthority = false)]
    public void CmdSetClicks(int newScore)
    {
        score = newScore;
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = score.ToString();
        }
    }

    private void OnScoreChanged(int oldScore, int newScore)
    {
        UpdateScoreText();
    }

    public override void OnStartServer()
    {
        score = 0;
    }
}
