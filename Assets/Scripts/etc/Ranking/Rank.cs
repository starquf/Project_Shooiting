using System;

[Serializable]
public class Rank
{
    public PlayerType playerType;
    public int stage;
    public int score;

    public Rank(PlayerType playerType, int stage, int score)
    {
        this.playerType = playerType;
        this.stage = stage;
        this.score = score;
    }
}
