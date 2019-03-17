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

    private const int MAX_POINT = 15;

    [SerializeField]
    private Text notice;
    [SerializeField]
    private GameObject resetButton;

    private List<Score> scoreList = new List<Score>();

    private Team currentServeTeam;

    private bool finished;

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

    public void OnTapBack()
    {
        scoreList.RemoveAt(scoreList.Count - 1);
        Refresh();
    }

    public void OnTapReset()
    {
        scoreList.Clear();
        resetButton.SetActive(false);
        finished = false;
        notice.text = "waiting...";
    }

    private void Refresh()
    {
        var justNowScore = GetLastScore();
        if (!finished && MAX_POINT <= GetCurrentPoint(justNowScore.team))
        {
            finished = true;
            resetButton.SetActive(true);
        }

        notice.text = CreateString(currentServeTeam, justNowScore.team);
        currentServeTeam = justNowScore.team;
    }

    private string CreateString(Team currentServeTeam, Team nextServeTeam)
    {
        if (finished)
        {
            return "Game set";
        }

        var justNowTeamPoint = GetCurrentPoint(nextServeTeam);
        var otherTeamPoint = GetCurrentPoint(GetOtherTeam(nextServeTeam));

        var str = "";

        // サーブ権ではなくポイントが入ったかどうか
        if (1 < scoreList.Count)
        {
            str += (currentServeTeam == nextServeTeam) ? "Point " : "Over ";
        }

        // ポイント表示
        if (justNowTeamPoint == otherTeamPoint)
        {
            str += string.Format("{0} all", GetPointString(justNowTeamPoint));
            if (justNowTeamPoint == 0)
            {
                // 初回だけ play って言う
                str += " play";
            }
        }
        else
        {
            str += string.Format("{0} - {1}", GetPointString(justNowTeamPoint), GetPointString(otherTeamPoint));
        }

        if (justNowTeamPoint == MAX_POINT - 1)
        {
            str += " match point";
        }

        return str;
    }

    private void AddScore(Score score)
    {
        if (finished)
        {
            return;
        }

        var first = scoreList.Count == 0;
        if (first)
        {
            currentServeTeam = score.team;
        }
        // 初回はサーブ権のみ記録するため0点
        var point = first ? 0 : 1;
        score.point = point;

        scoreList.Add(score);
    }

    private int GetCurrentPoint(Team team)
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

    private Score GetLastScore()
    {
        return scoreList[scoreList.Count - 1];
    }

    private string GetPointString(int point)
    {
        if (point == 0)
        {
            return "love";
        }
        else
        {
            return string.Format("{0}", point);
        }
    }

    private Team GetOtherTeam(Team team)
    {
        if (team == Team.LEFT)
        {
            return Team.RIGHT;
        }
        else
        {
            return Team.LEFT;
        }
    }
}
