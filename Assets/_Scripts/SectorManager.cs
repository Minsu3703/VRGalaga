using UnityEngine;

struct SectionXYZ
{
    public int x, y, z;
}

struct Section
{
    public SectionXYZ sectionXYZ;
    public Transform root;
}

public class SectorManager : MonoBehaviour
{
    [SerializeField] private float sectionWidth;

    private Section[,,] sections = new Section[5, 5, 5];

    private void Start()
    {
        InitSections();
    }

    private void InitSections()
    {
        for (int i = 0; i < sections.GetLength(0); i++)
        {
            for (int j = 0; j < sections.GetLength(1); j++)
            {
                for (int k = 0; k < sections.GetLength(2); k++)
                {
                    Section section = new Section();
                    section.sectionXYZ = new SectionXYZ() { x = i - sections.GetLength(0), y = j - sections.GetLength(1), z = k - sections.GetLength(2) };
                    section.root = new GameObject($"Sector_{section.sectionXYZ.x}_{section.sectionXYZ.y}_{section.sectionXYZ.z}").transform;
                    section.root.SetParent(transform);
                    section.root.position = new Vector3(section.sectionXYZ.x, section.sectionXYZ.y, section.sectionXYZ.z) * sectionWidth;
                }
            }
        }
    }

    private void RollObjectsToXPlus()
    {
        for (int i = 0; i < sections.GetLength(0); i++)
        {
            for (int j = 0; j < sections.GetLength(1); j++)
            {
                sections[0, i, j].root.gameObject.SetActive(false);

                for (int k = 0; k < sections.GetLength(2); k++)
                {

                }
            }
        }
    }
}
