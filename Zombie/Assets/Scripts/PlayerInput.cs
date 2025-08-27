using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public static readonly string AxisVertical = "Vertical";
    public static readonly string AxisHorizontal = "Horizontal";
    public static readonly string Fire_1 = "Fire1";
    public static readonly string Reload_1 = "Reload";
    public float Move { get; private set; }
    public float Roate { get; private set; }
    public bool Fire { get; private set; }
    public bool Reload { get; private set; }

    private void Update()
    {
        Move = Input.GetAxis(AxisVertical);
        Roate = Input.GetAxis(AxisHorizontal);

        Fire = Input.GetButton(Fire_1); // ���콺�� ���� Ű����� ���� ��Ʈ�� ������Ʈ ���ÿ� Ű ��������
        Reload = Input.GetButtonDown(Reload_1);
    }
}
