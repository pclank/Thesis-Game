using UnityEngine;
using System.Collections;

public class KnowledgeOnPickup : MonoBehaviour
{
    [Tooltip("Use for Item that is Targeted by Others.")]
    public bool no_knowledge = false;

    public int target_item_id = 0;
    public int target_knowledge_level = 0;

    // Message from Inventory that Item was Picked Up, and Increase Knowledge

    public void increaseKnowledge()
    {
        // Make Sure Editor Values are Valid
        if (!no_knowledge && target_item_id > 0 && target_knowledge_level > 0)
        {
            GameObject.FindWithTag("Player").GetComponent<main_inventory>().increaseKnowledge(target_item_id, target_knowledge_level);
        }
    }
}