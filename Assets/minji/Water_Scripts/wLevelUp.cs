using UnityEngine;

public class wLevelUp : MonoBehaviour
{

    public int wLevel = 1; // 현재 레벨
    public int wExperience = 0; // 현재 경험치
    public int wExperienceToNextLevel = 100; // 다음 레벨까지 필요한 경험치

    void Start()
    {
        Debug.Log($"레벨 {wLevel}, 경험치 {wExperience}/{wExperienceToNextLevel}");
    }

    public void GainExperience(int amount)
    {
        wExperience += amount;
        Debug.Log($"경험치 획득: {amount}, 현재 경험치: {wExperience}/{wExperienceToNextLevel}");

        while (wExperience >= wExperienceToNextLevel) // 경험치가 초과하면 레벨업
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        wExperience -= wExperienceToNextLevel; //남은 경험치 이월
        wLevel++; // 레벨업
        wExperienceToNextLevel = Mathf.RoundToInt(wExperienceToNextLevel * 1.2f); // 다음 레벨업 경험치 증가
        Debug.Log($"레벨업! 현재 레벨: {wLevel}, 다음 레벨까지 필요 경험치: {wExperienceToNextLevel}");
    }
}
