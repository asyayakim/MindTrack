using System.ComponentModel.DataAnnotations;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using CsvHelper.TypeConversion;

namespace MindTrack;

public class SocialAnxietyRecord
{
    [Name("Age")]
    public int Age { get; set; }

    [Name("Gender")]
    public string Gender { get; set; }

    [Name("Occupation")]
    public string Occupation { get; set; }

    [Name("Sleep Hours")]
    public double SleepingHours { get; set; }

    [Name("Physical Activity (hrs/week)")]
    public double PhysicalActivity { get; set; }

    [Name("Caffeine Intake (mg/day)")]
    public int CaffeineIntake { get; set; }

    [Name("Alcohol Consumption (drinks/week)")]
    public int AlcoholConsumption { get; set; }

    [Name("Smoking")]
    public string Smoking { get; set; }

    [Name("Family History of Anxiety")]
    public string FamilyHistoryOfAnxiety { get; set; }

    [Name("Stress Level (1-10)")]
    public int StressLevel { get; set; }

    [Name("Anxiety Level (1-10)")]
    public double AnxietyLevel { get; set; }

    [Name("Diet Quality (1-10)")]
    public int DietQuality { get; set; }
}
