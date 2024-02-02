using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlexibleLayoutGroup : LayoutGroup
{
    private enum GridScalingType
    {
        FreeAll, FixColumsCount, FixRowsCount
    }

    [SerializeField, Range(1,100)]private int _colums = 1;
    [SerializeField, Range (1,100)]private int _rows = 1;
    [SerializeField] private Vector2 _spacing;

    [SerializeField] private GridScalingType _gridScalingType;

    


    private float ParentWidth => rectTransform.rect.width;
    private float ParentHeight => rectTransform.rect.height;
    public override void CalculateLayoutInputHorizontal()
    {
        base.CalculateLayoutInputHorizontal();

        int childCount = rectTransform.childCount;

        if (childCount==0)
            return;
        
        if (_gridScalingType == GridScalingType.FreeAll)
        {
            float childCountSqrt = MathF.Sqrt(childCount);
            _colums = Mathf.CeilToInt(childCountSqrt);
            _rows = Mathf.CeilToInt(childCountSqrt);
        }
        else if (_gridScalingType == GridScalingType.FixColumsCount)
        {
            _rows = childCount/_colums;

            if (_rows<=0)
                _rows = 1;
        }
        else if (_gridScalingType == GridScalingType.FixRowsCount)
        {
            _colums = childCount/_rows;

            if (_colums<=0)
                _colums = 1;
        }
        
        

        float cellWidth = ParentWidth/(float)_colums-_spacing.x/_colums*2f-padding.left/_colums-padding.right/_colums;
        float cellHeight = ParentHeight/(float)_rows-_spacing.y/_rows*2f-padding.top/_rows-padding.bottom/_rows;

        for (int i = 0;i<childCount;i++)
        {
            int row = i%_rows;
            int column = i/_rows;

            var item = rectChildren[i];

            float xPos = cellWidth*column+_spacing.x*column/2f+padding.left;
            float yPos = cellHeight*row+_spacing.y*row/2f+padding.top;

            SetChildAlongAxis(item,0,xPos,cellWidth);
            SetChildAlongAxis(item,1,yPos, cellHeight);
        }
    }

    public override void CalculateLayoutInputVertical()
    {
        
    }

    public override void SetLayoutHorizontal()
    {
        
    }

    public override void SetLayoutVertical()
    {
        
    }
}
