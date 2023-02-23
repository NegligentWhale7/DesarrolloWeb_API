using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrays : MonoBehaviour
{
    int[] nums = { 2, 14, 18, 22, 22 };
    private void Start()
    {
        //ContainsDuplicate(nums);
        Debug.Log(ContainsDuplicate(nums));
    }
    public bool ContainsDuplicate(int[] nums)
    {
        for (int i = 0; i < nums.Length; i++)
        {
            for (int j = 1; i <= (nums.Length - 1); j++)
            {                
                Debug.Log("i " + nums[i]);
                Debug.Log("j " + nums[j]);
                
                if (nums[i] == nums[j])
                {
                    return true;
                }
                
            }        
        }
        return false;
    }
}
