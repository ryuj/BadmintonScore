using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    private enum Team
    {
        LEFT,
        RIGHT,
    }

    private class Score
    {
        public int point;
        public Team team
        {
            get;
        }

        public Score(Team team)
        {
            this.team = team;
        }
    }

    [SerializeField]
    private Text notice;

    private List<Score> scoreList = new List<Score>();

    public void OnTapLeft()
    {
        AddScore(new Score(Team.LEFT));
        Refresh();
    }

    public void OnTapRight()
    {
        AddScore(new Score(Team.RIGHT));
        Refresh();
    }

    private void Refresh()
    {
        notice.text = string.Format("{0} - {1}", GetCurrentScore(Team.LEFT), GetCurrentScore(Team.RIGHT));
    }

    private void AddScore(Score score)
    {
        var point = scoreList.Count == 0 ? 0 : 1;
        score.point = point;

        scoreList.Add(score);
    }

    private int GetCurrentScore(Team team)
    {
        var ret = 0;
        foreach (var v in scoreList)
        {
            if (v.team == team)
            {
                ret += v.point;
            }
        }
        return ret;
    }
}
