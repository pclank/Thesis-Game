﻿using UnityEngine;
using System;
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

    [Tooltip("Happiness Material for Calendar.")]
    public Material happiness_calendar_material;

    [Tooltip("Sadness Material for Calendar.")]
    public Material sadness_calendar_material;

    [Tooltip("Anger Material for Calendar.")]
    public Material anger_calendar_material;

    [Tooltip("Happiness Material for Walls.")]
    public Material happiness_wall_material;

    [Tooltip("Happiness Material for Floor.")]
    public Material happiness_floor_material;

    [Tooltip("Sadness Material for Walls.")]
    public Material sadness_wall_material;

    [Tooltip("Sadness Material for Floor.")]
    public Material sadness_floor_material;

    [Tooltip("Anger Material for Walls.")]
    public Material anger_wall_material;

    [Tooltip("Anger Material for Floor.")]
    public Material anger_floor_material;

    [Header("GameObjects to Reference for Material Application")]
    [Tooltip("Book GameObject.")]
    public GameObject book_object;

    [Tooltip("Book Prefabs.")]
    public GameObject[] book_prefabs = new GameObject[3];

    [Tooltip("Plant GameObject.")]
    public GameObject plant_object;

    [Tooltip("Plant Prefabs.")]
    public GameObject[] plant_prefabs = new GameObject[3];

    [Tooltip("Calendar GameObject.")]
    public GameObject calendar_object;

    [Tooltip("Main Wall GameObject.")]
    public GameObject main_wall;

    [Tooltip("Extra Wall GameObject.")]
    public GameObject extra_wall;

    [Tooltip("Floor GameObject.")]
    public GameObject floor;

    [Header("Padlock Puzzle Options")]
    [Tooltip("Padlock Happiness Combination.")]
    public int[] happiness_combination;

    [Tooltip("Padlock Sadness Combination.")]
    public int[] sadness_combination;

    [Tooltip("Padlock Anger Combination.")]
    public int[] anger_combination;

    [Header("Development Options Section")]
    [Tooltip("Enable Development Mode.")]
    public bool development_mode = false;

    [Tooltip("Emotion Index for Development Mode.")]
    public int development_index = 0;

    // ************************************************************************************
    // Member Functions
    // ************************************************************************************

    // Start Setup
    private void startSetup(int emotion_index)
    {
        // Happiness Detected
        if (emotion_index == 0)
        {
            setHappinessMaterials();
            setHappinessCombination();

            main_wall.GetComponent<MixShader>().enabled = false;
            extra_wall.GetComponent<MixShader>().enabled = false;
            floor.GetComponent<MixShader>().enabled = false;

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
        else
        {
            setHappinessMaterials();
            setHappinessCombination();

            main_wall.GetComponent<MixShader>().enabled = false;
            extra_wall.GetComponent<MixShader>().enabled = false;
            floor.GetComponent<MixShader>().enabled = false;

            happiness_projector.SetActive(true);
        }

        book_object.GetComponentInParent<ExaminableItem>().gb_prefab = book_prefabs[emotion_index];
        plant_object.GetComponentInParent<ExaminableItem>().gb_prefab = plant_prefabs[emotion_index];

        Destroy(this);                          // Destroy Script after Setup
    }

    // Change Book and Plant Materials
    private void changeMaterials(Material book_mat, Material plant_mat, Material calendar_mat, Material wall_mat, Material floor_mat)
    {
        book_object.GetComponent<MeshRenderer>().material = book_mat;
        plant_object.GetComponent<MeshRenderer>().material = plant_mat;
        calendar_object.GetComponent<MeshRenderer>().material = calendar_mat;
        main_wall.GetComponent<MeshRenderer>().material = wall_mat;
        extra_wall.GetComponent<MeshRenderer>().material = wall_mat;
        floor.GetComponent<MeshRenderer>().material = floor_mat;
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

        if (development_mode)
            startSetup(development_index);
    }

    void OnEnable()
    {
        Tuple<int, float> prediction = GameObject.FindWithTag("Player").GetComponent<JSONReader>().readEmotionIndex();  // Get Prediction

        if (!development_mode)
            startSetup(prediction.Item1);
    }

    // ************************************************************************************
    // Lambda Functions
    // ************************************************************************************

    void setHappinessMaterials() => changeMaterials(happiness_book_material, happiness_plant_material, happiness_calendar_material, happiness_wall_material, happiness_floor_material);
    void setHappinessCombination() => changeCombination(happiness_combination);
    void setSadnessMaterials() => changeMaterials(sadness_book_material, sadness_plant_material, sadness_calendar_material, sadness_wall_material, sadness_floor_material);
    void setSadnessCombination() => changeCombination(sadness_combination);
    void setAngerMaterials() => changeMaterials(anger_book_material, anger_plant_material, anger_calendar_material, anger_wall_material, anger_floor_material);
    void setAngerCombination() => changeCombination(anger_combination);
}