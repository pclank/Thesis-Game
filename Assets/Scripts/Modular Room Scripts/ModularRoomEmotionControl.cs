using UnityEngine;
using System.Collections;

// ************************************************************************************
// Modular Room Control Script
// ************************************************************************************

public class ModularRoomEmotionControl : MonoBehaviour
{
    // ************************************************************************************
    // Public Variables
    // ************************************************************************************

    [Header("GameObjects and Materials to Enable on Emotion")]
    [Tooltip("Happiness Decal Projector GameObject.")]
    public GameObject happiness_projector;

    [Tooltip("Sadness Decal Projector GameObject.")]
    public GameObject sadness_projector;

    [Tooltip("Anger Decal Projector GameObject.")]
    public GameObject anger_projector;

    [Tooltip("Happiness Material for Book.")]
    public Material happiness_book_material;

    [Tooltip("Sadness Material for Book.")]
    public Material sadness_book_material;

    [Tooltip("Anger Material for Book.")]
    public Material anger_book_material;

    [Tooltip("Happiness Material for Plant.")]
    public Material happiness_plant_material;

    [Tooltip("Sadness Material for Plant.")]
    public Material sadness_plant_material;

    [Tooltip("Anger Material for Plant.")]
    public Material anger_plant_material;

    [Header("GameObjects to Reference for Material Application")]
    [Tooltip("Book GameObject.")]
    public GameObject book_object;

    [Tooltip("Plant GameObject.")]
    public GameObject plant_object;

    [Header("Padlock Puzzle Options")]
    [Tooltip("Padlock Happiness Combination.")]
    public int[] happiness_combination;

    [Tooltip("Padlock Sadness Combination.")]
    public int[] sadness_combination;

    [Tooltip("Padlock Anger Combination.")]
    public int[] anger_combination;

    // ************************************************************************************
    // Member Functions
    // ************************************************************************************

    // Start Setup
    public void startSetup(int emotion_index)
    {
        // Happiness Detected
        if (emotion_index == 0)
        {
            setHappinessMaterials();
            setHappinessCombination();

            happiness_projector.SetActive(true);
        }
        // Sadness Detected
        else if (emotion_index == 1)
        {
            setSadnessMaterials();
            setSadnessCombination();

            sadness_projector.SetActive(true);
        }
        // Anger Detected
        else if (emotion_index == 2)
        {
            setAngerMaterials();
            setAngerCombination();

            anger_projector.SetActive(true);
        }
    }

    // Change Book and Plant Materials
    private void changeMaterials(Material book_mat, Material plant_mat)
    {
        book_object.GetComponent<MeshRenderer>().material = book_mat;
        plant_object.GetComponent<MeshRenderer>().material = plant_mat;
    }

    // Change Padlock Puzzle Combination
    private void changeCombination(int[] solution)
    {
        GameObject.FindWithTag("PadLock").GetComponent<PadLock>().code_solution = solution;
    }

    // Use this for initialization
    void Start()
    {
        // Check that Combinations are Valid

        int max_value = GameObject.FindWithTag("PadLock").GetComponent<PadLock>().max_value;        // Get Max Valid Value

        for (int i = 0; i < happiness_combination.Length; i++)
        {
            if (happiness_combination[i] > max_value || sadness_combination[i] > max_value || anger_combination[i] > max_value)
                Debug.LogError("Invalid Combination!");
        }

        // Disable Decal Projectors on Start

        happiness_projector.SetActive(false);
        sadness_projector.SetActive(false);
        anger_projector.SetActive(false);
    }

    // ************************************************************************************
    // Lambda Functions
    // ************************************************************************************

    void setHappinessMaterials() => changeMaterials(happiness_book_material, happiness_plant_material);
    void setHappinessCombination() => changeCombination(happiness_combination);
    void setSadnessMaterials() => changeMaterials(sadness_book_material, sadness_plant_material);
    void setSadnessCombination() => changeCombination(sadness_combination);
    void setAngerMaterials() => changeMaterials(anger_book_material, anger_plant_material);
    void setAngerCombination() => changeCombination(anger_combination);
}