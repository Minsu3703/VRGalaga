using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    private enum HUDType
    {
        Speed,
        Score,
        HP,
        Ammo
    }

    [SerializeField] private HUDType type;
    [SerializeField] Image image;
    [SerializeField] private Transform player;

    private Text text;

    // 나중에 Score, Ammo는 Update에서 동작 안하고 필요할 때만 호출하도록 변경하는 것도 필요.
    private void Update()
    {
        switch (type)
        {
            case HUDType.Speed:
                Rigidbody playerRigidbody = player.GetComponentInParent<Rigidbody>();
                image.fillAmount = playerRigidbody.velocity.magnitude > 100f ? 1f : playerRigidbody.velocity.magnitude / 100;
                break;

            case HUDType.Score:
                text = GetComponent<Text>();
                text.text = GameManager.Instance.Score.ToString();
                break;

            case HUDType.HP:
                PlayerController playerController = player.GetComponentInParent<PlayerController>();
                image.fillAmount = playerController.HP / 1000;
                break;
        }
    }
}
