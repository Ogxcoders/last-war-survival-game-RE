namespace UnityEngine.Rendering;

[HelpURL("https://docs.unity3d.com/Packages/com.unity.render-pipelines.core@7.5/manual/Free-Camera.html")]
[ExecuteAlways]
public class FreeCamera : MonoBehaviour
{
	public float m_LookSpeedController = 120f;

	public float m_LookSpeedMouse = 10f;

	public float m_MoveSpeed = 10f;

	public float m_MoveSpeedIncrement = 2.5f;

	public float m_Turbo = 10f;

	private static string kMouseX = "Mouse X";

	private static string kMouseY = "Mouse Y";

	private static string kRightStickX = "Controller Right Stick X";

	private static string kRightStickY = "Controller Right Stick Y";

	private static string kVertical = "Vertical";

	private static string kHorizontal = "Horizontal";

	private static string kYAxis = "YAxis";

	private static string kSpeedAxis = "Speed Axis";

	private void OnEnable()
	{
		RegisterInputs();
	}

	private void RegisterInputs()
	{
	}

	private void Update()
	{
		if (DebugManager.instance.displayRuntimeUI)
		{
			return;
		}
		float num = 0f;
		float num2 = 0f;
		if (Input.GetMouseButton(1))
		{
			num = Input.GetAxis(kMouseX) * m_LookSpeedMouse;
			num2 = Input.GetAxis(kMouseY) * m_LookSpeedMouse;
		}
		num += Input.GetAxis(kRightStickX) * m_LookSpeedController * Time.deltaTime;
		num2 += Input.GetAxis(kRightStickY) * m_LookSpeedController * Time.deltaTime;
		float axis = Input.GetAxis(kSpeedAxis);
		if (axis != 0f)
		{
			m_MoveSpeed += axis * m_MoveSpeedIncrement;
			if (m_MoveSpeed < m_MoveSpeedIncrement)
			{
				m_MoveSpeed = m_MoveSpeedIncrement;
			}
		}
		float axis2 = Input.GetAxis(kVertical);
		float axis3 = Input.GetAxis(kHorizontal);
		float axis4 = Input.GetAxis(kYAxis);
		if (num != 0f || num2 != 0f || axis2 != 0f || axis3 != 0f || axis4 != 0f)
		{
			float x = base.transform.localEulerAngles.x;
			float y = base.transform.localEulerAngles.y + num;
			float num3 = x - num2;
			if (x <= 90f && num3 >= 0f)
			{
				num3 = Mathf.Clamp(num3, 0f, 90f);
			}
			if (x >= 270f)
			{
				num3 = Mathf.Clamp(num3, 270f, 360f);
			}
			base.transform.localRotation = Quaternion.Euler(num3, y, base.transform.localEulerAngles.z);
			float num4 = Time.deltaTime * m_MoveSpeed;
			num4 = ((!Input.GetMouseButton(1)) ? (num4 * ((Input.GetAxis("Fire1") > 0f) ? m_Turbo : 1f)) : (num4 * (Input.GetKey(KeyCode.LeftShift) ? m_Turbo : 1f)));
			base.transform.position += base.transform.forward * num4 * axis2;
			base.transform.position += base.transform.right * num4 * axis3;
			base.transform.position += Vector3.up * num4 * axis4;
		}
	}
}
