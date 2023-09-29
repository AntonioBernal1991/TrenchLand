using UnityEngine;

public class GridSystemVisualSingle : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;

    // Show a visual Grid and set its material
    public void Show(Material material)
    {
        // Enable the MeshRenderer to make the object visible
        meshRenderer.enabled = true;

        // Set the material of the MeshRenderer to the specified material
        meshRenderer.material = material;
    }

    // Hide the visual Grid.
    public void Hide()
    {
        // Disable the MeshRenderer to make the object invisible
        meshRenderer.enabled = false;
    }
}