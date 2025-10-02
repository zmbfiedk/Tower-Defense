using UnityEngine;

public class InvisibleEnemy : MonoBehaviour
{
    [SerializeField] private bool isInvisible = true;
    public bool IsInvisible => isInvisible;
}
