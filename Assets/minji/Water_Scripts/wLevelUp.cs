using UnityEngine;

public class wLevelUp : MonoBehaviour
{

    public int wLevel = 1; // ���� ����
    public int wExperience = 0; // ���� ����ġ
    public int wExperienceToNextLevel = 100; // ���� �������� �ʿ��� ����ġ

    void Start()
    {
        Debug.Log($"���� {wLevel}, ����ġ {wExperience}/{wExperienceToNextLevel}");
    }

    public void GainExperience(int amount)
    {
        wExperience += amount;
        Debug.Log($"����ġ ȹ��: {amount}, ���� ����ġ: {wExperience}/{wExperienceToNextLevel}");

        while (wExperience >= wExperienceToNextLevel) // ����ġ�� �ʰ��ϸ� ������
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        wExperience -= wExperienceToNextLevel; //���� ����ġ �̿�
        wLevel++; // ������
        wExperienceToNextLevel = Mathf.RoundToInt(wExperienceToNextLevel * 1.2f); // ���� ������ ����ġ ����
        Debug.Log($"������! ���� ����: {wLevel}, ���� �������� �ʿ� ����ġ: {wExperienceToNextLevel}");
    }
}
