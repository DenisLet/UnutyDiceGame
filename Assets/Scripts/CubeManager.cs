using UnityEngine;

public class CubeManager : MonoBehaviour
{
    public GameObject cubePrefab; // Префаб кубика
    public float verticalSpacing = 2.0f; // Расстояние между кубиками по высоте
    public float horizontalVariance = 0.5f; // Максимальное смещение кубиков по горизонтали

    private float currentHeight = 0f; // Текущая высота, на которой размещается новый кубик

    // Метод для обновления количества кубиков
    public void UpdateCubes(int count)
    {
        // Удаляем все старые кубики
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // Сбрасываем текущую высоту
        currentHeight = 0f;

        // Создаем новое количество кубиков
        for (int i = 0; i < count; i++)
        {
            GameObject cube = Instantiate(cubePrefab);

            // Генерируем случайные смещения по горизонтали
            float randomX = Random.Range(-horizontalVariance, horizontalVariance);
            float randomZ = Random.Range(-horizontalVariance, horizontalVariance);

            // Устанавливаем позицию кубика с учетом смещения
            cube.transform.position = new Vector3(randomX, currentHeight, randomZ);
            cube.transform.parent = transform;

            // Обновляем текущую высоту
            currentHeight += verticalSpacing;
        }
    }
}
