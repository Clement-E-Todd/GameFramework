using UnityEngine;

namespace ClementTodd.EventGraphs
{
    public class NumberComparisonNode : ConditionNode
    {
        public string numberAKey;
        public Properties.Scope numberAScope;

        public float numberB;

        public enum Operation
        {
            EqualTo,
            NotEqualTo,
            GreaterThan,
            LessThan,
            GreaterOrEqual,
            LessOrEqual
        }
        public Operation operation;

        protected override bool Compare()
        {
            // Get the first number to compare
            float numberA = 0;
            Properties numberAProperties = numberAScope == Properties.Scope.Local ?
                    DataGraph.localProperties :
                    EventGraphManager.Instance.globalProperties;

            if (numberAProperties.CompareType<int>(numberAKey))
                numberA = numberAProperties.Get<int>(numberAKey);
            else if (numberAProperties.CompareType<float>(numberAKey))
                numberA = numberAProperties.Get<float>(numberAKey);

            // Compare the two numbers using the indicated operation
            switch (operation)
            {
                case Operation.EqualTo:
                    return Mathf.Approximately(numberA, numberB);

                case Operation.NotEqualTo:
                    return !Mathf.Approximately(numberA, numberB);

                case Operation.GreaterThan:
                    return numberA > numberB;

                case Operation.LessThan:
                    return numberA < numberB;

                case Operation.GreaterOrEqual:
                    return numberA >= numberB;

                case Operation.LessOrEqual:
                    return numberA <= numberB;

                default:
                    return false;
            }

        }
    }
}