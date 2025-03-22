using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragCamera : MonoBehaviour
{
    public float leftBoundary = -10.0f; // ��߽�
    public float rightBoundary = 10.0f; // �ұ߽�
    public float frontBoundary = -10.0f; // ǰ�߽�
    public float backBoundary = 10.0f; // ��߽�
    public float minY = -5.0f; // Y ����Сֵ
    public float maxY = 5.0f; // Y �����ֵ
    public float scrollSpeed = 1.0f; // �����ֿ�����������ŵ��ٶ�
    public float focusDistance = 5.0f; // �������Ŀ������ľ���
    public LayerMask targetLayer; // Ŀ���������ڵ�ͼ�㣨����Ϊ "Area"��
    public float smoothSpeed;

    private Camera mainCamera;
    private float currentYPosition; // ��ǰ������� Y ��λ��
    private bool isMoving;

    private void Start()
    {
        mainCamera = Camera.main;
        currentYPosition = mainCamera.transform.position.y; // ���浱ǰ Y ��λ��
        //targetCameraPosition = mainCamera.transform.position;
    }

    private void Update()
    {
        // ������������
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // ���߼��
            if (Physics.Raycast(ray, out hit, 500f, targetLayer) && !EventSystem.current.IsPointerOverGameObject())
            {
                // ���������Ŀ�����壬�����������λ��
                Vector3 targetPosition = hit.collider.gameObject.transform.position;
                Vector3 cameraPosition = new Vector3(0, 0, 0);
                // �������������λ��
                switch (hit.collider.gameObject.name)
                {
                    case "North":
                        cameraPosition = new Vector3(-20, 0, -10);
                        break;
                    case "West":
                        cameraPosition = new Vector3(-150, 0, -140);
                        break;
                    case "South":
                        cameraPosition = new Vector3(-20, 0, -270);
                        break;
                    case "Center":
                        cameraPosition = new Vector3(-20, 0, -140);
                        break;
                    case "WestSouth":
                        cameraPosition = new Vector3(-150, 0, -270);
                        break;
                }

                cameraPosition.y = currentYPosition; // ���ֵ�ǰ Y ��λ��

                // Ӧ���µ�λ��
                //mainCamera.transform.position = cameraPosition;
                //mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, cameraPosition, smoothSpeed);
                StartCoroutine(MoveCameraToPosition(cameraPosition));
            }
        }

        // �������м���ק
        if (Input.GetMouseButton(2)) // ��ס����м���ק
        {
            float deltaX = Input.GetAxis("Mouse X");
            float deltaZ = Input.GetAxis("Mouse Y"); // ʹ����� Y ����Ϊ Z ����ƶ�

            // ������ƶ�����ת��Ϊ���������е��ƶ�����
            Vector3 moveDistance = new Vector3(deltaX, 0, deltaZ) * mainCamera.orthographicSize * 0.5f;

            // ���������λ��
            Vector3 newPosition = mainCamera.transform.position - mainCamera.transform.rotation * moveDistance;

            // ������������ƶ���Χ
            newPosition.x = Mathf.Clamp(newPosition.x, leftBoundary, rightBoundary);
            newPosition.z = Mathf.Clamp(newPosition.z, frontBoundary, backBoundary);

            // ȷ�� Y ��λ�ñ��ֲ���
            newPosition.y = currentYPosition;

            // ���������λ��
            mainCamera.transform.position = newPosition;
        }

        // ʹ�������ֿ���������� Z ��� Y ���Ͽ�����Զ��
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        Vector3 scrollPosition = new Vector3(0, -scroll * scrollSpeed, scroll * scrollSpeed);

        // ���������λ��
        Vector3 newScrollPosition = mainCamera.transform.position + scrollPosition;

        // ����������� Y ��λ��
        newScrollPosition.y = Mathf.Clamp(newScrollPosition.y, minY, maxY);

        // ����������� Z ��λ��
        newScrollPosition.z = Mathf.Clamp(newScrollPosition.z, frontBoundary, backBoundary);

        // ���������λ��
        mainCamera.transform.position = newScrollPosition;

        // ���µ�ǰ Y ��λ��
        currentYPosition = mainCamera.transform.position.y;
    }

    private IEnumerator MoveCameraToPosition(Vector3 targetPosition)
    {
        isMoving = true;
        Vector3 startPosition = mainCamera.transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < smoothSpeed)
        {
            mainCamera.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / smoothSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        mainCamera.transform.position = targetPosition;
        isMoving = false;
    }
}
