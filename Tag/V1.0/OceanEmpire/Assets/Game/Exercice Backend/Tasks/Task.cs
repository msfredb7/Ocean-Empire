using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Task
{
    Reward reward = Reward_Tickets.Build(5);

    public void SetAutoReward(RewardType rewardType)
    {
        reward = RewardBuilder.AutoReward(this, rewardType);
    }

    public void SetReward(RewardType rewardType, int Amount = 0)
    {
        reward = RewardBuilder.ManualReward(rewardType, Amount);
    }

    public Reward GetReward()
    {
        return reward;
    }
    public abstract ExerciseType GetExerciseType();
    public abstract TimeSpan GetAllocatedTime();

    public override string ToString()
    {
        return "Reward:\n" + reward.ToString();
    }
}