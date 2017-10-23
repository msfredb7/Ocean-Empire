using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExerciseComponents
{
    public static string GetDisplayName(ExerciseType type)
    {
        switch (type)
        {
            case ExerciseType.Walk:
                return "Marche";
            case ExerciseType.Run:
                return "Course";
            case ExerciseType.Stairs:
                return "Escaliers";
            default:
                return "";
        }
    }
    public static string GetDescription(ExerciseType type)
    {
        switch (type)
        {
            case ExerciseType.Walk:
                return "La marche à pied est une activité physique.";
            case ExerciseType.Run:
                return "La course à pied est une activité physique.";
            case ExerciseType.Stairs:
                return "Montez et descendez les escalier.";
            default:
                return "";
        }
    }
    public static ExerciseTracker GetTracker(ExerciseType type)
    {
        switch (type)
        {
            case ExerciseType.Walk:
                return new WalkTracker();
            case ExerciseType.Run:
                return null;
            case ExerciseType.Stairs:
                return null;
            default:
                return null;
        }
    }

    public static List<ExerciseType> GetAllTypes()
    {
        return new List<ExerciseType>()
        {
            ExerciseType.Walk,
            ExerciseType.Run,
            ExerciseType.Stairs
        };
    }
}
