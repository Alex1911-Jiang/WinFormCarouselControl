using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarouselControl
{
    public static class IndexHelper
    {
        public static int Binarysearch(this int[] array, int targetNum)
        {
            if (targetNum< array[0])
                return array[0];
            if (targetNum > array[array.Length - 1])
                return array[array.Length - 1];

            int left = 0;
            int right;
            for (right = array.Length - 1; left != right;)
            {
                int midIndex = (right + left) / 2;
                int mid = (right - left);
                int midValue = array[midIndex];
                if (targetNum == midValue)
                    return array[midIndex];
                if (targetNum > midValue)
                    left = midIndex;
                else
                    right = midIndex;

                if (mid <= 1)
                    break;
            }

            int rightNum = array[right];
            int leftNum = array[left];
            int closedNum = Math.Abs((rightNum - leftNum) / 2) > Math.Abs(rightNum - targetNum) ? rightNum : leftNum;
            return closedNum;
        }
    }
}
